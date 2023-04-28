using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;
using GameCore.Services;
using System.Linq;
using GameCore.UI;
using GameCore.Data;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace HyperCasual.Runner
{
    /// <summary>
    /// A class used to store game state information,
    /// load levels, and save/load statistics as applicable.
    /// The GameManager class manages all game-related
    /// state changes.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Returns the GameManager.
        /// </summary>
        public static GameManager Instance => s_Instance;

        private static GameManager s_Instance;

        [SerializeField]
        private AbstractGameEvent m_WinEvent;

        [SerializeField]
        private AbstractGameEvent m_LoseEvent;

        private LevelDefinition m_CurrentLevel;

        private const int k_NumberOfGates = 4;

        /// <summary>
        /// Returns true if the game is currently active.
        /// Returns false if the game is paused, has not yet begun,
        /// or has ended.
        /// </summary>
        public bool IsPlaying => m_IsPlaying;

        private bool m_IsPlaying;
        private GameObject m_CurrentLevelGO;
        private GameObject m_CurrentTerrainGO;
        private GameObject m_LevelMarkersGO;

        private static LevelManager s_LevelManager;
        private static List<Color> s_LevelColors = new();
        public IMatchService MatchService { get; } = new MatchService();

#if UNITY_EDITOR
        private bool m_LevelEditorMode;
#endif

        private void Awake()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;

#if UNITY_EDITOR
            // If LevelManager already exists, user is in the LevelEditorWindow
            if (LevelManager.Instance != null)
            {
                //StartGame();
                m_LevelEditorMode = true;
            }
#endif
        }

        /// <summary>
        /// This method calls all methods necessary to load and
        /// instantiate a level from a level definition.
        /// </summary>
        public void LoadLevel(LevelDefinition levelDefinition)
        {
            m_CurrentLevel = levelDefinition;
            LoadLevel(m_CurrentLevel, ref m_CurrentLevelGO);
            CreateTerrain(m_CurrentLevel, ref m_CurrentTerrainGO);
            PlaceLevelMarkers(m_CurrentLevel, ref m_LevelMarkersGO);
            StartGame(levelDefinition.PlayerSpeed);
        }

        /// <summary>
        /// This method calls all methods necessary to restart a level,
        /// including resetting the player to their starting position
        /// </summary>
        public void ResetLevel(PlayerSpeedPreset playerSpeed)
        {
            if (PlayerController.Instance != null)
            {
                PlayerController.Instance.ResetPlayer(playerSpeed);
            }

            CameraManager.Instance.ResetCamera();

            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.ResetSpawnables();
            }
        }

        /// <summary>
        /// This method loads and instantiates the level defined in levelDefinition,
        /// storing a reference to its parent GameObject in levelGameObject
        /// </summary>
        /// <param name="levelDefinition">
        /// A LevelDefinition ScriptableObject that holds all information needed to
        /// load and instantiate a level.
        /// </param>
        /// <param name="levelGameObject">
        /// A new GameObject to be created, acting as the parent for the level to be loaded
        /// </param>
        public static void LoadLevel(LevelDefinition levelDefinition, ref GameObject levelGameObject)
        {
            if (levelDefinition == null)
            {
                Debug.LogError("Invalid Level!");
                return;
            }

            if (levelGameObject != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(levelGameObject);
                }
                else
                {
                    DestroyImmediate(levelGameObject);
                }
            }

            levelGameObject = new GameObject("LevelManager");
            s_LevelManager = levelGameObject.AddComponent<LevelManager>();
            s_LevelManager.LevelDefinition = levelDefinition;
            SetLevelColors(levelDefinition.NumberOfColors, levelDefinition);
            Transform levelParent = levelGameObject.transform;

            var targetColor = Color.white;
            if (Application.isPlaying)
            {
                levelDefinition.Spawnables.Reverse();
            }

            for (int i = 0; i < levelDefinition.Spawnables.Length; i++)
            {
                LevelDefinition.SpawnableObject spawnableObject = levelDefinition.Spawnables[i];

                if (spawnableObject.SpawnablePrefab == null)
                {
                    continue;
                }

                Vector3 position = spawnableObject.Position;
                Vector3 eulerAngles = spawnableObject.EulerAngles;
                Vector3 scale = spawnableObject.Scale;

                GameObject go = null;

                if (Application.isPlaying)
                {
                    go = GameObject.Instantiate(spawnableObject.SpawnablePrefab, position, Quaternion.Euler(eulerAngles));
                }
                else
                {
#if UNITY_EDITOR
                    go = (GameObject)PrefabUtility.InstantiatePrefab(spawnableObject.SpawnablePrefab);
                    go.transform.position = position;
                    go.transform.eulerAngles = eulerAngles;
#endif
                }

                if (go == null)
                {
                    return;
                }

                // Set Base Color
                Spawnable spawnable = go.GetComponent<Spawnable>();
                if (spawnable != null)
                {
                    if (spawnable is Block block)
                    {
                        block.BuildGates(levelDefinition);
                        var blockData = GenerateBlockData(levelDefinition);

                        for (int j = 0; j < blockData.GateColors.Count; j++)
                        {
                            block.Gates[j].SetBaseColor(blockData.GateColors[j]);
                            block.Gates[j].MixValue = blockData.MixValue;

                            var pos = block.Gates[j].transform.position;
                            var offset = blockData.PositionOffsets[j];
                            block.Gates[j].transform.position = new Vector3(pos.x + offset.x, pos.y + offset.y, pos.z + offset.z);
                        }

                        targetColor = Color.Lerp(blockData.CorrectColor, targetColor, blockData.MixValue);
                    }
                    else
                    {
                        spawnable.SetBaseColor(spawnableObject.BaseColor);
                    }

                    spawnable.SetScale(scale);
                    s_LevelManager.AddSpawnable(spawnable);
                }

                if (go != null)
                {
                    go.transform.SetParent(levelParent);
                }
            }

            var target = s_LevelManager.ActiveSpawnables.FirstOrDefault(s => s is TargetBase);
            if (target != null)
            {
                target.SetBaseColor(targetColor);
            }
        }

        public void UnloadCurrentLevel()
        {
            if (m_CurrentLevelGO != null)
            {
                GameObject.Destroy(m_CurrentLevelGO);
            }

            if (m_LevelMarkersGO != null)
            {
                GameObject.Destroy(m_LevelMarkersGO);
            }

            if (m_CurrentTerrainGO != null)
            {
                GameObject.Destroy(m_CurrentTerrainGO);
            }

            m_CurrentLevel = null;
        }

        private void StartGame(PlayerSpeedPreset playerSpeed)
        {
            ResetLevel(playerSpeed);
        }

        /// <summary>
        /// Creates and instantiates the StartPrefab and EndPrefab defined inside
        /// the levelDefinition.
        /// </summary>
        /// <param name="levelDefinition">
        /// A LevelDefinition ScriptableObject that defines the start and end prefabs.
        /// </param>
        /// <param name="levelMarkersGameObject">
        /// A new GameObject that is created to be the parent of the start and end prefabs.
        /// </param>
        public static void PlaceLevelMarkers(LevelDefinition levelDefinition, ref GameObject levelMarkersGameObject)
        {
            if (levelMarkersGameObject != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(levelMarkersGameObject);
                }
                else
                {
                    DestroyImmediate(levelMarkersGameObject);
                }
            }

            levelMarkersGameObject = new GameObject("Level Markers");

            GameObject start = levelDefinition.StartPrefab;
            GameObject end = levelDefinition.EndPrefab;

            if (start != null)
            {
                GameObject go = GameObject.Instantiate(start, new Vector3(start.transform.position.x, start.transform.position.y, 0.0f), Quaternion.identity);
                go.transform.SetParent(levelMarkersGameObject.transform);
            }

            if (end != null)
            {
                GameObject go = GameObject.Instantiate(end, new Vector3(end.transform.position.x, end.transform.position.y, levelDefinition.LevelLength), Quaternion.identity);
                go.transform.SetParent(levelMarkersGameObject.transform);

                var finishLine = go.GetComponent<FinishLine>();
                var target = s_LevelManager.ActiveSpawnables.FirstOrDefault(s => s is TargetBase);
                if (target != null)
                {
                    target.transform.SetPositionAndRotation(finishLine.TargetPosition.position, finishLine.TargetPosition.rotation);
                }
            }
        }

        /// <summary>
        /// Creates and instantiates a Terrain GameObject, built
        /// to the specifications saved in levelDefinition.
        /// </summary>
        /// <param name="levelDefinition">
        /// A LevelDefinition ScriptableObject that defines the terrain size.
        /// </param>
        /// <param name="terrainGameObject">
        /// A new GameObject that is created to hold the terrain.
        /// </param>
        public static void CreateTerrain(LevelDefinition levelDefinition, ref GameObject terrainGameObject)
        {
            TerrainGenerator.TerrainDimensions terrainDimensions = new TerrainGenerator.TerrainDimensions()
            {
                Width = levelDefinition.LevelWidth,
                Length = levelDefinition.LevelLength,
                StartBuffer = levelDefinition.LevelLengthBufferStart,
                EndBuffer = levelDefinition.LevelLengthBufferEnd,
                Thickness = levelDefinition.LevelThickness
            };
            TerrainGenerator.CreateTerrain(terrainDimensions, levelDefinition.TerrainMaterial, ref terrainGameObject);
        }

        public void Win()
        {
            m_WinEvent.Raise();

#if UNITY_EDITOR
            //if (m_LevelEditorMode)
            //{
            //    ResetLevel();
            //}
#endif
        }

        public void Lose()
        {
            m_LoseEvent.Raise();

#if UNITY_EDITOR
            //if (m_LevelEditorMode)
            //{
            //    ResetLevel();
            //}
#endif
        }

        private static void SetLevelColors(int numberOfColors, LevelDefinition levelDefinition)
        {
            s_LevelColors.Clear();
            var colors = levelDefinition.LevelColors.ToList();
            for (int i = 0; i < numberOfColors; i++)
            {
                RandomAddWithOutRepetition(s_LevelColors, colors);
            }
        }

        private static BlockData GenerateBlockData(LevelDefinition levelDefinition)
        {
            var gateData = new BlockData();
            var colors = s_LevelColors.ToList();

            for (int i = 0; i < levelDefinition.NumberOfGates; i++)
            {
                if (levelDefinition.IsRandomOrder)
                {
                    RandomAddWithOutRepetition(gateData.GateColors, colors);
                }
                else
                {
                    gateData.GateColors.Add(colors[i]);
                }
                gateData.PositionOffsets.Add(new Vector3(Random.Range(-levelDefinition.Offset.x, levelDefinition.Offset.x), Random.Range(-levelDefinition.Offset.y, levelDefinition.Offset.y), Random.Range(-levelDefinition.Offset.z, levelDefinition.Offset.z)));
            }

            gateData.CorrectColor = gateData.GateColors[Random.Range(0, gateData.GateColors.Count)];
            return gateData;
        }

        private static void RandomAddWithOutRepetition(List<Color> listToAdd, List<Color> listToAddFrom)
        {
            var index = Random.Range(0, listToAddFrom.Count);
            listToAdd.Add(listToAddFrom[index]);
            listToAddFrom.RemoveAt(index);
        }
    }
}
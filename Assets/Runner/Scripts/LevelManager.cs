using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A class used to hold a reference to the current
    /// level and provide access to other classes.
    /// </summary>
    [ExecuteInEditMode]
    public class LevelManager : MonoBehaviour
    {
        /// <summary>
        /// Returns the LevelManager.
        /// </summary>
        public static LevelManager Instance => s_Instance;

        private static LevelManager s_Instance;

        /// <summary>
        /// Returns the LevelDefinition used to create this LevelManager.
        /// </summary>
        public LevelDefinition LevelDefinition
        {
            get => m_LevelDefinition;
            set
            {
                m_LevelDefinition = value;

                if (m_LevelDefinition != null && PlayerController.Instance != null)
                {
                    PlayerController.Instance.SetCustomSpeed(m_LevelDefinition.PlayerSpeed);
                    PlayerController.Instance.SetMaxXPosition(m_LevelDefinition.LevelWidth);
                    PlayerController.Instance.SetupPlayer();
                }
            }
        }

        private LevelDefinition m_LevelDefinition;

        private List<Spawnable> m_ActiveSpawnables = new List<Spawnable>();
        public List<Spawnable> ActiveSpawnables => m_ActiveSpawnables;

        /// <summary>
        /// Call this method to add a Spawnable to the list of active Spawnables.
        /// </summary>
        public void AddSpawnable(Spawnable spawnable)
        {
            m_ActiveSpawnables.Add(spawnable);
        }

        /// <summary>
        /// Calling this method calls the Reset() method on all Spawnables in this level.
        /// </summary>
        public void ResetSpawnables()
        {
            for (int i = 0, c = m_ActiveSpawnables.Count; i < c; i++)
            {
                m_ActiveSpawnables[i].ResetData();
            }
        }

        private void Awake()
        {
            SetupInstance();
        }

        private void OnEnable()
        {
            SetupInstance();
        }

        private void SetupInstance()
        {
            if (s_Instance != null && s_Instance != this)
            {
                if (Application.isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject);
                }
                return;
            }

            s_Instance = this;
        }
    }
}
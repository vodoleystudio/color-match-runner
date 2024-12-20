using System;
using System.Collections.Generic;
using DG.Tweening;
using HyperCasual.Core;
using UnityEngine;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A scriptable object that stores all information
    /// needed to load and set up a Runner level.
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "Runner/LevelDefinition", order = 1)]
    public class LevelDefinition : AbstractLevelData
    {
        /// <summary>
        /// The Length of the level.
        /// </summary>
        public float LevelLength = 100.0f;

        /// <summary>
        /// The amount of extra terrain to be added before the start of the level.
        /// </summary>
        public float LevelLengthBufferStart = 5.0f;

        /// <summary>
        /// The amount of extra terrain to be added after the end of the level.
        /// </summary>
        public float LevelLengthBufferEnd = 5.0f;

        /// <summary>
        /// The width of the level.
        /// </summary>
        public float LevelWidth = 5.0f;

        /// <summary>
        /// The thickness of the level.
        /// </summary>
        public float LevelThickness = 0.1f;

        /// <summary>
        /// True means that spawnables should snap to a grid in this level.
        /// </summary>
        public bool SnapToGrid = true;

        /// <summary>
        /// The size of the grid that spawnables will snap to if SnapToGrid
        /// is true.
        /// </summary>
        public float GridSize = 1.0f;

        /// <summary>
        /// The material applied to the generated terrain for this level.
        /// </summary>
        public Material TerrainMaterial;

        /// <summary>
        /// A prefab placed at the start point of this level.
        /// </summary>
        public GameObject StartPrefab;

        /// <summary>
        /// A prefab placed at the end of this level. This prefab should
        /// contain collision logic to complete the level.
        /// </summary>
        public GameObject EndPrefab;

        public Vector3 Offset = Vector3.zero;

        public List<Color> LevelColors = new();

        public int NumberOfColors = 4;

        public bool IsRandomOrder = false;

        public int NumberOfGates = 4;

        public Vector3 OffsetBetweenTheGates = new(3f, 0f, 0f);

        public Vector3 AddToEachGateStartPosition = new(0f, 0.5f, 0f);

        public Vector3 StartGateRotation = new(0f, 90f, 0f);

        public float PlayerSpeed = 100f;

        public Movment GatesMovment;

        [Serializable]
        public class Movment
        {
            public float Duration = 0.0f;

            public float WaitTime = 0.0f;

            public int NumberOfGatesMoving = 0;

            public bool IsTheGatesCentrade = true;

            public bool IsTheMovingGatesAreInRandomOrder = false;

            public Vector2 MaxAndMinStartTimeRange = Vector2.zero;

            [Serializable]
            public class MovmentDirections
            {
                public Vector3 MovmentOffset = Vector3.zero;

                public Ease Ease = Ease.Linear;
            }

            public List<MovmentDirections> Directions = new();

            public int ProbabilityToChabgeDirectionInProcent = 50;
        }

        /// <summary>
        /// An array of all SpawnableObjects that exist in this level.
        /// </summary>
        public SpawnableObject[] Spawnables;

        [System.Serializable]
        public class SpawnableObject
        {
            /// <summary>
            /// The prefab spawned by this SpawnableObject.
            /// </summary>
            public GameObject SpawnablePrefab;

            /// <summary>
            /// The world position of this SpawnableObject.
            /// </summary>
            public Vector3 Position = Vector3.zero;

            /// <summary>
            /// The rotational euler angles of this SpawnableObject.
            /// </summary>
            public Vector3 EulerAngles = Vector3.zero;

            /// <summary>
            /// The world scale of this SpawnableObject.
            /// </summary>
            public Vector3 Scale = Vector3.one;

            /// <summary>
            /// The base color to be applied to the materials on
            /// this SpawnableObject.
            /// </summary>
            public Color BaseColor = Color.white;

            /// <summary>
            /// True if this object should snap to a levels grid.
            /// Setting this value to false will make this SpawnableObject
            /// ignore the level's snap settings.
            /// </summary>
            public bool SnapToGrid = true;
        }

        /// <summary>
        /// Store all values from updatedLevel into this LevelDefinition.
        /// </summary>
        /// <param name="updatedLevel">
        /// The LevelDefinition that has been edited in the Runner Level Editor Window.
        /// </param>
        public void SaveValues(LevelDefinition updatedLevel)
        {
            // TODO - Add Tests for this!
            LevelLength = updatedLevel.LevelLength;
            LevelLengthBufferStart = updatedLevel.LevelLengthBufferStart;
            LevelLengthBufferEnd = updatedLevel.LevelLengthBufferEnd;
            LevelWidth = updatedLevel.LevelWidth;
            LevelThickness = updatedLevel.LevelThickness;
            SnapToGrid = updatedLevel.SnapToGrid;
            GridSize = updatedLevel.GridSize;
            TerrainMaterial = updatedLevel.TerrainMaterial;
            StartPrefab = updatedLevel.StartPrefab;
            EndPrefab = updatedLevel.EndPrefab;
            Offset = updatedLevel.Offset;
            LevelColors = updatedLevel.LevelColors;
            NumberOfColors = updatedLevel.NumberOfColors;
            IsRandomOrder = updatedLevel.IsRandomOrder;
            Spawnables = updatedLevel.Spawnables;
            NumberOfColors = updatedLevel.NumberOfColors;
            OffsetBetweenTheGates = updatedLevel.OffsetBetweenTheGates;
            StartGateRotation = updatedLevel.StartGateRotation;
            PlayerSpeed = updatedLevel.PlayerSpeed;
            GatesMovment = updatedLevel.GatesMovment;
            AddToEachGateStartPosition = updatedLevel.AddToEachGateStartPosition;
        }
    }
}
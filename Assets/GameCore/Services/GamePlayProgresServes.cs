using UnityEngine;
using GameCore.Data;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;

namespace GameCore.Services
{
    public class GamePlayProgressService : IGamePlayProgressService
    {
        private const float MinColorUnitResolution = 1 / 256f; // 0.00390625
        private const float MaxOffset = 127 * MinColorUnitResolution;
        private const float OffsetStep = 3f * MinColorUnitResolution;
        private const int m_NumberOfColors = 4;

        private List<Color> m_AllColors = new() { Color.blue, Color.green, Color.cyan, Color.gray, Color.red, Color.magenta, Color.yellow };
        private List<Color> m_LevelColors = new();
        private float _offset;

        public void Setup()
        {
            m_LevelColors.Clear();
            var colors = m_AllColors.ToList();
            int index;
            for (int i = 0; i < m_NumberOfColors; i++)
            {
                index = Random.Range(0, colors.Count);
                m_LevelColors.Add(colors[index]);
                colors.RemoveAt(index);
            }
        }

        public BlockData GenerateBlockData()
        {
            var gateData = new BlockData();

            foreach (var color in m_LevelColors)
            {
                gateData.GateColors.Add(color);
            }

            gateData.CorrectColor = gateData.GateColors[Random.Range(0, gateData.GateColors.Count)];

            Update();

            return gateData;
        }

        private void Update()
        {
            _offset -= OffsetStep;
            _offset = Mathf.Clamp(_offset, OffsetStep, MaxOffset);
        }
    }
}
using UnityEngine;
using GameCore.Data;
using System.Collections.Generic;
using System.Linq;

namespace GameCore.Services
{
    public class GamePlayProgressService : IGamePlayProgressService
    {
        protected int m_NumberOfColors = 4;
        private List<Color> m_AllColors = new() { Color.blue, Color.green, Color.cyan, Color.gray, Color.red, Color.magenta, Color.yellow };
        private List<Color> m_LevelColors = new();

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
                gateData.PositionOffsets.Add(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-5f, 5f)));
            }

            gateData.CorrectColor = gateData.GateColors[Random.Range(0, gateData.GateColors.Count)];
            return gateData;
        }
    }
}
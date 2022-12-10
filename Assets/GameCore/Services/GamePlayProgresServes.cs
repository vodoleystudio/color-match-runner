using UnityEngine;
using GameCore.Data; 

namespace GameCore.Services
{
    public class GamePlayProgressService : IGamePlayProgressService
    {
        private IRandomColorService _randomColorService = new RandomColorService();

        private const float MinColorUnitResolution = 1/256f; // 0.00390625
        private const float Treshold = 127 * MinColorUnitResolution;

        private float _treshold;

        public void Reset()
        {
            _treshold = Treshold;
        }

        public BlockData GenerateBlockData()
        {
            var gateData = new BlockData();
            var randomColor = _randomColorService.GetRandomColor();
            gateData.GateColors.Add(randomColor);
            gateData.GateColors.Add(_randomColorService.GetRandomSimilarColor(randomColor, 0f, _treshold));
            gateData.CorrectColor = gateData.GateColors[Random.Range(0, gateData.GateColors.Count)];

            UpdateTresholds();
            
            return gateData;
        }

        private void UpdateTresholds()
        {
            _treshold -= MinColorUnitResolution;
        }
    }
}


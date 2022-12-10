using UnityEngine;
using GameCore.Data; 

namespace GameCore.Services
{
    public class GamePlayProgressService : IGamePlayProgressService
    {
        private IRandomColorService _randomColorService = new RandomColorService();

        private const float MinColorUnitResolution = 1/256f; // 0.00390625
        private const float MaxOffset = 127 * MinColorUnitResolution;
        private const float OffesetStep = 10f; 
        private float _offest;

        public void Reset()
        {
            _offest = MaxOffset;
        }

        public BlockData GenerateBlockData()
        {
            var gateData = new BlockData();
            var randomColor = _randomColorService.GetRandomColor();
            gateData.GateColors.Add(randomColor);
            gateData.GateColors.Add(_randomColorService.GetSimilarColor(randomColor ,_offest));
            gateData.CorrectColor = gateData.GateColors[Random.Range(0, gateData.GateColors.Count)];

            UpdateTresholds();
            
            return gateData;
        }

        private void UpdateTresholds()
        {
            _offest -= MinColorUnitResolution * OffesetStep;
        }
    }
}


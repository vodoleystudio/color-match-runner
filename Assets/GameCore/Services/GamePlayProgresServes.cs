using UnityEngine;
using GameCore.Data; 

namespace GameCore.Services
{
    public class GamePlayProgressService : IGamePlayProgressService
    {
        private IRandomColorService _randomColorService = new RandomColorService();

        private const float MinColorUnitResolution = 1/256f; // 0.00390625
        private const float MaxOffset = 127 * MinColorUnitResolution;
        private const float OffsetStep = 3f * MinColorUnitResolution;

        private float _offset;

        public void Reset()
        {
            _offset = MaxOffset;
        }

        public BlockData GenerateBlockData()
        {
            var gateData = new BlockData();
            var randomColor = _randomColorService.GetRandomColor();
            gateData.GateColors.Add(randomColor);
            gateData.GateColors.Add(_randomColorService.GetSimilarColor(randomColor, _offset));
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


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
        private static Color EmptyColor = new Color(0f, 0f, 0f, 0f);

        private float _offset;

        private Color _color = EmptyColor;

        public void Reset()
        {
            _offset = MaxOffset;
            _color = EmptyColor;
        }

        public BlockData GenerateBlockData()
        {
            var gateData = new BlockData();
            if (_color == EmptyColor)
            {
                _color = _randomColorService.GetRandomColor();
            }

            var randomColor = _color;
            //gateData.GateColors.Add(randomColor);
            //gateData.GateColors.Add(_randomColorService.GetSimilarRandomColor(randomColor, ColorComponent.R, 0f, 1f));
            gateData.GateColors.Add(Color.green);
            gateData.GateColors.Add(Color.red);
            gateData.GateColors.Add(Color.yellow);
            gateData.GateColors.Add(Color.blue);
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


using UnityEngine;
using GameCore.Data; 

namespace GameCore.Services
{
    public class GamePlayProgressService : IGamePlayProgressService
    {
        private IRandomColorService _randomColorService = new RandomColorService();

        public BlockData GenerateBlockData(float maxTrheshold, float minThreshold)
        {
            var gateData = new BlockData();
            var randomColor = _randomColorService.GetRandomColor();
            gateData.GateColors.Add(randomColor);
            gateData.GateColors.Add(_randomColorService.GetSimilarColor(randomColor, maxTrheshold , minThreshold));

            gateData.CorrectColor = gateData.GateColors[Random.Range(0, gateData.GateColors.Count)];
            
            return gateData;
        }
    }
}


using System.Collections.Generic;
using UnityEngine;
using GameCore.Data; 

namespace GameCore.Services
{
    public class GamePlayProgresServes : IGamePlayProgresService
    {
        private IRandomColorService _randomCOlorService = new RandomColorService();

        public GateData GetListOfSimilarsColors(float diffrenceTrheshold)
        {
            var gateData = new GateData();
            gateData.GateColors.Add(_randomCOlorService.GetRandomColor());
            gateData.GateColors.Add(_randomCOlorService.GetSimilarColor(gateData.GateColors[0] , diffrenceTrheshold));

            gateData.corectColor = gateData.GateColors[Random.Range(0, gateData.GateColors.Count)];
            
            return gateData;    
        }
    }
}


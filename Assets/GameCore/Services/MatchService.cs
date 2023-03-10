using UnityEngine;
using System;
using GameCore.Data;
using System.Collections.Generic;

namespace GameCore.Services
{
    public class MatchService : IMatchService
    {
        private const float k_MatchBarier = 0.06f;
        private const float k_PartialMatchBarier = 0.15f;
        private const float k_NotMatchBarier = 0.85f;

        public MatchData MatchColors(Color firstColor, Color secondColor)
        {
            var matchData = new MatchData();

            List<float> colorComponents = new List<float>();

            colorComponents.Add(Math.Abs(firstColor.r - secondColor.r));
            colorComponents.Add(Math.Abs(firstColor.g - secondColor.g));
            colorComponents.Add(Math.Abs(firstColor.b - secondColor.b));

            var avarge = (colorComponents[0] + colorComponents[1] + colorComponents[2]) / colorComponents.Count;

            if (avarge >= 0.0f && avarge < k_MatchBarier)
            {
                matchData.MatchState = MatchState.Match;
                Debug.LogError("Match State:" + matchData.MatchState);
            }
            else if (avarge >= k_MatchBarier && avarge < k_PartialMatchBarier)
            {
                matchData.MatchState = MatchState.PartialMatch;
                Debug.LogError("Match State:" + matchData.MatchState);
            }
            else if (avarge >= k_PartialMatchBarier && avarge <= k_NotMatchBarier)
            {
                matchData.MatchState = MatchState.NotMatch;
                Debug.LogError("Match State:" + matchData.MatchState);
            }
            else
            {
                Debug.Log("The color value is out of range");
            }

            matchData.MatchInPercentage = (int)Math.Round((1.0f - avarge) * 100);
            Debug.LogError("Match in procent:" + (int)Math.Round((1.0f - avarge) * 100));

            return matchData;
        }
    }
}
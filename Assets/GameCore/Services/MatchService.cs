using UnityEngine;
using System;
using GameCore.Data;
using System.Collections.Generic;

namespace GameCore.Services
{
    public class MatchService : IMatchService
    {
        private const int k_MatchBarier = 95;
        private const int k_PartialMatchBarier = 90;
        private const int k_PartMatchBarier = 85;

        public MatchData MatchColors(Color firstColor, Color secondColor)
        {
            var matchData = new MatchData();

            List<float> colorComponents = new List<float>();

            colorComponents.Add(Math.Abs(firstColor.r - secondColor.r));
            colorComponents.Add(Math.Abs(firstColor.g - secondColor.g));
            colorComponents.Add(Math.Abs(firstColor.b - secondColor.b));

            var matchInProcent = (int)Math.Round((1f - (colorComponents[0] + colorComponents[1] + colorComponents[2]) / colorComponents.Count) * 100);

            if (100 >= matchInProcent && matchInProcent >= k_MatchBarier)
            {
                matchData.MatchState = MatchState.Match;
            }
            else if (k_MatchBarier > matchInProcent && matchInProcent >= k_PartialMatchBarier)
            {
                matchData.MatchState = MatchState.PartialMatch;
            }
            else if (k_PartialMatchBarier > matchInProcent && matchInProcent >= k_PartMatchBarier)
            {
                matchData.MatchState = MatchState.PartMatch;
            }
            else if (k_PartialMatchBarier > matchInProcent && matchInProcent >= 0)
            {
                matchData.MatchState = MatchState.NotMatch;
            }
            else
            {
                Debug.Log("The color value is out of range");
            }

            matchData.MatchInPercentage = matchInProcent;
            Debug.LogError($"MatchState is {matchData.MatchState}, Match in procent: {matchInProcent}");
            return matchData;
        }
    }
}
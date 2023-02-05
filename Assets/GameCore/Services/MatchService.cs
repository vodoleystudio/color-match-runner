using UnityEngine;
using System;
using GameCore.Data;
using System.Collections.Generic;

namespace GameCore.Services
{
    public class MatchService
    {
        private const float k_MatchBarier = 0.33f;
        private const float k_PartialMatch = 0.67f;
        private const float k_NotMatchBarier = 1.0f;

        public MatchData MatchColors(Color firstColor, Color secondColor)
        {
            MatchData matchData = new MatchData();

            List<float> colorComponents = new List<float>();

            colorComponents.Add(Math.Abs(firstColor.r - secondColor.b));
            colorComponents.Add(Math.Abs(firstColor.g - secondColor.g));
            colorComponents.Add(Math.Abs(firstColor.g - secondColor.b));

            var Avarge = (colorComponents[0] + colorComponents[1] + colorComponents[2]) / colorComponents.Count;

            if (Avarge >= 0.0f && Avarge < k_MatchBarier)
            {
                matchData.m_MatchState = MatchState.Match;
            }
            else if (Avarge >= k_MatchBarier && Avarge < k_PartialMatch)
            {
                matchData.m_MatchState = MatchState.PartialMatch;
            }
            else if (Avarge >= k_PartialMatch && Avarge < k_NotMatchBarier)
            {
                matchData.m_MatchState = MatchState.Match;
            }
            else
            {
                Debug.Log("The color value is out of range");
            }

            matchData.m_MatchInProsent = (int)Math.Round((1.0f - Avarge) * 10);

            return matchData;
        }
    }
}
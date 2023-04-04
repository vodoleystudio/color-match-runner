using GameCore.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Services
{
    public class MatchService : IMatchService
    {
        private const int k_HeartBarier = 100;
        private const int k_LikeBarier = 95;
        private const int k_BrokenHeart = 85;
        private const int k_DisLikeBarier = 75;

        public MatchData MatchColors(Color firstColor, Color secondColor)
        {
            var matchData = new MatchData();

            List<float> colorComponents = new List<float>();

            colorComponents.Add(Math.Abs(firstColor.r - secondColor.r));
            colorComponents.Add(Math.Abs(firstColor.g - secondColor.g));
            colorComponents.Add(Math.Abs(firstColor.b - secondColor.b));

            var matchInPercent = (int)Math.Round((1f - (colorComponents[0] + colorComponents[1] + colorComponents[2]) / colorComponents.Count) * 100);

            if (k_HeartBarier >= matchInPercent && matchInPercent >= k_LikeBarier)
            {
                matchData.MatchState = MatchState.Heart;
            }
            else if (k_LikeBarier > matchInPercent && matchInPercent >= k_BrokenHeart)
            {
                matchData.MatchState = MatchState.Like;
            }
            else if (k_BrokenHeart > matchInPercent && matchInPercent >= k_DisLikeBarier)
            {
                matchData.MatchState = MatchState.BrokenHeart;
            }
            else if (k_BrokenHeart > matchInPercent && matchInPercent >= 0)
            {
                matchData.MatchState = MatchState.DisLike;
            }
            else
            {
                Debug.Log("The color value is out of range");
            }

            matchData.MatchInPercentage = matchInPercent;
            Debug.LogError($"MatchState is {matchData.MatchState}, Match in procent: {matchInPercent}");
            return matchData;
        }
    }
}
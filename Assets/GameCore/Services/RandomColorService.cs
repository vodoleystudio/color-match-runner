using GameCore.Data;
using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
   
namespace GameCore.Services
{
    public class RandomColorService : IRandomColorService
    {
        private const float MinColorValue = 0f;
        private const float MaxColorValue = 1f;

        public Color GetRandomColor()
        {
            return new Color(Random.Range(MinColorValue, MaxColorValue), Random.Range(MinColorValue, MaxColorValue), Random.Range(MinColorValue, MaxColorValue));
        }

        public Color GetSimilarColor(Color color, float TheDifrentBtwThwColors)
        {
            var values = Enum.GetValues(typeof(ColorComponents)).Cast<ColorComponents>().ToList();
            values.Remove(ColorComponents.None);
            var colorComponent = values[Random.Range(0, values.Count)];

            switch (colorComponent)
            {
                case ColorComponents.R:
                    return new Color(Random.Range((1f - TheDifrentBtwThwColors) / 2f, ((1f - TheDifrentBtwThwColors) / 2f) + TheDifrentBtwThwColors), color.g, color.b);

                case ColorComponents.G:
                    return new Color(color.r, Random.Range((1f - TheDifrentBtwThwColors) / 2f, ((1f - TheDifrentBtwThwColors) / 2f) + TheDifrentBtwThwColors), color.b);

                case ColorComponents.B:
                    return new Color(color.r, color.g, Random.Range((1f - TheDifrentBtwThwColors) / 2f, ((1f - TheDifrentBtwThwColors) / 2f) + TheDifrentBtwThwColors));

                default:
                    throw new Exception();
            }
        }
    }
}

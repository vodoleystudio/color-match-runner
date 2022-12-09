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

        public Color GetSimilarColor(Color color, float maxThreshold)
        {
            var values = Enum.GetValues(typeof(ColorComponents)).Cast<ColorComponents>().ToList();
            values.Remove(ColorComponents.None);
            var colorComponent = values[Random.Range(0, values.Count)];

            switch (colorComponent)
            {
                case ColorComponents.R:
                    return new Color(GetChangedComponentValue(maxThreshold, color.r), color.g, color.b); 

                case ColorComponents.G:
                    return new Color(color.r, GetChangedComponentValue(maxThreshold, color.g), color.b);

                case ColorComponents.B:
                    return new Color(color.r, color.g, GetChangedComponentValue(maxThreshold, color.b));

                default:
                    throw new Exception();
            }
        }

        private float GetChangedComponentValue(float maxDiffrenceThreshold , float colorComponent)
        {
            var halfRange = maxDiffrenceThreshold / 2f;

            if ((colorComponent + halfRange <= MaxColorValue) && (colorComponent - halfRange >= MinColorValue))
            {
                return Random.Range(colorComponent - halfRange, colorComponent + halfRange);
            }
            else if (colorComponent - halfRange < MinColorValue)
            {
                return Random.Range(MinColorValue, colorComponent + halfRange);
            }
            else if ((colorComponent + halfRange > MaxColorValue))
            {
                return Random.Range(colorComponent - halfRange, MaxColorValue);
            }
            else
            {
                throw new Exception();
            }
        }
    }
}

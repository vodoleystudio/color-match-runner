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

        public Color GetRandomSimilarColor(Color color, float minThreshold, float maxThreshold)
        {
            var values = Enum.GetValues(typeof(ColorComponents)).Cast<ColorComponents>().ToList();
            values.Remove(ColorComponents.None);
            var colorComponent = values[Random.Range(0, values.Count)];

            return colorComponent switch
            {
                ColorComponents.R => new Color(GetChangedComponentValue(color.r, minThreshold, maxThreshold), color.g, color.b),
                ColorComponents.G => new Color(color.r, GetChangedComponentValue(color.g, minThreshold, maxThreshold), color.b),
                ColorComponents.B => new Color(color.r, color.g, GetChangedComponentValue(color.b, minThreshold, maxThreshold)),
                _ => throw new Exception(),
            };
        }

        private float GetChangedComponentValue(float colorComponent, float minThreshold, float maxThreshold)
        {
            maxThreshold = Math.Clamp(maxThreshold, MinColorValue, MaxColorValue); // TODO : should be in some way near minThreshold = Math.Clamp

            var halfRange = maxThreshold / 2f; // TODO : do half range to minThreshold as well or do for both full value
            var rnd = Random.Range(0, 2); // TODO : create utils function that returns true/false randomly and change 'rnd' to be clear boolean name

            minThreshold = Math.Clamp(minThreshold, MinColorValue, halfRange); // TODO : should be in some way near maxThreshold = Math.Clamp

            if (minThreshold > halfRange)
            {
                throw new Exception("maxThreshold should be bigger twice than minThreshold");
            }

            // TODO : check for duplicated code
            if ((colorComponent + halfRange <= MaxColorValue) && (colorComponent - halfRange >= MinColorValue))
            {               
                if (rnd == 1)
                {
                    return Random.Range(colorComponent - halfRange, colorComponent - minThreshold);
                }
                else
                {
                    return Random.Range(colorComponent + minThreshold, colorComponent + halfRange);
                }
            }
            else if (colorComponent - halfRange < MinColorValue)
            {
                if (colorComponent - minThreshold < MinColorValue)
                {
                    return Random.Range(colorComponent + minThreshold, colorComponent + halfRange);
                }
                else if (rnd == 1)
                {
                    return  Random.Range(MinColorValue, colorComponent - minThreshold);
                }
                else
                {
                    return Random.Range(colorComponent + minThreshold, colorComponent + halfRange);
                }
            }
            else if (colorComponent + halfRange > MaxColorValue)
            {
                if (colorComponent + minThreshold > MaxColorValue)
                {
                    return Random.Range(colorComponent - halfRange, MaxColorValue);
                }
                else if (rnd == 1)
                {
                    return Random.Range(colorComponent + minThreshold, MaxColorValue);
                }
                else
                {
                    return Random.Range(colorComponent - halfRange, colorComponent - minThreshold);
                }  
            }
            else
            {
                throw new Exception("suitable component value not found");
            }
        }
    }
}

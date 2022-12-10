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
        private ColorComponents GetRandomComponent()
        {
            var values = Enum.GetValues(typeof(ColorComponents)).Cast<ColorComponents>().ToList();
            values.Remove(ColorComponents.None);
            var colorComponent = values[Random.Range(0, values.Count)];
            return colorComponent;
        }

        public Color GetSimilarColor(Color color, float minThreshold, float maxThreshold)
        {
            var colorComponent = GetRandomComponent();
            return colorComponent switch
            {
                ColorComponents.R => new Color(GetChangedComponentValueBasedOnRandom(color.r, minThreshold, maxThreshold), color.g, color.b),
                ColorComponents.G => new Color(color.r, GetChangedComponentValueBasedOnRandom(color.g, minThreshold, maxThreshold), color.b),
                ColorComponents.B => new Color(color.r, color.g, GetChangedComponentValueBasedOnRandom(color.b, minThreshold, maxThreshold)),
                _ => throw new Exception(),
            };
        }

        public Color GetSimilarColor(Color color, float offset)
        {
            var colorComponent = GetRandomComponent();
            return colorComponent switch
            {
                ColorComponents.R => new Color(GetChangedColorComponentValueBasedOnOffset (color.r , offset), color.g, color.b),
                ColorComponents.G => new Color(color.r, GetChangedColorComponentValueBasedOnOffset(color.g, offset) , color.b),
                ColorComponents.B => new Color(color.r, color.g, GetChangedColorComponentValueBasedOnOffset(color.b, offset)),
                _ => throw new Exception(),
            };
        }

        private float GetChangedColorComponentValueBasedOnOffset(float colorComponentValue, float offset)
        {
            var colorPlusOffset = colorComponentValue + offset;
            var colorMinusOffset = colorComponentValue - offset;

            if ((colorMinusOffset >= MinColorValue) && (colorPlusOffset <= MaxColorValue))
            {
                var rightOrLeft = Utils.RnadomBolean();
                if (rightOrLeft)
                {
                    return colorMinusOffset;
                }
                else
                {
                    return colorPlusOffset;
                }
            }
            else if (colorMinusOffset < MinColorValue)
            {
                return colorPlusOffset;
            }
            else if (colorPlusOffset > MaxColorValue)
            {
                return colorMinusOffset;
            }
            else
            {
                throw new Exception("suitable component value not found");
            }
        }

        private float GetChangedComponentValueBasedOnRandom(float colorComponent, float minThreshold, float maxThreshold)
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

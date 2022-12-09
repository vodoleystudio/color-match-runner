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

        public Color GetSimilarColor(Color color, float maxThreshold, float minThershold)
        {
            var values = Enum.GetValues(typeof(ColorComponents)).Cast<ColorComponents>().ToList();
            values.Remove(ColorComponents.None);
            var colorComponent = values[Random.Range(0, values.Count)];

            switch (colorComponent)
            {
                case ColorComponents.R:
                    return new Color(GetChangedComponentValue(minThershold ,maxThreshold, color.r), color.g, color.b); 

                case ColorComponents.G:
                    return new Color(color.r, GetChangedComponentValue(minThershold,maxThreshold, color.g), color.b);

                case ColorComponents.B:
                    return new Color(color.r, color.g, GetChangedComponentValue(minThershold,maxThreshold, color.b));

                default:
                    throw new Exception();
            }
        }

        private float GetChangedComponentValue(float minThreshold ,float maxThreshold , float colorComponent)
        {
            maxThreshold = Math.Clamp(maxThreshold, MinColorValue, MaxColorValue);

            var halfRange = maxThreshold / 2f;
            var rnd = Random.Range(0, 2);

            minThreshold = Math.Clamp(minThreshold, MinColorValue, halfRange - 0.01f);

            if (minThreshold > halfRange)
            {
                throw new Exception("maxThreshold should be bigger twice than minThreshold");
            }

            if ((colorComponent + halfRange <= MaxColorValue) && (colorComponent - halfRange >= MinColorValue))
            {               
                if (rnd == 1)
                {
                    return Random.Range( colorComponent - halfRange, colorComponent - minThreshold);
                }
                else
                {
                    return Random.Range(colorComponent + minThreshold, colorComponent  + halfRange);
                }
            }
            else if (colorComponent - halfRange < MinColorValue)
            {
                if ((colorComponent - minThreshold < MinColorValue) )
                {
                    return Random.Range(colorComponent + minThreshold, colorComponent + halfRange);
                }
                else if (rnd == 1 )
                {
                    return  Random.Range(MinColorValue, colorComponent - minThreshold);
                }
                else
                {
                   return Random.Range(colorComponent + minThreshold, colorComponent + halfRange);
                }
            }
            else if ((colorComponent + halfRange > MaxColorValue))
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
                throw new Exception();
            }
        }
    }
}

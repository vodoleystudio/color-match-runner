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

        public Color GetSimilarColor(Color color, float diffrenceThreshold = MaxColorValue)
        {
            if (Mathf.Approximately(diffrenceThreshold, MinColorValue))
            {
                return new Color(color.r, color.g, color.b);
            }

            var values = Enum.GetValues(typeof(ColorComponents)).Cast<ColorComponents>().ToList();
            values.Remove(ColorComponents.None);
            var colorComponent = values[Random.Range(0, values.Count)];

            var changedComponentValue = GetChangedComponentValue(diffrenceThreshold);

            switch (colorComponent)
            {
                case ColorComponents.R:
                    return new Color(changedComponentValue, color.g, color.b);

                case ColorComponents.G:
                    return new Color(color.r, changedComponentValue, color.b);

                case ColorComponents.B:
                    return new Color(color.r, color.g, changedComponentValue);

                default:
                    throw new Exception();
            }
        }
        private float GetChangedComponentValue(float diffrenceThreshold)
        {
            return Random.Range((1f - diffrenceThreshold) / 2f, ((1f - diffrenceThreshold) / 2f) + diffrenceThreshold);
        }
    }
}

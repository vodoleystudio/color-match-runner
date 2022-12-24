using GameCore.Data;
using UnityEngine;

namespace GameCore.Services
{
    public interface IRandomColorService
    {
        Color GetRandomColor();
        Color GetSimilarRandomColor(Color color, float minThreshold, float maxThreshold);
        Color GetSimilarRandomColor(Color color, float offset);

        Color GetSimilarRandomColor(Color color, ColorComponent colorComponent, float offset);

        Color GetSimilarRandomColor(Color color, ColorComponent colorComponent, float minThreshold, float maxThreshold);
    }
}
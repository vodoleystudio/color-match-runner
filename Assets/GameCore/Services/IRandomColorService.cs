using UnityEngine;

namespace GameCore.Services
{
    public interface IRandomColorService
    {
        Color GetRandomColor();
        Color GetRandomSimilarColor(Color color, float minThreshold, float maxThreshold);
    }
}
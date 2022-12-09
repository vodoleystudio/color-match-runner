using UnityEngine;

namespace GameCore.Services
{
    public interface IRandomColorService
    {
        Color GetRandomColor();
        Color GetSimilarColor(Color color, float TheDifrentBtwThwColors);
    }
}
using UnityEngine;

public interface IRandomColorService
{
    Color GetRandomColor();
    Color GetSimilarColor(Color color);
}


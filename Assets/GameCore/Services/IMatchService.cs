using UnityEngine;
using GameCore.Data;

public interface IMatchService
{
    public MatchData MatchColors(Color firstColor, Color secondColor);
}
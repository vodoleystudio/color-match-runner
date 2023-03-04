using System;

namespace GameCore.Data
{
    [Serializable]
    public class LevelData
    {
        public string LevelId;

        public MatchData MatchData;

        public LevelData()
        {
        }

        public LevelData(string levelId, MatchData matchData)
        {
            LevelId = levelId;
            MatchData = matchData;
        }

        public override string ToString()
        {
            return base.ToString() + $" LevelId: {LevelId} ,MatchState: {MatchData?.MatchState} ,MatchInProcent: {MatchData?.MatchInPercentage}";
        }
    }
}
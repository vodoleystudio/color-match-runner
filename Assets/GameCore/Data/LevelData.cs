using JetBrains.Annotations;

namespace GameCore.Data
{
    public class LevelData
    {
        public LevelData(string name, int levelId, int numberOfColors, MatchData matchData)
        {
            m_TargetName = name;
            m_LevelId = levelId;
            m_NumberOfColors = numberOfColors;
            m_MatchState = matchData.m_MatchState;
            m_MatchInProcent = matchData.m_MatchInPercentage;
        }

        public string m_TargetName;
        public int m_LevelId;
        public MatchState m_MatchState;
        public int m_MatchInProcent;
        public int m_NumberOfColors;
    }
}
using Mono.Cecil.Cil;

namespace GameCore.Data
{
    public class LevelData
    {
        private string m_TargetName;
        public string TargetName => m_TargetName;

        private int m_LevelId;
        public int LevelId => m_LevelId;

        private MatchData m_MatchData;

        public LevelData(string name, int levelId, MatchData matchData)
        {
            m_TargetName = name;
            m_LevelId = levelId;
            m_MatchData = matchData;
        }

        public override string ToString()
        {
            return base.ToString() + $" ,Name: {m_TargetName} ,LevelId: {m_LevelId} ,MatchState: {m_MatchData.MatchState} ,MatchInProcent: {m_MatchData.MatchInPercentage}";
        }
    }
}
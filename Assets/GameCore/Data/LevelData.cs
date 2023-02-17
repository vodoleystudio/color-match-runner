namespace GameCore.Data
{
    public class LevelData
    {
        public LevelData(string name, int levelId, MatchState matchstate, int matchInProcent)
        {
            m_TargetName = name;
            m_LevelId = levelId;
            m_MatchState = matchstate;
            m_MatchInProcent = matchInProcent;
        }

        public string toString()
        {
            return $",LevelId :{m_LevelId}, TargetName: {m_TargetName}, MatchState:{m_MatchState},MatchInProcent:{m_MatchInProcent} ";
        }

        public string m_TargetName;
        public int m_LevelId;
        public MatchState m_MatchState;
        public int m_MatchInProcent;
    }
}
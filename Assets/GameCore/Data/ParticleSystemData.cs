using System;
using UnityEngine;

namespace GameCore.Data
{
    [Serializable]
    public class ParticleSystemData
    {
        [SerializeField]
        private MatchState m_MatchState;

        public MatchState MatchState => m_MatchState;

        [SerializeField]
        private ParticleSystem m_ParticleSystem;

        public ParticleSystem ParticleSystem => m_ParticleSystem;
    }
}
using UnityEngine;
using GameCore.Data;
using System.Collections.Generic;
using System.Linq;
using System;

namespace GameCore.Services
{
    [Serializable]
    public class PrticleSystemService : IPrticleSystemService
    {
        [SerializeField]
        private List<ParticleSystemData> m_ParticleSystem = new List<ParticleSystemData>();

        public void PlayParticleSystem(MatchState matchState)
        {
            m_ParticleSystem.FirstOrDefault(p => p.MatchState == matchState).ParticleSystem.Play();
        }
    }
}
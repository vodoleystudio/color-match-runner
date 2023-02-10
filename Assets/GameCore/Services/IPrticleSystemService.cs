using GameCore.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Services
{
    public interface IPrticleSystemService
    {
        public void PlayParticleSystem(MatchState matchState);
    }
}
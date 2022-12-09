
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Data;

namespace GameCore.Services
{
    public interface IGamePlayProgressService
    {
        BlockData GenerateBlockData(float diffrenceTrheshold);    
    }
}


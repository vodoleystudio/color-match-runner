using System.Collections;
using UnityEngine;
using GameCore.Data;

namespace GameCore.Services
{
    public interface IAnimationEntatyService
    {
        void Play(AnimationType animationType , Animator animator);
    }
}

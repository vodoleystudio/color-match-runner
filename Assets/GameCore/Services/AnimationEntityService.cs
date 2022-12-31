using UnityEngine;
using GameCore.Data;

namespace GameCore.Services
{
    public class AnimationEntityService :IAnimationEntityService 
    {
        private static readonly AnimationEntityService _instance = new AnimationEntityService();

        public static AnimationEntityService Instance => _instance;

        public void Play(AnimationType animationType , Animator animator)
        {
            var animationName = "animation";
            if (animationType != AnimationType.None)
            {
                animator.SetInteger(animationName, (int)animationType);
            }
            return;
        }

        private AnimationEntityService() { }
    }
}
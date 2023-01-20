using DG.Tweening;
using GameCore.Data;
using System;
using UnityEngine;

namespace GameCore.Services
{
    public class AnimationEntityService : IAnimationEntityService 
    {
        private static readonly AnimationEntityService _instance = new AnimationEntityService();

        public static AnimationEntityService Instance => _instance;

        public void Play(AnimationType animationType, Animator animator)
        {
            var animationName = "animation";
            if (animationType == AnimationType.None)
            {
                return;
            }
            animator.SetInteger(animationName, (int)animationType);
        }

        public void MoveTo(Animator animator ,AnimationType animationType ,Transform playerTransform ,Transform endPositionTransform ,float animationTime, Action onComplete = null)
        {
            Play(animationType, animator);
            playerTransform.DOMove(endPositionTransform.position, animationTime).OnComplete(() =>
            {
                Play(AnimationType.Idle, animator);
                onComplete?.Invoke();
            });
        }

        private AnimationEntityService() { }
    }
}

using UnityEngine;
using GameCore.Data;
using System.Diagnostics;
using System;

namespace GameCore.Services
{
    public class AnimationEntatyService :IAnimationEntatyService 
    {
        public void Play(AnimationType animationType , Animator animator)
        {
            var animationName = "animation";

            switch (animationType)
            {       
                case AnimationType.Idle:
                    animator.SetInteger(animationName, 1);
                    break;
                case AnimationType.Yes:
                    animator.SetInteger(animationName, 2);
                    break;
                case AnimationType.No:
                    animator.SetInteger(animationName, 3);
                    break;
                case AnimationType.Eat:
                    animator.SetInteger(animationName, 4);
                    break;
                case AnimationType.Roar:
                    animator.SetInteger(animationName, 5);
                    break;
                case AnimationType.Jump:
                    animator.SetInteger(animationName, 6);
                    break;
                case AnimationType.Die:
                    animator.SetInteger(animationName, 7);
                    break;
                case AnimationType.Rest:
                    animator.SetInteger(animationName, 8);
                    break;
                case AnimationType.Walk:
                    animator.SetInteger(animationName, 9);
                    break;
                case AnimationType.walk_L:
                    animator.SetInteger(animationName, 10);
                    break;
                case AnimationType.Walk_R:
                    animator.SetInteger(animationName, 11);
                    break;
                case AnimationType.Run:
                    animator.SetInteger(animationName, 12);
                    break;
                case AnimationType.Run_L:
                    animator.SetInteger(animationName, 13);
                    break;
                case AnimationType.Run_R:
                    animator.SetInteger(animationName, 14);
                    break;
                case AnimationType.Fire:
                    animator.SetInteger(animationName, 15);
                    break;
                case AnimationType.Sick:
                    animator.SetInteger(animationName, 16);
                    break;
                case AnimationType.Fly:
                    animator.SetInteger(animationName, 17);
                    break;
                case AnimationType.Fly_L:
                    animator.SetInteger(animationName, 18);
                    break;
                case AnimationType.Fly_R:
                    animator.SetInteger(animationName, 19);
                    break;
                case AnimationType.Fly_Up:
                    animator.SetInteger(animationName, 20);
                    break;
                case AnimationType.Fly_Down:
                    animator.SetInteger(animationName, 21);
                    break;
                case AnimationType.Fly_Fire:
                    animator.SetInteger(animationName, 22);
                    break;
                case AnimationType.Anim_Dra_Demage:
                    animator.SetInteger(animationName, 23);
                    break;
                default: 
                    throw new Exception("Incorect Input");
            }
        }
    }
}

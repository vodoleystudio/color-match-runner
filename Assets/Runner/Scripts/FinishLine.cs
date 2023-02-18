using GameCore.Services;
using GameCore.Data;
using GameCore.UI;
using UnityEngine;
using System.Linq;
using System;
using System.Text.RegularExpressions;

namespace HyperCasual.Runner
{
    /// <summary>
    /// Ends the game on collision, forcing a win state.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(Collider))]
    public class FinishLine : Spawnable
    {
        private const string k_PlayerTag = "Player";
        private const float k_AnimationTime = 2f;

        [SerializeField]
        private Transform m_PlayerEndPosition;

        [SerializeField]
        private Transform m_endCameraPosition;

        [SerializeField]
        private MiniCamera m_miniCamera;

        [SerializeField]
        private Transform m_TargetPosition;

        [SerializeField]
        private PrticleSystemService m_prticleSystemService;

        public Transform TargetPosition => m_TargetPosition;

        private IMatchService m_MatchService = new MatchService();

        private Target GetTargetReference()
        {
            return (Target)LevelManager.Instance.ActiveSpawnables.FirstOrDefault(s => s is Target);
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(k_PlayerTag))
            {
                //GameManager._instance.Win();
                m_miniCamera.Hide();

                if (PlayerController.Instance != null)
                {
                    PlayerController.Instance.Stop();
                    PlayerController.Instance.MoveTo(PlayerController.Instance.Animator, AnimationType.Jump, PlayerController.Instance.Transform, m_PlayerEndPosition, k_AnimationTime, () =>
                    {
                        var matchData = m_MatchService.MatchColors(GetTargetReference().BaseColor, PlayerController.Instance.GetColor());
                        //SaveManager.Instance.SaveLevelData("TestData", new LevelData("TestLevel", 1, 3, matchData));
                        //print(matchData.ToString());
                        m_prticleSystemService.PlayParticleSystem(matchData.m_MatchState);
                        CameraManager.Instance.Hide();
                        EndAnimationSequence.Instance.SetParentPosition(m_endCameraPosition);
                        EndAnimationSequence.Instance.ActivateCamera(CameraManager.Instance.GetCameraTransform());
                    });
                }
            }
        }
    }
}
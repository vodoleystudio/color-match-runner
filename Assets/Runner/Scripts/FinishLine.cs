using GameCore.Services;
using GameCore.Data;
using GameCore.UI;
using UnityEngine;
using System.Linq;
using HyperCasual.Core;
using System.Collections;
using System;
using HyperCasual.Gameplay;

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
        public Transform TargetPosition => m_TargetPosition;

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

        [SerializeField]
        private GenericGameEventListener m_EndGameEvent;

        [SerializeField]
        private GenericGameEventListener m_BackEvent;

        private void ResetCameras()
        {
            CameraManager.Instance.Activate();
            EndAnimationSequence.Instance.HideCamera();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            m_EndGameEvent?.Subscribe();
            m_BackEvent?.Subscribe();
        }

        protected void OnDisable()
        {
            m_EndGameEvent?.Unsubscribe();
            m_BackEvent?.Unsubscribe();
        }

        private void Start()
        {
            if (m_EndGameEvent != null)
            {
                m_EndGameEvent.EventHandler = ResetCameras;
            }
            if (m_BackEvent != null)
            {
                m_BackEvent.EventHandler = ResetCameras;
            }
        }

        private Target GetTargetReference()
        {
            return (Target)LevelManager.Instance.ActiveSpawnables.FirstOrDefault(s => s is Target);
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(k_PlayerTag))
            {
                StartCoroutine(runEndAnimationSequence());
            }
        }

        private void SetupMainCameras()
        {
            CameraManager.Instance.Hide();
            EndAnimationSequence.Instance.ActivateCamera(CameraManager.Instance.GetCameraTransform());
        }

        private IEnumerator runEndAnimationSequence()
        {
            var matchData = GameManager.Instance.MatchService.MatchColors(GetTargetReference().BaseColor, PlayerController.Instance.GetColor());
            m_miniCamera.Hide();
            PlayerController.Instance.StopPlayer();
            PlayerController.Instance.MoveTo(AnimationType.Jump, m_PlayerEndPosition, k_AnimationTime, () =>
            {
                var levelData = new LevelData(LevelManager.Instance.LevelDefinition.name, matchData);
                SaveManager.Instance.SaveLevelData(levelData.LevelId, levelData);
                Debug.LogError(SaveManager.Instance.GetLevelData(levelData.LevelId.ToString()));

                m_prticleSystemService.PlayParticleSystem(matchData.MatchState);
                EndAnimationSequence.Instance.SetParentPosition(m_endCameraPosition);
                SetupMainCameras();
            });

            yield return new WaitForSeconds(k_AnimationTime);

            var gameOverScreen = UIManager.Instance.GetView<GameoverScreen>();
            gameOverScreen.Slider.value = matchData.MatchInPercentage;
            GameManager.Instance.Lose();
            //LevelManager.Instance.LevelDefinition.name
        }
    }
}
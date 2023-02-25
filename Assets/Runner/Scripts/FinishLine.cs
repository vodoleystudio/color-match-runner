using GameCore.Services;
using GameCore.Data;
using GameCore.UI;
using UnityEngine;
using System.Linq;
using HyperCasual.Core;
using System.Collections;

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
        private MatchData m_MatchData;
        private GameoverScreen m_GameOverScreen;
        public MatchData GetMatchData => m_MatchData;
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

        private void ResetMainCameras()
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
            m_GameOverScreen = UIManager.Instance.GetView<GameoverScreen>();
            if (m_EndGameEvent != null)
            {
                m_EndGameEvent.EventHandler = ResetMainCameras;
            }
            if (m_BackEvent != null)
            {
                m_BackEvent.EventHandler = ResetMainCameras;
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
            m_miniCamera.Hide();

            if (PlayerController.Instance != null)
            {
                PlayerController.Instance.Stop();
                PlayerController.Instance.MoveTo(AnimationType.Jump, m_PlayerEndPosition, k_AnimationTime, () =>
                {
                    m_MatchData = GameManager.Instance.MatchService.MatchColors(GetTargetReference().BaseColor, PlayerController.Instance.GetColor());
                    ////var levelData = new LevelData("TestLevel", 1, matchData);
                    ////SaveManager.Instance.SaveLevelData("TestData", new LevelData("TestLevel", 1, matchData));
                    ////Debug.LogError(levelData);
                    m_GameOverScreen.Slider.value = m_MatchData.m_MatchInPercentage;
                    m_prticleSystemService.PlayParticleSystem(m_MatchData.m_MatchState);
                    EndAnimationSequence.Instance.SetParentPosition(m_endCameraPosition);
                    SetupMainCameras();
                });

                yield return new WaitForSeconds(k_AnimationTime);
                GameManager.Instance.Lose();
            }
        }
    }
}
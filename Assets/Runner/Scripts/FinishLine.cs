using GameCore.Services;
using GameCore.Data;
using GameCore.UI;
using UnityEngine;
using System.Linq;
using HyperCasual.Core;
using System.Collections;
using DG.Tweening;
using System;

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
        private const float k_SliderTextAnimationTime = 2.3f;

        private GameoverScreen m_GameOverScreen;
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
        private PrticleSystemService m_ParticleSystemService;

        [SerializeField]
        private GenericGameEventListener m_EndGameEvent;

        [SerializeField]
        private GenericGameEventListener m_BackEvent;

        private Target Target => (Target)LevelManager.Instance.ActiveSpawnables.FirstOrDefault(s => s is Target);

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
            m_GameOverScreen = UIManager.Instance.GetView<GameoverScreen>();
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
            var matchData = GameManager.Instance.MatchService.MatchColors(Target.BaseColor, PlayerController.Instance.GetColor());
            var levelData = new LevelData(LevelManager.Instance.LevelDefinition.name, matchData);
            SaveManager.Instance.SaveLevelData(levelData.LevelId, levelData);
            m_miniCamera.Hide();

            PlayerController.Instance.StopPlayer();
            PlayerController.Instance.MoveTo(AnimationType.Jump, m_PlayerEndPosition, k_AnimationTime, () =>
            {
                EndAnimationSequence.Instance.SetParentPosition(m_endCameraPosition);
                SetupMainCameras();
            });

            yield return new WaitForSeconds(k_AnimationTime / 2);
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayEffect(SoundID.ProgressBarFill);
            m_GameOverScreen.SliderMask.anchorMax = new Vector2(matchData.MatchInPercentage / 100f, 1f);
            DOTween.To((t) => m_GameOverScreen.MatchInProcentText = (int)t, 0f, matchData.MatchInPercentage, k_SliderTextAnimationTime).OnComplete(() => PlayAnimations(matchData));
            StartCoroutine(PlayParticleSystem(matchData));
            GameManager.Instance.Lose();
        }

        private IEnumerator PlayParticleSystem(MatchData matchData)
        {
            yield return new WaitForSeconds(k_SliderTextAnimationTime - 0.1f);
            m_ParticleSystemService.PlayParticleSystem(matchData.MatchState);
        }

        private void PlayAnimations(MatchData matchData)
        {
            switch (matchData.MatchState)
            {
                case MatchState.Heart:
                    play(AnimationType.Jump);
                    AudioManager.Instance.PlayMusic(SoundID.MatchSound);
                    break;

                case MatchState.Like:
                    play(AnimationType.Yes);
                    AudioManager.Instance.PlayMusic(SoundID.PartialMatchSound);
                    break;

                case MatchState.DisLike:
                    play(AnimationType.Roar);
                    AudioManager.Instance.PlayMusic(SoundID.NoMatchSound);
                    break;

                default:
                    throw new Exception($"State {matchData.MatchState} not defined!");
            }

            void play(AnimationType animation)
            {
                AnimationEntityService.Instance.Play(animation, PlayerController.Instance.Animator);
                AnimationEntityService.Instance.Play(animation, Target.Animator);
            }
        }
    }
}
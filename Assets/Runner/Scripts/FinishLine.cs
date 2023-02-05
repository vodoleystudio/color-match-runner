using GameCore.Services;
using GameCore.Data;
using GameCore.UI;
using UnityEngine;

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
        private Transform _endCameraPosition;

        [SerializeField]
        private MiniCamera _miniCamera;

        [SerializeField]
        private Transform m_TargetPosition;

        public Transform TargetPosition => m_TargetPosition;

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(k_PlayerTag))
            {
                //GameManager._instance.Win();
                _miniCamera.Hide();

                if (PlayerController.Instance != null)
                {
                    PlayerController.Instance.Stop();
                    PlayerController.Instance.MoveTo(PlayerController.Instance.Animator, AnimationType.Jump, PlayerController.Instance.Transform, m_PlayerEndPosition, k_AnimationTime, () =>
                    {
                        PlayerController.Instance.SetPosition(m_PlayerEndPosition.position);
                        CameraManager.Instance.Hide();
                        EndAnimationSequence.Instance.SetParentPosition(_endCameraPosition);
                        EndAnimationSequence.Instance.ActivateCamera(CameraManager.Instance.GetCameraTransform());
                    });
                }
            }
        }
    }
}
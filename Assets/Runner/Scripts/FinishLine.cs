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
        private Transform m_endPositionTransform;

        [SerializeField]
        private Transform m_MiniCameraSpot;

        public Transform MiniCameraSpot => m_MiniCameraSpot;

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(k_PlayerTag))
            {
                //GameManager._instance.Win();
                MiniCamera.Instance.Hide();

                if (PlayerController.Instance != null)
                {
                    PlayerController.Instance.Stop();
                    PlayerController.Instance.MoveTo(PlayerController.Instance.Animator, AnimationType.Jump, PlayerController.Instance.Transform, m_endPositionTransform, k_AnimationTime, () =>
                    {
                        PlayerController.Instance.SetPosition(m_endPositionTransform.position);
                        CameraManager.Instance.Hide();
                        EndAnimationSequence.Instance.SetParentPosition(m_endPositionTransform);
                        EndAnimationSequence.Instance.ActivateCamera(CameraManager.Instance.GetCameraTransform());
                    });
                }
            }
        }
    }
}
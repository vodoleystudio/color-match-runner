using GameCore.Services;
using GameCore.Data;
using GameCore.UI;
using UnityEngine;
using DG.Tweening;

namespace HyperCasual.Runner
{
    /// <summary>
    /// Ends the game on collision, forcing a win state.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(Collider))]
    public class FinishLine : Spawnable
    {
        const string k_PlayerTag = "Player";
        const float k_AnimationTime = 2f;
        const float k_MovmentSpeed = 3f;

        [SerializeField]
        Transform _endPositionTransform;

        void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(k_PlayerTag))
            {
                //GameManager._instance.Win();
                MiniCamera.Instance.Hide();

                if (PlayerController.Instance != null) 
                {
                    PlayerController.Instance.Stop();
                    PlayerController.Instance.MoveTo(PlayerController.Instance.Animator, AnimationType.Jump, PlayerController.Instance.Transform, _endPositionTransform, k_AnimationTime, () =>
                    {
                        PlayerController.Instance.SetPosition(_endPositionTransform.position);
                        CameraManager.Instance.Hide();
                        EndAnimationSequance.Instance.SetEndCameraPosition(CameraManager.Instance.GetCameraTransform(), EndAnimationSequance.Instance.GetEndSceneCameraTransform());
                        EndAnimationSequance.Instance.ActivateCamera();
                        
                    });
                }
                //CameraManager._instance.transform.DORotate(new Vector3(0f, 360f, 0f), 10f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
            }
        }
    }
}
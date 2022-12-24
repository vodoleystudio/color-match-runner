using DG.Tweening;
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
        const string k_PlayerTag = "Player";
        
        void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(k_PlayerTag))
            {
                //GameManager.Instance.Win();
                MiniCamera.Instance.Hide();
                if (PlayerController.Instance != null) 
                {
                    PlayerController.Instance.Stop();
                }
                //CameraManager.Instance.transform.DORotate(new Vector3(0f, 360f, 0f), 10f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
            }
        }
    }
}
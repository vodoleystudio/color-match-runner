using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.UI
{
    public class MiniCamera : MonoBehaviour
    {
        private const float AnimationDuration = 0.3f;
        public static MiniCamera Instance => s_Instance;
        private static MiniCamera s_Instance;

        [SerializeField]
        private List<Transform> Objects;

        private void Awake()
        {
            SetupInstance();
        }

        private void OnEnable()
        {
            SetupInstance();
        }

        private void SetupInstance()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;
        }

        public void Show()
        {
            DoScale(Vector3.one);
        }

        public void Hide()
        {
            DoScale(Vector3.zero);
        }

        public void SetPosition(Transform transform)
        {
            gameObject.transform.position = transform.position;
            gameObject.transform.rotation = transform.rotation;
        }

        private void DoScale(Vector3 scale)
        {
            foreach (var obj in Objects)
            {
                obj.DOScale(scale, AnimationDuration);
            }
        }
    }
}
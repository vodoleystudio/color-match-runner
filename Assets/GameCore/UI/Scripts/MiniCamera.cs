using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.UI
{
    public class MiniCamera : MonoBehaviour
    {
        private const float AnimationDuration = 0.3f;

        public static MiniCamera Instance => s_Instance;
        static MiniCamera s_Instance;

        [SerializeField]
        private List<Transform> Objects;

        void Awake()
        {
            SetupInstance();
        }

        void OnEnable()
        {
            SetupInstance();
        }

        void SetupInstance()
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

        private void DoScale(Vector3 scale)
        {
            foreach (var obj in Objects)
            {
                obj.DOScale(scale, AnimationDuration);
            }
        }
    }
}

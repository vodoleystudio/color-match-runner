using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.UI
{
    public class MiniCamera : MonoBehaviour
    {
        private const float AnimationDuration = 0.3f;

        [SerializeField]
        private List<Transform> Objects;

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
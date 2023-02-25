using DG.Tweening;
using UnityEngine;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A class representing a Spawnable object.
    /// If a GameObject tagged "Player" collides
    /// with this object, it will trigger a fail
    /// state with the GameManager.
    /// </summary>
    public class Gate : Entity
    {
        private const string k_PlayerTag = "Player";
        private const float HideDuration = 0.1f;
        private Vector3 defaultScale;

        public bool IsUsed { get; private set; }
        public float MixValue { get; set; }

        private Transform _parent;

        protected override void Awake()
        {
            defaultScale = transform.localScale;
            _parent = transform.parent;
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(k_PlayerTag))
            {
                transform.DOScale(Vector3.zero, HideDuration);
                transform.SetParent(PlayerController.Instance.transform);
                var currentPlayerColor = PlayerController.Instance.GetColor();
                PlayerController.Instance.SetColor(Color.Lerp(BaseColor, currentPlayerColor, MixValue));
            }
        }

        public override void ResetData()
        {
            IsUsed = false;
            transform.localScale = defaultScale;
            transform.SetParent(_parent);
        }
    }
}
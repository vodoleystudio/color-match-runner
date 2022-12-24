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
        const string k_PlayerTag = "Player";
        const float HideDuration = 0.1f;

        public bool IsUsed { get; private set; }
        public float MixValue { get; set; }

        private Transform _parent;

        protected override void Awake() => _parent = transform.parent;

        void OnTriggerEnter(Collider col)
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
            transform.localScale = Vector3.one;
            transform.SetParent(_parent);
        }
    }
}
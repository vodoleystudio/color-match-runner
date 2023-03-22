using UnityEngine;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A class representing a Spawnable object.
    /// If a GameObject tagged "Player" collides
    /// with this object, it will trigger a fail
    /// state with the GameManager.
    /// </summary>
    public class Target : TargetBase
    {
        [SerializeField]
        private SkinnedMeshRenderer m_SkinnedMeshRenderer;

        public Animator Animator { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Animator = GetComponentInChildren<Animator>();
        }

        protected override void OnSetBaseColor(Color color)
        {
            m_SkinnedMeshRenderer.material.color = color;
            m_SkinnedMeshRenderer.material.SetColor("_1st_ShadeColor", color);
            m_SkinnedMeshRenderer.material.SetColor("_BaseColor", color);
        }
    }
}
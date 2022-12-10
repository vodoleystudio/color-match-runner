using System;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A class representing a Spawnable object.
    /// If a GameObject tagged "Player" collides
    /// with this object, it will trigger a fail
    /// state with the GameManager.
    /// </summary>
    public class Block : Spawnable
    {
        const string k_PlayerTag = "Player";

        [SerializeField]
        float m_Value;
        [SerializeField]
        RectTransform m_Text;
        [SerializeField]
        List<Gate> m_Gates;
        [SerializeField]
        Floor m_Floor;

        bool m_Applied;
        Vector3 m_TextInitialScale;

        public List<Gate> Gates => m_Gates;
        public Floor Floor => m_Floor;
        public int Score { get; set; }

        public bool HasGate(Gate gate)
        {
            return Gates.Contains(gate);
        }

        /// <summary>
        /// Sets the local scale of this spawnable object
        /// and ensures the Text attached to this gate
        /// does not scale.
        /// </summary>
        /// <param name="scale">
        /// The scale to apply to this spawnable object.
        /// </param>
        public override void SetScale(Vector3 scale)
        {
            // Ensure the text does not get scaled
            if (m_Text != null)
            {
                float xFactor = Mathf.Min(scale.y / scale.x, 1.0f);
                float yFactor = Mathf.Min(scale.x / scale.y, 1.0f);
                m_Text.localScale = Vector3.Scale(m_TextInitialScale, new Vector3(xFactor, yFactor, 1.0f));

                m_Transform.localScale = scale;
            }
        }

        /// <summary>
        /// Reset the gate to its initial state. Called when a level
        /// is restarted by the GameManager.
        /// </summary>
        public override void ResetData()
        {
            m_Applied = false;
            foreach (Gate gate in m_Gates)
            {
                gate.ResetData();
            }
        }

        protected override void Awake()
        {
            base.Awake();

            if (m_Text != null)
            {
                m_TextInitialScale = m_Text.localScale;
            }
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(k_PlayerTag) && !m_Applied)
            {
                m_Applied = true;
                GameManager.Instance.UpdateProgress(this);
            }
        }
    }
}
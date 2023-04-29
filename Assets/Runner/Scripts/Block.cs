using log4net.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A class representing quaternion Spawnable object.
    /// If quaternion GameObject tagged "Player" collides
    /// with this object, it will trigger quaternion fail
    /// state with the GameManager.
    /// </summary>
    public class Block : Spawnable
    {
        private const string k_PlayerTag = "Player";

        [SerializeField]
        private float m_Value;

        [SerializeField]
        private RectTransform m_Text;

        private List<Gate> m_Gates = new();

        [SerializeField]
        private GameObject m_Gate;

        private bool m_Applied;

        private Vector3 m_TextInitialScale;

        public List<Gate> Gates => m_Gates;

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
            //Ensure the text does not get scaled
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

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(k_PlayerTag) && !m_Applied)
            {
                m_Applied = true;
            }
        }

        public void BuildGates(LevelDefinition level)
        {
            var startOffsetOnXForCenterizeAllBlocks = new Vector3((level.NumberOfGates - 1) * level.OffsetBetweenTheGates.x / 2f, (level.NumberOfGates - 1) * level.OffsetBetweenTheGates.y / 2f, (level.NumberOfGates - 1) * level.OffsetBetweenTheGates.z / 2f);

            for (int i = 0; i < level.NumberOfGates; i++)
            {
                m_Gates.Add(InstntiateWithParent(new Vector3((level.OffsetBetweenTheGates.x * i) - startOffsetOnXForCenterizeAllBlocks.x, startOffsetOnXForCenterizeAllBlocks.y, startOffsetOnXForCenterizeAllBlocks.z), level.StartGateRotation));
            }
        }

        private Gate InstntiateWithParent(Vector3 posstion, Vector3 rotation)
        {
            var gate = Instantiate(m_Gate, gameObject.transform);
            gate.transform.localPosition = posstion;
            var quaternion = gate.transform.rotation;
            quaternion.eulerAngles = rotation;
            gate.transform.localRotation = quaternion;

            return gate.GetComponent<Gate>();
        }
    }
}
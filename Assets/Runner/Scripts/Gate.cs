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

        public bool IsUsed { get; private set; }

        void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(k_PlayerTag))
            {
                IsUsed = true;  
            }
        }

        public override void ResetData()
        {
            IsUsed = false;
        }
    }
}
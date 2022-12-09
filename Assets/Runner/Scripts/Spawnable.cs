using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HyperCasual.Runner
{
    /// <summary>
    /// A base class for all objects which populate a
    /// LevelDefinition. This class includes all logic 
    /// necessary for snapping an object to a level's grid.
    /// </summary>
    [ExecuteInEditMode]
    public class Spawnable : Entity
    {
        protected override void Awake()
        {
            base.Awake();

            if (LevelManager.Instance != null)
            {
#if UNITY_EDITOR
                if (PrefabUtility.IsPartOfNonAssetPrefabInstance(gameObject))
#endif
                    m_Transform.SetParent(LevelManager.Instance.transform);
            }
        }
    }
}
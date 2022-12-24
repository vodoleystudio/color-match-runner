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
        public override void SetBaseColor(Color baseColor)
        {
            var meshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (meshRenderers != null)
            {
                for (int i = 0; i < meshRenderers.Length; i++)
                {
                    var meshRenderer = meshRenderers[i];

                    if (meshRenderer != null && meshRenderer.sharedMaterial != null)
                    {
                        Material material = new Material(meshRenderer.sharedMaterial);
                        material.SetColor("_1st_ShadeColor", baseColor);
                        material.SetColor("_BaseColor", baseColor);
                        material.color = baseColor;
                        meshRenderer.sharedMaterial = material;
                    }
                }
            }
        }
    }
}
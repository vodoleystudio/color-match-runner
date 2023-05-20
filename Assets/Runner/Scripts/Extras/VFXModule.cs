using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using HyperCasual.Core;

public class VFXModule : AbstractSingleton<VFXModule>, IVFXModule
{
    [SerializeField]
    private List<VFXData> _vfxData;

    private Dictionary<VFXType, List<ParticleSystem>> _vfxDictionary = new Dictionary<VFXType, List<ParticleSystem>>();

    protected override void Awake()
    {
        base.Awake();
        foreach (var vfxData in _vfxData)
        {
            if (!vfxData.Particles.Any())
            {
                continue;
            }

            _vfxDictionary.Add(vfxData.VFXType, new List<ParticleSystem>());
            foreach (var particle in vfxData.Particles)
            {
                _vfxDictionary[vfxData.VFXType].Add(Instantiate(particle, transform).GetComponent<ParticleSystem>());
            }
        }
    }

    public void Play(VFXType type, Vector3 position)
    {
        if (_vfxDictionary.ContainsKey(type) && _vfxDictionary[type].Any())
        {
            var effect = _vfxDictionary[type][Random.Range(0, _vfxDictionary[type].Count())];
            if (effect.isPlaying)
            {
                effect.Stop();
            }
            effect.gameObject.transform.position = position;
            effect.Play();
        }
    }
}

public enum VFXType
{
    CloudKilled,
    SpecialWin
}

[Serializable]
public class VFXData
{
    [SerializeField]
    private VFXType _VFXType;

    public VFXType VFXType => _VFXType;

    [SerializeField]
    private GameObject[] _particlesSystem;

    public GameObject[] Particles => _particlesSystem;
}

public interface IVFXModule
{
    void Play(VFXType type, Vector3 position);
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum EffectType
{
    GameObject,
    Particle,
    Material,
}

public class EffectData
{
    EffectType _effectType;
    string _name;
    string _path;

    public EffectType EffectType => _effectType;
    public string Name => _name;
    public string Path => _path;
}

public class EffectManager
{
    GameObject[] objects;
    ParticleSystem[] particles;
    Material[] materials;

    public void Init()
    {

    }

    void ChaseToTarget()
    {

    }

    void PlayParticle()
    {

    }

    public void ChangeMaterial()
    {

    }

    public void ChangeAllMaterial(Transform transform)
    {
        transform.GetComponentInChildren<MeshRenderer>().material = null;
    }

    void ChangeColor()
    {

    }
}

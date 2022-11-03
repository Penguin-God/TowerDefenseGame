using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EffectManager
{
    class EffectData
    {
        EffectType _effectType;
        string _name;
        string _path;
    }

    enum EffectType
    {

    }

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

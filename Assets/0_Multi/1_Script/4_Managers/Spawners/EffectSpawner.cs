using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public enum Effects
{
    Unit_Tp_Effect,

}

public class EffectSpawner : Multi_SpawnerBase
{
    [SerializeField] GameObject[] effects;
    [SerializeField] int count;

    protected override void MasterInit()
    {
        foreach (GameObject effect in effects)
            CreatePool_InGroup(effect, count);
    }

    public void Play(Effects type, Vector3 pos)
    {
        ParticleSystem particle = Spawn(type, pos);
        if (particle != null)
        {
            particle.Play();
            StartCoroutine(Co_AfterPush(particle.gameObject, particle.main.duration));
        }
    }

    IEnumerator Co_AfterPush(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        Multi_Managers.Pool.Push(go);
    }

    ParticleSystem Spawn(Effects type, Vector3 pos) 
        => Multi_Managers.Resources.PhotonInsantiate($"{_rootPath}/{Enum.GetName(typeof(Effects), type)}", pos).GetComponent<ParticleSystem>();
}

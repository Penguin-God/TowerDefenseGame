using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public enum Effects
{
    Unit_Tp_Effect,
    WhiteUnit_Timer,
}

public class EffectSpawner : Multi_SpawnerBase
{
    [SerializeField] GameObject[] effects;
    [SerializeField] int count;

    protected override void MasterInit()
    {
        foreach (GameObject effect in effects)
            CreatePoolGroup(effect, BuildPath(_rootPath, effect), count);
    }

    public GameObject ShwoForTime(Effects type, Vector3 pos, float aliveTime)
    {
        GameObject effect = Spawn(type, pos);
        StartCoroutine(Co_AfterPush(effect, aliveTime));
        return effect;
    }

    public void Play(Effects type, Vector3 pos)
    {
        ParticleSystem particle = Spawn(type, pos).GetComponent<ParticleSystem>();
        if (particle != null)
        {
            particle.GetComponent<RPCable>().PlayParticle_RPC();
            StartCoroutine(Co_AfterPush(particle.gameObject, particle.main.duration));
        }
    }

    IEnumerator Co_AfterPush(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        Multi_Managers.Pool.Push(go);
    }

    public void ChangedMaterials()
    {

    }

    GameObject Spawn(Effects type, Vector3 pos) => Multi_Managers.Resources.PhotonInsantiate($"{_rootPath}/{Enum.GetName(typeof(Effects), type)}", pos);
}

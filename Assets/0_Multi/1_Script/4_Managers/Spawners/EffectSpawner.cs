﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public enum Effects
{
    UnitTpEffect,
    WhiteUnitTimer,
}

public class EffectSpawner : Multi_SpawnerBase
{
    [SerializeField] GameObject[] effects;
    [SerializeField] int count;

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
        Managers.Pool.Push(go);
    }

    GameObject Spawn(Effects type, Vector3 pos) => Managers.Multi.Instantiater.PhotonInstantiate($"{_rootPath}/{Enum.GetName(typeof(Effects), type)}", pos);
}

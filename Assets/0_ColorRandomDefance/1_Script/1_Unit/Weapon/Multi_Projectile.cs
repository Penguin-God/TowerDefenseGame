﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

public class Multi_Projectile : MonoBehaviourPun
{
    [SerializeField] bool isAOE; // area of effect : 범위(광역) 공격
    [SerializeField] float aliveTime = 5f;
    [SerializeField] protected int _speed;
    protected Rigidbody Rigidbody = null;
    protected Action<Multi_Enemy> OnHit = null;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        StartCoroutine(Co_Inactive(aliveTime));
    }

    public void Shot(Vector3 dir, Action<Multi_Enemy> hitAction)
    {
        OnHit = hitAction;
        photonView.RPC(nameof(RPC_Shot), RpcTarget.All, dir.x, dir.z);
    }

    [PunRPC]
    void RPC_Shot(float x, float z) => RPC_Shot(new Vector3(x, 0, z));

    [PunRPC]
    protected void RPC_Shot(Vector3 dir)
    {
        Rigidbody.velocity = dir * _speed;
        Quaternion lookDir = Quaternion.LookRotation(dir);
        transform.rotation = lookDir;
    }

    void HitEnemy(Multi_Enemy enemy)
    {
        Debug.Assert(OnHit != null, "OnHit이 널임");

        OnHit?.Invoke(enemy);
        if (!isAOE)
        {
            StopAllCoroutines();
            ReturnObjet();
        }
    }

    IEnumerator Co_Inactive(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        ReturnObjet();
    }

    protected void ReturnObjet()
    {
        OnHit = null;
        if (PhotonNetwork.IsMasterClient == false) return;
        Managers.Multi.Instantiater.PhotonDestroy(gameObject);
    }

    protected virtual void OnTriggerHit(Collider other) 
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Multi_Enemy enemy = other.GetComponentInParent<Multi_Enemy>(); // 콜라이더가 자식한테 있음
            if (enemy != null) HitEnemy(enemy);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerHit(other);
    }
}

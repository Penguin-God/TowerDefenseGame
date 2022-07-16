﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class Multi_HitSkill : MonoBehaviourPun
{
    private void Awake() => sphereCollider = GetComponent<SphereCollider>();

    public event Action<Multi_Enemy> OnHitSkile;
    public void SetHitActoin(Action<Multi_Enemy> action) => OnHitSkile = action;

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;

        if (other.GetComponentInParent<Multi_Enemy>() != null)
        {
            if (OnHitSkile != null)
                OnHitSkile(other.GetComponentInParent<Multi_Enemy>());
        }
    }

    [SerializeField] protected SphereCollider sphereCollider;
    [SerializeField] private float activeDelayTime; // 콜라이더가 켜지기 전 공격 대기 시간
    [SerializeField] private float aliveTime; // 콜라이더가 켜지기 전 공격 대기 시간

    private void OnEnable() => StartCoroutine(Co_OnCollider(activeDelayTime));
    IEnumerator Co_OnCollider(float delayTIme)
    {
        yield return new WaitForSeconds(delayTIme);
        sphereCollider.enabled = true;
        yield return new WaitForSeconds(delayTIme);
        sphereCollider.enabled = false;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Multi_HitSkill : MonoBehaviour
{
    private void Awake() => sphereCollider = GetComponent<SphereCollider>();

    event Action<Multi_Enemy> OnHitSkile;
    public void SetHitActoin(Action<Multi_Enemy> action) => OnHitSkile = action;

    void OnTriggerEnter(Collider other)
    {
        var monster = other.GetComponentInParent<Multi_Enemy>();
        if (monster != null)
            OnHitSkile?.Invoke(monster);
    }

    SphereCollider sphereCollider;
    [SerializeField] private float activeDelayTime; // 콜라이더가 켜지기 전 공격 대기 시간
    [SerializeField] private float hitTime; // 콜라이더가 켜져 있는 시간

    private void OnEnable() => StartCoroutine(Co_OnCollider());
    IEnumerator Co_OnCollider()
    {
        yield return new WaitForSeconds(activeDelayTime);
        sphereCollider.enabled = true;
        Debug.Assert(hitTime > 0, "Hit Time 설정 잘못됨");
        yield return new WaitForSeconds(hitTime);
        sphereCollider.enabled = false;
    }
}

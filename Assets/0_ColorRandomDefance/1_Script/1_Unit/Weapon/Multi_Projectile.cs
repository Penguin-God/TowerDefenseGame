using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

public class Multi_Projectile : MonoBehaviour
{
    [SerializeField] bool isAOE; // area of effect : 범위(광역) 공격
    [SerializeField] protected int _speed;
    protected Rigidbody Rigidbody = null;
    protected Action<Multi_Enemy> OnHit = null;
    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    protected void Shot(Vector3 dir)
    {
        Rigidbody.velocity = dir.normalized * _speed;
        Quaternion lookDir = Quaternion.LookRotation(dir);
        transform.rotation = lookDir;
    }

    public void AttackShot(Vector3 dir, Action<Multi_Enemy> hitAction)
    {
        OnHit = hitAction;
        Shot(dir);
    }

    protected virtual void OnTriggerHit(Collider other)
    {
        if (other.transform.parent == null) return;

        // 컴포넌트가 부모에게 있음
        if (other.transform.parent.TryGetComponent<Multi_Enemy>(out var enemy) == false) return;
        if (PhotonNetwork.IsMasterClient) OnHit?.Invoke(enemy);
        if (isAOE == false) GetComponent<AutoDestoryAfterSecond>().ReturnObjet();
    }

    void OnTriggerEnter(Collider other)
    {
        OnTriggerHit(other);
    }

    void OnDisable() => OnHit = null;
}

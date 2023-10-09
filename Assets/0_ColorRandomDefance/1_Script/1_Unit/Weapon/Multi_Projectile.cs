using System.Collections;
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

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        StartCoroutine(Co_Inactive(aliveTime));
    }

    [PunRPC]
    protected void RPC_Shot(Vector3 dir)
    {
        Rigidbody.velocity = dir * _speed;
        Quaternion lookDir = Quaternion.LookRotation(dir);
        transform.rotation = lookDir;
    }

    public void AttackShot(Vector3 dir, Action<Multi_Enemy> hitAction)
    {
        OnHit = hitAction;
        RPC_Shot(dir);
    }

    IEnumerator Co_Inactive(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        ReturnObjet();
    }

    void ReturnObjet()
    {
        OnHit = null;
        StopAllCoroutines();
        Managers.Resources.Destroy(gameObject);
    }

    protected virtual void OnTriggerHit(Collider other) 
    {
        if(PhotonNetwork.IsMasterClient == false || other.transform.parent == null) return;
        // 컴포넌트가 부모에게 있음
        if(other.transform.parent.TryGetComponent<Multi_Enemy>(out var enemy) == false)
            return;

        OnHit?.Invoke(enemy);
        if (isAOE == false)
            ReturnObjet();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerHit(other);
    }
}

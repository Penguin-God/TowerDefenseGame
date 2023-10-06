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

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        StartCoroutine(Co_Inactive(aliveTime));
    }

    public void SetHitAction(Action<Multi_Enemy> hitAction) => OnHit = hitAction;

    public void Throw(Vector3 dir)
    {
        Rigidbody.velocity = dir * _speed;
        Quaternion lookDir = Quaternion.LookRotation(dir);
        transform.rotation = lookDir;
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
        gameObject.SetActive(false);
        if (PhotonNetwork.IsMasterClient && gameObject.GetComponent<Poolable>() != null) // PhotonNetwork.IsMasterClient 는 검은 법사 스킬 때문
            Managers.Pool.Push(gameObject.GetComponent<Poolable>());
    }

    protected virtual void OnTriggerHit(Collider other) 
    {
        if(PhotonNetwork.IsMasterClient == false) return;
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

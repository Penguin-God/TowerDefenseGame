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

    public void Shot(Vector3 dir, Action<Multi_Enemy> hitAction)
    {
        OnHit = hitAction;
        photonView.RPC("RPC_ProjectileShot", RpcTarget.All, dir);
    }

    [PunRPC]
    public void RPC_ProjectileShot(Vector3 _dir)
    {
        Rigidbody.velocity = _dir * _speed;
        Quaternion lookDir = Quaternion.LookRotation(_dir);
        transform.rotation = lookDir;
    }

    // TODO : 법사 스킬에서 사용중인데 새로 만든 Shot으로 갈아버려야 됨
    #region 레거시

    public void Shot(Vector3 pos, Vector3 dir, int speed, Action<Multi_Enemy> hitAction)
    {
        OnHit = hitAction;
        photonView.RPC("SetShotData", RpcTarget.All, pos, dir, speed);
        RPC_Utility.Instance.RPC_Active(photonView.ViewID, true);
    }

    [PunRPC]
    public void SetShotData(Vector3 _pos, Vector3 _dir, int speed)
    {
        transform.position = _pos;
        Rigidbody.velocity = _dir * speed;
        Quaternion lookDir = Quaternion.LookRotation(_dir);
        transform.rotation = lookDir;
    }

    #endregion


    void HitEnemy(Multi_Enemy enemy)
    {
        if (OnHit == null) return;

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
        Multi_Managers.Pool.Push(gameObject.GetOrAddComponent<Poolable>());
    }

    protected virtual void OnTriggerHit(Collider other) 
    {
        if (photonView.IsMine)
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

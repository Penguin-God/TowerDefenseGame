using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

public class Multi_Projectile : MonoBehaviourPun
{
    [SerializeField] bool isAOE; // area of effect : 범위(광역) 공격
    [SerializeField] float aliveTime = 5f; 
    Rigidbody Rigidbody = null;
    private Action<Multi_Enemy> OnHit = null;
    //[HideInInspector] public MyPunRPC myRPC = null;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        StartCoroutine(Co_Inactive(aliveTime));
    }

    [SerializeField] int _speed;
    public void Shot(Vector3 dir, Action<Multi_Enemy> hitAction)
    {
        OnHit = hitAction;
        photonView.RPC("RPC_ProjectileShot", RpcTarget.All, dir);
        RPC_Utility.Instance.RPC_Active(photonView.ViewID, true);
    }

    [PunRPC]
    public void RPC_ProjectileShot(Vector3 _dir)
    {
        Rigidbody.velocity = _dir * _speed;
        Quaternion lookDir = Quaternion.LookRotation(_dir);
        transform.rotation = lookDir;
    }

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
        OnHit?.Invoke(enemy);
        if (!isAOE)
        {
            StopAllCoroutines(); // 코루틴 하나만 있으니까 임시로 씀
            ReturnObjet();
        }
    }

    IEnumerator Co_Inactive(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        ReturnObjet();
    }

    void ReturnObjet()
    {
        OnHit = null;
        Multi_Managers.Pool.Push(gameObject.GetOrAddComponent<Poolable>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            Multi_Enemy enemy = other.GetComponentInParent<Multi_Enemy>();
            if (enemy != null) HitEnemy(enemy);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

public class Multi_Projectile : MonoBehaviourPun
{
    [SerializeField] bool isAOE; // area of effect : 범위(광역) 공격

    Rigidbody Rigidbody = null;
    private Action<Multi_Enemy> OnHit = null;
    //[HideInInspector] public MyPunRPC myRPC = null;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void Shot(Vector3 pos, Vector3 dir, int speed, Action<Multi_Enemy> hitAction)
    {
        OnHit = hitAction;
        photonView.RPC("SetShotData", RpcTarget.All, pos, dir, speed);
    }

    [PunRPC]
    public void SetShotData(Vector3 _pos, Vector3 _dir, int _speed)
    {
        transform.position = _pos;
        Rigidbody.velocity = _dir * _speed;
        Quaternion lookDir = Quaternion.LookRotation(_dir);
        transform.rotation = lookDir;
    }

    void HitEnemy(Multi_Enemy enemy)
    {
        OnHit?.Invoke(enemy);
        if (!isAOE) RPC_Utility.Instance.RPC_Active(photonView.ViewID, false);
        OnHit = null;
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

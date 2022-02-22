using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

public class Multi_Projectile : MonoBehaviourPun
{
    [SerializeField] bool isAOE; // area of effect : 범위(광역) 공격

    Rigidbody Rigidbody = null;
    [HideInInspector] public MyPunRPC myRPC = null;
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        gameObject.AddComponent<MyPunRPC>();
        myRPC = GetComponent<MyPunRPC>();
    }

    //[PunRPC]
    //public void SetActive(bool _isActive) => gameObject.SetActive(_isActive);

    private Action<Multi_Enemy> OnHit = null;
    public void Shot(Vector3 pos, Vector3 dir, int speed, Action<Multi_Enemy> action)
    {
        OnHit = null;
        OnHit = action;
        if (!isAOE) OnHit += (Multi_Enemy enemy) => myRPC.RPC_Active(false);
        photonView.RPC("SetVector", RpcTarget.All, pos, dir, speed);
    }

    [PunRPC]
    public void SetVector(Vector3 _pos, Vector3 _dir, int _speed)
    {
        transform.position = _pos;
        Rigidbody.velocity = _dir * _speed;
        Quaternion lookDir = Quaternion.LookRotation(_dir);
        transform.rotation = lookDir;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.gameObject.GetComponentInParent<Multi_Enemy>() != null)
            {
                if (OnHit != null)
                    OnHit(other.GetComponentInParent<Multi_Enemy>());
            }
        }
    }
}

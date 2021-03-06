using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Multi_Meteor : Multi_Projectile
{
    [SerializeField] GameObject explosionObject;
    Action<Multi_Enemy> explosionAction = null;

    public void Shot(Multi_Enemy enemy, Vector3 enemyPos, Action<Multi_Enemy> hitAction)
    {
        Vector3 chasePos = enemyPos + ( (enemy != null) ? enemy.dir.normalized * enemy.speed : Vector3.zero);
        Shot((chasePos - transform.position).normalized, null);
        explosionAction = hitAction;
    }

    protected override void OnTriggerHit(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (other.tag == "World") photonView.RPC("MeteorExplosion", RpcTarget.All);
    }

    [PunRPC]
    void MeteorExplosion() // 메테오 폭발
    {
        if(explosionAction != null)
        {
            Multi_Managers.Resources.PhotonInsantiate(explosionObject, transform.position).GetComponent<Multi_HitSkill>().SetHitActoin(explosionAction);
            explosionAction = null;
        }

        Rigidbody.velocity = Vector3.zero;
        ReturnObjet();
    }
}

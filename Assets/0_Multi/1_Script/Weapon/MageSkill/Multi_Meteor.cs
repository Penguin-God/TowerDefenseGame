using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Multi_Meteor : Multi_Projectile
{
    [SerializeField] GameObject explosionObject;
    Action<Multi_Enemy> explosionAction = null;

    void Start()
    {
        _renderer = gameObject.GetOrAddComponent<MeshRenderer>();
    }

    public void Shot(Multi_Enemy enemy, Vector3 enemyPos, Action<Multi_Enemy> hitAction)
    {
        Vector3 chasePos = enemyPos + ( (enemy != null) ? enemy.dir.normalized * enemy.Speed : Vector3.zero);
        Shot((chasePos - transform.position).normalized, null);
        explosionAction = hitAction;
    }

    protected override void OnTriggerHit(Collider other)
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        if (other.tag == "World") photonView.RPC("MeteorExplosion", RpcTarget.All);
    }

    Renderer _renderer;
    [PunRPC]
    void MeteorExplosion() // 메테오 폭발
    {
        if(explosionAction != null)
        {
            Multi_Managers.Sound.PlayEffect_If(EffectSoundType.MeteorExplosion, () => _renderer.isVisible);
            Multi_Managers.Resources.PhotonInsantiate(explosionObject, transform.position).GetComponent<Multi_HitSkill>().SetHitActoin(explosionAction);
            explosionAction = null;
        }

        Rigidbody.velocity = Vector3.zero;
        ReturnObjet();
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Multi_Meteor : Multi_Projectile
{
    const string expolsionPath = "Prefabs/Weapon/MageSkills/Meteor_Explosion";
    Action<Multi_Enemy> explosionAction = null;

    void Start()
    {
        _renderer = gameObject.GetOrAddComponent<MeshRenderer>();
    }

    public void Shot(Multi_Enemy enemy, Vector3 enemyPos, Action<Multi_Enemy> hitAction)
    {
        explosionAction = hitAction;
        Vector3 chasePos = enemyPos + ( (enemy != null) ? enemy.dir.normalized * enemy.Speed : Vector3.zero);
        photonView.RPC(nameof(RPC_Shot), RpcTarget.All, (chasePos - transform.position).normalized);
    }

    protected override void OnTriggerHit(Collider other)
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        if (other.tag == "World") photonView.RPC(nameof(MeteorExplosion), RpcTarget.All);
    }

    Renderer _renderer;
    [PunRPC]
    void MeteorExplosion() // 메테오 폭발
    {
        if(explosionAction != null)
        {
            Managers.Sound.PlayEffect_If(EffectSoundType.MeteorExplosion, () => _renderer.isVisible);
            Managers.Multi.Instantiater.PhotonInstantiate(expolsionPath, transform.position).GetComponent<Multi_HitSkill>().SetHitActoin(explosionAction);
            explosionAction = null;
        }

        Rigidbody.velocity = Vector3.zero;
        Managers.Multi.Instantiater.PhotonDestroy(gameObject);
    }
}

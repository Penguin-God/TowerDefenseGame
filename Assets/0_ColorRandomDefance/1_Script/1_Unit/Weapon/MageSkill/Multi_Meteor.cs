using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Multi_Meteor : Multi_Projectile
{
    readonly string ExpolsionPath = new ResourcesPathBuilder().BuildEffectPath(SkillEffectType.MeteorExplosion);
    Action<Multi_Enemy> explosionAction = null;

    void Start()
    {
        _renderer = gameObject.GetOrAddComponent<MeshRenderer>();
    }

    public void Shot(Vector3 dir, Action<Multi_Enemy> hitAction)
    {
        explosionAction = hitAction;
        RPC_Shot(dir);
    }

    protected override void OnTriggerHit(Collider other)
    {
        if (other.tag == "World")
            MeteorExplosion();
    }

    Renderer _renderer;
    [PunRPC]
    void MeteorExplosion() // 메테오 폭발
    {
        if(explosionAction != null)
        {
            if(_renderer.isVisible)
                Managers.Sound.PlayEffect(EffectSoundType.MeteorExplosion);
            Managers.Resources.Instantiate(ExpolsionPath, transform.position).GetComponent<Multi_HitSkill>().SetHitActoin(explosionAction);
            explosionAction = null;
        }

        Rigidbody.velocity = Vector3.zero;
        Managers.Resources.Destroy(gameObject);
    }
}

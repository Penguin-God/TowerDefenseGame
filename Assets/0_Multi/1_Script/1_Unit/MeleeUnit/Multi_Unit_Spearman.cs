using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Unit_Spearman : Multi_MeleeUnit
{
    [Header("창병 변수")]

    [SerializeField] ProjectileData shotSpearData;
    [SerializeField] GameObject trail;
    [SerializeField] GameObject spear; // 평타칠 때 쓰는 창

    [SerializeField]
    private AudioClip skillAudioClip;

    public override void OnAwake()
    {
        shotSpearData = new ProjectileData(Multi_Managers.Data.WeaponDataByUnitFlag[UnitFlags].Paths[0], transform, shotSpearData.SpawnTransform);
    }

    public override void SetInherenceData()
    {
        skillDamage = (int)(damage * 1.5f);
    }


    public override void NormalAttack() => StartCoroutine("SpaerAttack");
    IEnumerator SpaerAttack()
    {
        base.StartAttack();

        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.55f);
        trail.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        if(pv.IsMine) HitMeeleAttack();
        yield return new WaitForSeconds(0.3f);
        trail.SetActive(false);

        EndAttack();
    }


    public override void SpecialAttack() => StartCoroutine("Spearman_SpecialAttack");
    IEnumerator Spearman_SpecialAttack()
    {
        base.SpecialAttack();
        animator.SetTrigger("isSpecialAttack");
        yield return new WaitForSeconds(1f);

        spear.SetActive(false);
        nav.isStopped = true;

        if (PhotonNetwork.IsMasterClient)
        {
            Multi_Projectile weapon = ProjectileShotDelegate.ShotProjectile(shotSpearData, transform.forward, OnSkileHit);
            weapon.GetComponent<RPCable>().SetRotate_RPC(new Vector3(90, 0, 0));
        }

        if (enterStoryWorld == Multi_GameManager.instance.playerEnterStoryMode)
            unitAudioSource.PlayOneShot(skillAudioClip, 0.12f);

        yield return new WaitForSeconds(0.5f);
        nav.isStopped = false;
        spear.SetActive(true);

        SkillCoolDown(skillCoolDownTime);
    }
}

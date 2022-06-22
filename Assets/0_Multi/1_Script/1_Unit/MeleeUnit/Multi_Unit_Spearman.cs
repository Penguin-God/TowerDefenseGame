using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Unit_Spearman : Multi_MeleeUnit
{
    [Header("창병 변수")]

    [SerializeField] ProjectileData projectileData;
    [SerializeField] GameObject trail;
    [SerializeField] GameObject spear; // 평타칠 때 쓰는 창
    [SerializeField] GameObject skillSpear; // 발사할 때 생성하는 창
    [SerializeField] Transform spearCreatePosition;

    [SerializeField]
    private AudioClip skillAudioClip;

    [ContextMenu("set data")]
    void SetData()
    {
        projectileData = new ProjectileData(projectileData.Original, spearCreatePosition, null);
    }

    public override void OnAwake()
    {
        Debug.Assert(projectileData.Original != null && projectileData.SpawnTransform != null, "projectileData가 설정되어 있지 않음");
        projectileData = new ProjectileData(projectileData.Original, projectileData.SpawnTransform, OnSkileHit);
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

        if (pv.IsMine)
        {
            Multi_Projectile weapon = ShotProjectile(projectileData, transform.forward);
            RPC_Utility.Instance.RPC_Rotate(weapon.photonView.ViewID, new Vector3(90, 0, 0));
        }

        if (enterStoryWorld == Multi_GameManager.instance.playerEnterStoryMode)
            unitAudioSource.PlayOneShot(skillAudioClip, 0.12f);

        yield return new WaitForSeconds(0.5f);
        nav.isStopped = false;
        spear.SetActive(true);

        SkillCoolDown(skillCoolDownTime);
    }
}

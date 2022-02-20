using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Unit_Spearman : Multi_MeleeUnit
{
    [Header("창병 변수")]

    [SerializeField] GameObject trail;
    [SerializeField] GameObject spear; // 평타칠 때 쓰는 창
    [SerializeField] GameObject skileSpaer; // 발사할 때 생성하는 창
    [SerializeField] Transform spearCreatePosition;

    [SerializeField]
    private AudioClip skillAudioClip;

    public override void OnAwake()
    {
        poolManager.SettingWeaponPool(skileSpaer, 5);
    }

    public override void SetInherenceData()
    {
        skillDamage = (int)(damage * 1.5f);
    }

    [PunRPC]
    public override void NormalAttack()
    {
        StartCoroutine("SpaerAttack");
    }

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


    public override void SpecialAttack()
    {
        StartCoroutine("Spearman_SpecialAttack");
    }

    IEnumerator Spearman_SpecialAttack()
    {
        isAttack = true;
        animator.SetTrigger("isSpecialAttack");
        yield return new WaitForSeconds(1f);

        spear.SetActive(false);
        nav.isStopped = true;

        //CollisionWeapon weapon = poolManager.UsedWeapon(spearCreatePosition, transform.forward * -1, 50, (Enemy enemy) => delegate_OnSkile(enemy));
        //weapon.transform.Rotate(90, 0, 0);

        if (enterStoryWorld == Multi_GameManager.instance.playerEnterStoryMode)
            unitAudioSource.PlayOneShot(skillAudioClip, 0.12f);

        yield return new WaitForSeconds(0.5f);
        nav.isStopped = false;
        spear.SetActive(true);

        base.NormalAttack();
    }
}

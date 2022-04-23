using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Spearman : MeeleUnit, IEvent
{
    [Header("창병 변수")]
    public GameObject trail;
    public GameObject spear;
    public GameObject skileSpaer; // 발사할 때 생성하는 창
    public Transform spearCreatePosition;

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
        HitMeeleAttack();
        yield return new WaitForSeconds(0.3f);
        trail.SetActive(false);

        base.NormalAttack();
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

        CollisionWeapon weapon = poolManager.UsedWeapon(spearCreatePosition, transform.forward * -1, 50, (Enemy enemy) => delegate_OnSkile(enemy));
        weapon.transform.Rotate(90, 0, 0);

        if (enterStoryWorld == GameManager.instance.playerEnterStoryMode)
            unitAudioSource.PlayOneShot(skillAudioClip, 0.12f);

        yield return new WaitForSeconds(0.5f);
        nav.isStopped = false;
        spear.SetActive(true);

        base.NormalAttack();
    }


    // 스킬 사용 빈도 증가 이벤트
    public void SkillPercentUp()
    {
        specialAttackPercent += 30;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Multi_GreenMage : Multi_Unit_Mage
{
    public override void SetMageAwake()
    {
        attackRange *= 2; // 패시브
    }

    void ShootSkill()
    {
        ProjectileShotDelegate.ShotProjectile(SkillSpawn(energyBallTransform.position).GetComponent<Multi_Projectile>(), transform, target, 2, OnSkileHit);
    }

    protected override void _MageSkile()
    {
        StartCoroutine(Co_GreenMageSkile());
        StartCoroutine(Co_FixMana());
    }

    IEnumerator Co_GreenMageSkile()
    {
        nav.isStopped = true;
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.7f);
        ShootSkill();

        yield return new WaitForSeconds(0.5f);
        nav.isStopped = false;
    }

    // isSkillAttack 조절이 안되서 지금은 의미없는 코드임
    IEnumerator Co_FixMana()
    {
        // 공 튕기는 동안에는 마나 충전 못하게 하기
        int savePlusMana = PlusMana;
        PlusMana = 0;
        yield return new WaitForSeconds(skillCoolDownTime); // skillCoolDownTime을 마나 제한 시간으로 사용
        PlusMana = savePlusMana;
    }

    // 강화
    [SerializeField] Transform UltimateTransform = null;
    void Ultimate()
    {
        for (int i = 0; i < UltimateTransform.childCount; i++)
        {
            GameObject _skill = UsedSkill(UltimateTransform.GetChild(i).position);
            //RPC_Utility.Instance.RPC_Rotation(photonView.ViewID, UltimateTransform.GetChild(i).rotation);
            //_skill.GetComponent<MyPunRPC>().RPC_Rotation(UltimateTransform.GetChild(i).rotation);
        }
    }
}

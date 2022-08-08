using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Multi_GreenMage : Multi_Unit_Mage
{
    public override void SetInherenceData()
    {
        base.SetInherenceData();
        AttackRange *= 2;
    }

    void ShootSkill() => ProjectileShotDelegate.ShotProjectile(skillData, target, OnSkileHit);

    protected override void MageSkile()
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

    IEnumerator Co_FixMana()
    {
        // 공 튕기는 동안에는 마나 충전 못하게 하기
        int savePlusMana = PlusMana;
        PlusMana = 0;
        yield return new WaitForSeconds(skillCoolDownTime); // skillCoolDownTime을 마나 제한 시간으로 사용
        PlusMana = savePlusMana;
    }
}

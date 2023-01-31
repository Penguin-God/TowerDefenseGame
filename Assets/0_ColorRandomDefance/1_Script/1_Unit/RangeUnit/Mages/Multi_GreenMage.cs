using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Multi_GreenMage : Multi_Unit_Mage
{
    [SerializeField] int bounceBallDamage;
    [SerializeField] float manaLockTime;

    protected override void OnAwake()
    {
        base.OnAwake();
        bounceBallDamage = (int)base.skillStats[0];
        manaLockTime = base.skillStats[1];
    }

    void OnSkillHit(Multi_Enemy enemy) => base.SkillAttackToEnemy(enemy, bounceBallDamage);
    void ShootSkill() => ProjectileShotDelegate.ShotProjectile(skillData, target, OnSkillHit);

    protected override void MageSkile()
    {
        StartCoroutine(Co_GreenMageSkile());
        StartCoroutine(Co_FixMana());
    }

    protected override void PlaySkillSound() => PlaySound(EffectSoundType.MageAttack, 0.6f);

    IEnumerator Co_GreenMageSkile()
    {
        nav.isStopped = true;
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.7f);
        ShootSkill();

        yield return new WaitForSeconds(0.5f);
        nav.isStopped = false;
    }

    IEnumerator Co_FixMana() // 공 튕기는 동안에는 마나 충전 못하게 하기
    {
        base.manaSystem.LockMana();
        yield return new WaitForSeconds(manaLockTime); // skillCoolDownTime을 마나 제한 시간으로 사용
        base.manaSystem.ReleaseMana();
    }
}

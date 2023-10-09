using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_GreenMage : Multi_Unit_Mage
{
    [SerializeField] float _damRate;
    [SerializeField] float manaLockTime;
    protected override void OnAwake()
    {
        base.OnAwake();
        _damRate = base.skillStats[0];
        manaLockTime = base.skillStats[1];
    }

    void OnSkillHit(Multi_Enemy enemy) => UnitAttacker.SkillAttack(enemy, CalculateSkillDamage(_damRate));
    void ShootSkill() => ShotEnergyBall(new ResourcesPathBuilder().BuildMageSkillEffectPath(UnitFlags.UnitColor), OnSkillHit); 
    //_energyBallThower.FlatThrow(new ResourcesPathBuilder().BuildMageSkillEffectPath(UnitFlags.UnitColor), target, OnSkillHit);

    protected override void MageSkile()
    {
        StartCoroutine(Co_GreenMageSkile());
        manaSystem.LockManaForDuration(manaLockTime);
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
}

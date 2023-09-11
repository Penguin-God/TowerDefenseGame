using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttacker
{
    Multi_Enemy target;
    protected Multi_UnitPassive passive;
    int Damage;
    int BossDamage;

    Action<Multi_Enemy> OnPassiveHit;
    protected bool TargetIsNormal => target != null && target.enemyType == EnemyType.Normal;
    protected void NormalAttack(Multi_Enemy target) => Attack(target, CalaulateAttack(), false, OnPassiveHit);
    protected void SkillAttackWithPassive(Multi_Enemy target) => Attack(target, CalaulateAttack(), true, OnPassiveHit);
    protected int CalaulateAttack() => TargetIsNormal ? Damage : BossDamage;

    protected void SkillAttack(Multi_Enemy target, int attack) => Attack(target, attack, true, null);
    protected void SkillAttackWithPassive(Multi_Enemy target, int attack) => SkillAttackWithSide(target, attack, OnPassiveHit);
    protected void SkillAttackWithSide(Multi_Enemy target, int attack, Action<Multi_Enemy> sideEffect) => Attack(target, attack, true, sideEffect);

    void Attack(Multi_Enemy target, int attack, bool isSkill, Action<Multi_Enemy> sideEffect)
    {
        target.OnDamage(attack, isSkill);
        sideEffect?.Invoke(target);
    }
}

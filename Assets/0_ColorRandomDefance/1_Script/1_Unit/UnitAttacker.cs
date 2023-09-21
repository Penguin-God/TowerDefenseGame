using System;
using System.Collections;
using System.Collections.Generic;

public class UnitAttacker
{
    readonly Unit _unit;
    public UnitAttacker(Unit unit) => _unit = unit;

    // 스킬일 때는 더 높은 대미지였나?
    public int CalculateDamage(EnemyType type) => type == EnemyType.Normal ? _unit.DamageInfo.ApplyDamage : _unit.DamageInfo.ApplyBossDamage;

    public void NormalAttack(Multi_Enemy target) => Attack(target, false);
    public void SkillAttack(Multi_Enemy target) => Attack(target, true);
    public void SkillAttack(Multi_Enemy target, int damage) => Attack(target, damage, true);
    void Attack(Multi_Enemy target, bool isSkill)
    {
        if (target != null)
            Attack(target, CalculateDamage(target.enemyType), isSkill);
    }
    void Attack(Multi_Enemy target, int damage, bool isSkill)
    {
        if (target == null) return;
        target.OnDamage(damage, isSkill);
        new UnitPassiveCreator(Managers.Data).CreatePassive(_unit.UnitFlags)?.DoUnitPassive(_unit, target);
    }
}

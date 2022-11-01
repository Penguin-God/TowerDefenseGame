using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class UnitRandomSkillSystem
{
    public UnitRandomSkillSystem()
    {

    }

    public UnitRandomSkillSystem(Multi_TeamSoldier unit, float rate)
    {
        unit.OnDamageChanaged += (damage) => SetSkillDamage(damage, rate);
        unit.OnBossDamageChanged += (bossDamage) => SetBossSkillDamage(bossDamage, rate);

        SetSkillDamage(unit.Damage, rate);
        SetBossSkillDamage(unit.BossDamage, rate);
    }

    [SerializeField] int _skillDamage;
    [SerializeField] int _bossSkillDamage;

    void SetSkillDamage(int originDamage, float damageRate) => _skillDamage = Mathf.RoundToInt(originDamage * damageRate);
    void SetBossSkillDamage(int originDamage, float damageRate) => _bossSkillDamage = Mathf.RoundToInt(originDamage * damageRate);

    public int GetApplyDamage(Multi_Enemy enemy) => enemy.enemyType == EnemyType.Normal ? _skillDamage : _bossSkillDamage;

    public bool Attack(Action normalAct, Action skillAct, int rate)
    {
        bool result = rate > Random.Range(0, 101);
        if (result) skillAct?.Invoke();
        else normalAct?.Invoke();
        return result;
    }
}

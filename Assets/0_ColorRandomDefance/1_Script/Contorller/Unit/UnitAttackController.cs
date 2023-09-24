using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitAttackActor
{
    IEnumerator Do(Multi_Enemy target);
    float AttackCoolTime { get; }
}

public class UnitAttackController : MonoBehaviour
{
    Unit _unit;

    void Attack(Multi_Enemy target, bool isSkill, Action<Multi_Enemy> sideEffect = null)
    {
        if (target == null) return;
        target.OnDamage(CalaulateDamage(target), isSkill);
        sideEffect?.Invoke(target);
    }
    int CalaulateDamage(Multi_Enemy target) => target.enemyType == EnemyType.Normal ? _unit.DamageInfo.ApplyDamage : _unit.DamageInfo.ApplyBossDamage;

    public void Attack(IUnitAttackActor unitAction, Multi_Enemy target) => StartCoroutine(Co_AttackTemplate(unitAction, target));
    IEnumerator Co_AttackTemplate(IUnitAttackActor unitAction, Multi_Enemy target)
    {
        // _unit.ChangeState(UnitState.Attack);
        yield return StartCoroutine(unitAction.Do(target));
        StartCoroutine(Co_AttackCoolDown(unitAction.AttackCoolTime));
    }

    IEnumerator Co_AttackCoolDown(float coolTime)
    {
        yield return new WaitForSeconds(coolTime);
        // ReadyAttack();
    }
}

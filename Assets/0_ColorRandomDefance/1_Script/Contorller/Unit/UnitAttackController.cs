using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAttackController : MonoBehaviour
{
    Unit _unit;

    void Attack(Multi_Enemy target, bool isSkill, Action<Multi_Enemy> sideEffect = null)
    {
        if (target == null) return;
        target.OnDamage(CalaulateDamage(target), isSkill);
        sideEffect?.Invoke(target);
    }
    int CalaulateDamage(Multi_Enemy target) => target.enemyType == EnemyType.Normal ? _unit.DamageInfo.ApplyDamage : _unit.DamageInfo.ApplyBossDamage;

    public void Attack() => StartCoroutine(Co_AttackTemplate());
    IEnumerator Co_AttackTemplate()
    {
        _unit.ChangeState(UnitState.Attack);
        yield return StartCoroutine(Co_Attack());
        StartCoroutine(Co_AttackCoolDown(_unit.CoolDown));
    }

    protected abstract IEnumerator Co_Attack();

    public void EndAttack(float coolTime)
    {
        StartCoroutine(Co_AttackCoolDown(coolTime));
    }

    IEnumerator Co_AttackCoolDown(float coolTime)
    {
        yield return new WaitForSeconds(coolTime);
        // ReadyAttack();
    }
}

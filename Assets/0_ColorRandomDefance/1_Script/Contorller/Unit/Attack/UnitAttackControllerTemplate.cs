using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAttackControllerTemplate : MonoBehaviour
{
    Animator _animator;
    protected virtual string AnimationName { get; }
    Multi_TeamSoldier.UnitState _unitState;
    protected Unit _unit;

    protected virtual void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void DependencyInject(Multi_TeamSoldier.UnitState unitState, Unit unit)
    {
        _unitState = unitState;
        _unit = unit;
    }

    float _attackSpeed;
    void DoAnima(float animaSpeed)
    {
        if(_animator == null ) return;
        _animator.speed = animaSpeed;
        _animator.SetTrigger(AnimationName);
    }
    public void DoAttack(float attackSpeed, float coolDownTime)
    {
        DoAnima(attackSpeed);
        _attackSpeed = attackSpeed;
        StartCoroutine(Co_DoAttack(coolDownTime));
    }
    IEnumerator Co_DoAttack(float coolDownTime)
    {
        _unitState.StartAttack();
        yield return StartCoroutine(Co_Attack());
        _unitState.EndAttack(coolDownTime);
    }
    protected abstract IEnumerator Co_Attack();
    protected WaitForSeconds WatiSecond(float second) => new WaitForSeconds(second / _attackSpeed);
}

public class UnitAttackControllerGenerator
{
    T GenerateTemplate<T>(Multi_TeamSoldier unit)
    {
        var result = unit.GetComponent<UnitAttackControllerTemplate>();
        result.DependencyInject(unit._state, unit.Unit);
        return result.GetComponent<T>();
    }

    public SwordmanAttackController GenerateSwordmanAttacker(Multi_TeamSoldier unit)
    {
        var result = GenerateTemplate<SwordmanAttackController>(unit);
        result.RecevieInject(unit);
        return result;
    }

    public ArcherNormalAttackController GenerateArcherAttacker(Multi_TeamSoldier unit, Transform shotPoint)
    {
        var result = GenerateTemplate<ArcherNormalAttackController>(unit);
        result.Inject(shotPoint, unit, unit.UnitAttacker);
        return result;
    }

    public ArcherSpecialAttackController GenerateArcherSkillAttcker(Multi_TeamSoldier unit, ArcherArrowShoter archerArrowShoter)
    {
        var result = GenerateTemplate<ArcherSpecialAttackController>(unit);
        result.Inject(archerArrowShoter, unit, unit.UnitAttacker);
        return result;
    }
}
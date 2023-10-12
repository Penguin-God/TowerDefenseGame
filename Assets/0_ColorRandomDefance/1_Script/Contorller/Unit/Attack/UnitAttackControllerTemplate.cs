using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAttackControllerTemplate : MonoBehaviour
{
    Animator _animator;
    string AnimationName;
    Multi_TeamSoldier.UnitState _unitState;
    protected Unit _unit;
    public void DependencyInject(Animator animator, string animationName, Multi_TeamSoldier.UnitState unitState, Unit unit)
    {
        _animator = animator;
        AnimationName = animationName;
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

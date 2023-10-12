using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAttackControllerTemplate : MonoBehaviour
{
    Animator _animator;
    string AnimationName;
    Multi_TeamSoldier.UnitState _unitState;
    public void DependencyInject(Animator animator, string animationName, Multi_TeamSoldier.UnitState unitState)
    {
        _animator = animator;
        AnimationName = animationName;
        _unitState = unitState;
    }

    float _attackSpeed;
    public void DoAttack(float attackSpeed, float coolDownTime)
    {
        _animator.speed = attackSpeed;
        _animator.SetTrigger(AnimationName);
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

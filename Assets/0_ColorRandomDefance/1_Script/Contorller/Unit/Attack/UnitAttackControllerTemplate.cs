using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAttackControllerTemplate : MonoBehaviour
{
    readonly Animator _animator;
    readonly string AnimationName;
    public UnitAttackControllerTemplate(Animator animator, string animationName)
    {
        _animator = animator;
        AnimationName = animationName;
    }

    float _attackSpeed;
    public void DoAttack(float attackSpeed)
    {
        _animator.speed = attackSpeed;
        _animator.SetTrigger(AnimationName);
        _attackSpeed = attackSpeed;
        StartCoroutine(Co_Attack());
    }
    protected abstract IEnumerator Co_Attack();
    protected WaitForSeconds WatiSecond(float second) => new WaitForSeconds(second / _attackSpeed);
}

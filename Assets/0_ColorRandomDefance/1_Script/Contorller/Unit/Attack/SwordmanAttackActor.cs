using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordmanAttackActor : MonoBehaviour, IUnitAttackActor
{
    [SerializeField] GameObject _trail;
    [SerializeField] Animator animator;
    UnitAttacker _unitAttacker;
    public void RecevieInject(UnitAttacker unitAttacker)
    {
        _unitAttacker = unitAttacker;
    }

    public float AttackCoolTime => 0.5f;
    public IEnumerator Do(Multi_Enemy target)
    {
        animator.SetTrigger("isSword");
        yield return new WaitForSeconds(0.8f);
        _trail.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        _unitAttacker.Attack(target);
        _trail.SetActive(false);
    }
}

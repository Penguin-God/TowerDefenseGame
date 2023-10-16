using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class SpearmanSkillAttackController : UnitAttackControllerTemplate
{
    protected override string AnimationName => "isSpecialAttack";

    NavMeshAgent _nav;
    [SerializeField] GameObject _spear;
    [SerializeField] Transform _shotPoint;
    protected override void Awake()
    {
        base.Awake();
        _nav = GetComponent<NavMeshAgent>();
    }

    SpearShoter _spearShoter;
    Action<Multi_Enemy> _attack;
    public void Inject(SpearShoter spearShoter, Action<Multi_Enemy> attack)
    {
        _spearShoter = spearShoter;
        _attack = attack;
    }

    protected override IEnumerator Co_Attack()
    {
        yield return StartCoroutine(_spearShoter.Co_ShotSpear(transform, _shotPoint, _attack));

        _spear.SetActive(false);
        _nav.isStopped = true;
        // PlaySound(EffectSoundType.SpearmanSkill);

        yield return WaitSecond(0.5f);
        _nav.isStopped = false;
        _spear.SetActive(true);
    }
}

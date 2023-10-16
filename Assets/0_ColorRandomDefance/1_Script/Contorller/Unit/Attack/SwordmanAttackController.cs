using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordmanAttackController : UnitAttackControllerTemplate
{
    protected override string AnimationName => "isSword";
    [SerializeField] GameObject _trail;
    Multi_TeamSoldier _unitController;
    public void RecevieInject(Multi_TeamSoldier unit)
    {
        _unitController = unit;
    }

    protected override IEnumerator Co_Attack()
    {
        yield return WaitSecond(0.8f);
        _trail.SetActive(true);
        yield return WaitSecond(0.3f);
        _unitController._NormalAttack();
        _trail.SetActive(false);
    }
}
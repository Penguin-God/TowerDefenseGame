using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordmanAttackController : UnitAttackControllerTemplate
{
    [SerializeField] GameObject _trail;
    Multi_TeamSoldier _unit;
    public void RecevieInject(Multi_TeamSoldier unit)
    {
        _unit = unit;
    }

    protected override IEnumerator Co_Attack()
    {
        yield return WatiSecond(0.8f);
        _trail.SetActive(true);
        yield return WatiSecond(0.3f);
        _unit._NormalAttack();
        _trail.SetActive(false);
    }
}

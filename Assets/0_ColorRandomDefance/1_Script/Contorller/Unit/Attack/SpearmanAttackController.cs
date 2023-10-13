using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmanAttackController : UnitAttackControllerTemplate
{
    protected override string AnimationName => "isAttack";
    Multi_TeamSoldier _unitController;
    public void Inject(Multi_TeamSoldier unit) => _unitController = unit;

    [SerializeField] GameObject _trail;
    protected override IEnumerator Co_Attack()
    {
        yield return new WaitForSeconds(0.55f);
        _trail.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        _unitController._NormalAttack();
        yield return new WaitForSeconds(0.3f);
        _trail.SetActive(false);
    }
}

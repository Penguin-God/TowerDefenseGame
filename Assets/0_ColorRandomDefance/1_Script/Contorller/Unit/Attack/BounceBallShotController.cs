using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBallShotController : MageAttackerController
{
    float _manaLockTime;
    public void SetSkillData(float manaLockTime)
    {
        _manaLockTime = manaLockTime;
    }

    protected override IEnumerator Co_Attack()
    {
        _manaSystem.LockManaForDuration(_manaLockTime);
        yield return StartCoroutine(base.Co_Attack());
    }
}

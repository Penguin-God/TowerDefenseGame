using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackController : MonoBehaviour
{
    MonsterAttacker attacker = new MonsterAttacker();

    public void Attack()
    {

    }

    protected void StartAttack()
    {
        // AfterPlaySound(normalAttackSound, normalAttakc_AudioDelay);
    }

    public void EndAttack(float coolTime)
    {
        // _unitAttackState = _unitAttackState.AttackDone();
        StartCoroutine(Co_AttackCoolDown(coolTime));
    }

    IEnumerator Co_AttackCoolDown(float coolTime)
    {
        yield return new WaitForSeconds(coolTime);
        // ReadyAttack();
    }

    protected void EndSkillAttack(float coolTime)
    {

    }
}

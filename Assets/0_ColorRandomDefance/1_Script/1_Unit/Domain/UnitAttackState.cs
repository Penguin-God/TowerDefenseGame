using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UnitAttackState
{
    public bool IsAttackable { get; private set; }
    public bool IsAttack { get; private set; }

    public UnitAttackState(bool isAttackable, bool isAttack)
    {
        IsAttackable = isAttackable;
        IsAttack = isAttack;
    }

    public UnitAttackState DoAttack() => new UnitAttackState(false, true);
    public UnitAttackState AttackDone() => new UnitAttackState(false, false); // ��ٿ��� �־ ������ ������ IsAttackable�� false
    public UnitAttackState ReadyAttack() => new UnitAttackState(true, false);
}

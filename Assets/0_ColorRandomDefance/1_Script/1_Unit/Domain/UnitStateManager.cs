using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateManager
{
    public bool IsAttack { get; private set; }
    public bool IsAttackable;
    public bool EnterStoryWorld;

    public void ChangedWorld()
    {
        IsAttackable = true;
        IsAttack = false;
        EnterStoryWorld = !EnterStoryWorld;
    }

    public void DoAttack()
    {
        IsAttackable = false;
        IsAttack = true;
    }

    public void EndAttack() => IsAttack = false;

    public void Dead()
    {
        IsAttackable = true;
        IsAttack = false;
    }
}

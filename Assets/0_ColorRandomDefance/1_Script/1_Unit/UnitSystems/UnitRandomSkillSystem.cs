using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class UnitRandomSkillSystem
{
    public bool Attack(Action normalAct, Action skillAct, int rate)
    {
        bool result = rate > Random.Range(0, 101);
        if (result) skillAct?.Invoke();
        else normalAct?.Invoke();
        return result;
    }
}

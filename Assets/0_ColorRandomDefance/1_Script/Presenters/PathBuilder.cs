using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathBuilder
{
    public string BuildUnitPath(UnitClass unitClass) => $"Unit/{Enum.GetName(typeof(UnitClass), unitClass)}";

    static Dictionary<int, string> _numberByName = new Dictionary<int, string>()
    {
        {3, "Swordman" },
        {0, "Archer" },
        {2, "Spearman" },
        {1, "Mage" },
    };
    public string BuildMonsterPath(int monsterNumber) => $"Enemy/{_numberByName[monsterNumber]}";
}

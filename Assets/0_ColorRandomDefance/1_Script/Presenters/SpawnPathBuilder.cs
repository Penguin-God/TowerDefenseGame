using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnPathBuilder
{
    public string BuildUnitPath(UnitFlags flag)
        => $"Unit/{Enum.GetName(typeof(UnitClass), flag.UnitClass)}/{Enum.GetName(typeof(UnitColor), flag.UnitColor)}_{Enum.GetName(typeof(UnitClass), flag.UnitClass)} 1";

    static Dictionary<int, string> _numberByName = new Dictionary<int, string>()
    {
        {3, "Swordman" },
        {0, "Archer" },
        {2, "Spearman" },
        {1, "Mage" },
    };
    public string BuildMonsterPath(int monsterNumber) => $"Enemy/Normal/Enemy_{_numberByName[monsterNumber]} 1";
    public string BuildBossMonsterPath(int monsterNumber) => $"Enemy/Boss/Boss_Enemy_{_numberByName[monsterNumber]} 1";
    public string BuildEnemyTowerPath(int towerLevel) => $"Enemy/Tower/Lvl{towerLevel}_Twoer";
}

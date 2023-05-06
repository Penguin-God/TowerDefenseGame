using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusPersenter
{
    public string BuildUnitCountText(int currentUnit, int maxUnit) => $"{currentUnit}/{maxUnit}";
    public string BuildMonsterCountText(int currentMonster, int maxMonster) => $"{currentMonster}/{maxMonster}";
}

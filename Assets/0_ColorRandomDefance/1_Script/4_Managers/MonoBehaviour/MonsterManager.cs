using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : IMonsterManager
{
    List<Multi_NormalEnemy> _normalMonsters = new List<Multi_NormalEnemy>();
    public void AddNormalMonster(Multi_NormalEnemy multi_NormalEnemy) => _normalMonsters.Add(multi_NormalEnemy);
    public void RemoveNormalMonster(Multi_NormalEnemy multi_NormalEnemy) => _normalMonsters.Remove(multi_NormalEnemy);
    public IReadOnlyList<Multi_NormalEnemy> GetNormalMonsters() => _normalMonsters;
}

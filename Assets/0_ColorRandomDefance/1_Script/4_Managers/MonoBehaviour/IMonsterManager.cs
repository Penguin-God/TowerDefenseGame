using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterManager
{
    public void RegisterMonsterCountChange(Action<int> OnCountChange);
    public void AddNormalMonster(Multi_NormalEnemy multi_NormalEnemy);
    public IReadOnlyList<Multi_NormalEnemy> GetNormalMonsters();
}

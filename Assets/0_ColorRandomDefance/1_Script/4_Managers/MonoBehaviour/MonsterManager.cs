using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : IMonsterManager
{
    List<Multi_NormalEnemy> _normalMonsters = new List<Multi_NormalEnemy>();
    public event Action<int> OnNormalMonsterCountChanged = null;
    void NotifyNormalCountChange() => OnNormalMonsterCountChanged?.Invoke(_normalMonsters.Count);

    public void AddNormalMonster(Multi_NormalEnemy multi_NormalEnemy)
    {
        _normalMonsters.Add(multi_NormalEnemy);
        NotifyNormalCountChange();
    }
    public void RemoveMonster(Multi_NormalEnemy multi_NormalEnemy)
    {
        _normalMonsters.Remove(multi_NormalEnemy);
        NotifyNormalCountChange();
    }

    public IReadOnlyList<Multi_NormalEnemy> GetNormalMonsters() => _normalMonsters;
    public void RegisterMonsterCountChange(Action<int> OnCountChange) => OnNormalMonsterCountChanged += OnCountChange;
}

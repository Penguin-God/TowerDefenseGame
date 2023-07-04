using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventDispatcher
{
    public event Action<int> OnMonsterCountChanged;
    public event Action<int> OnOpponentMonsterCountChange;

    public void NotifyMonsterCountChange(byte playerId, int count)
    {
        if(playerId == PlayerIdManager.Id)
            OnMonsterCountChanged?.Invoke(count);
        else
            OnOpponentMonsterCountChange?.Invoke(count);
    }

    public event Action<Multi_NormalEnemy> OnNormalMonsterDead;
    public void NotifyMonsterDead(Multi_NormalEnemy monster) => OnNormalMonsterDead?.Invoke(monster);

    public event Action<int> OnOpponentUnitCountChanged = null;
    public event Action<UnitClass, int> OnOpponentUnitCountChangedByClass;
    public event Action<int> OnOpponentUnitMaxCountChanged = null;

    public void NotifyOpponentUnitCountChanged(int count, UnitClass unitClass, int classCount)
    {
        OnOpponentUnitCountChanged?.Invoke(count);
        OnOpponentUnitCountChangedByClass?.Invoke(unitClass, classCount);
    }

    public void NotifyOpponentUnitMaxCountChanged(int maxCount) => OnOpponentUnitMaxCountChanged?.Invoke(maxCount);

    public event Action OnGameStart = null;
    public void NotifyGameStart() => OnGameStart?.Invoke();

    public event Action<int> OnStageUp = null;
    public void NotifyStageUp(int stage) => OnStageUp?.Invoke(stage);
}

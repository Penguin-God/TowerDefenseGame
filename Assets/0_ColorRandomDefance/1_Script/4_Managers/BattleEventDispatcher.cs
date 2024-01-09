using System;
using System.Collections.Generic;
using System.Linq;

public class BattleEventDispatcher
{
    public event Action<int> OnMonsterCountChanged;
    public event Action<int> OnOpponentMonsterCountChange;
    public event Action<int, int> OnAnyMonsterCountChanged;

    public void NotifyAnyMonsterCountChange(int masterCount, int clientCount) => OnAnyMonsterCountChanged?.Invoke(masterCount, clientCount);
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
    public event Action<int> OnStageUpExcludingFirst = null;
    public void NotifyStageUp(int stage)
    {
        if(stage > 1)
            OnStageUpExcludingFirst?.Invoke(stage);
        OnStageUp?.Invoke(stage);
    }

    public event Action<int> OnUnitCountChange = null;
    public event Action<UnitFlags, int> OnUnitCountChangeByFlag = null;
    public event Action<UnitClass, int> OnUnitCountChangeByClass = null;
    public void NotifyUnitListChange(UnitFlags changeFlag, IEnumerable<UnitFlags> flags)
    {
        OnUnitCountChange?.Invoke(flags.Count());
        OnUnitCountChangeByFlag?.Invoke(changeFlag, flags.Where(x => x == changeFlag).Count());
        OnUnitCountChangeByClass?.Invoke(changeFlag.UnitClass, flags.Where(x => x.UnitClass == changeFlag.UnitClass).Count());
    }

    public event Action<Multi_TeamSoldier> OnUnitSpawn;
    public void NotifyUnitSpawn(Multi_TeamSoldier unit) => OnUnitSpawn?.Invoke(unit);

    public event Action<UnitFlags> OnUnitCombine = null;
    public void NotifyUnitCombine(UnitFlags flag) => OnUnitCombine?.Invoke(flag);

    public event Action<int> OnMaxUnitCountChange = null;
    public void NotifyMaxUnitCountChange(int count) => OnMaxUnitCountChange?.Invoke(count);
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventDispatcher
{
    public event Action<int> OnMonsterCountChanged;
    public event Action<int> OnOppentMonsterCountChange;
    
    public void NotifyMonsterCountChange(byte playerId, int count)
    {
        if(playerId == PlayerIdManager.Id)
            OnMonsterCountChanged?.Invoke(count);
        else
            OnOppentMonsterCountChange?.Invoke(count);
    }
}

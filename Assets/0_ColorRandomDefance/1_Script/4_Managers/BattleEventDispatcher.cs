using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEventDispatcher
{
    public event Action<int> OnMonsterCountChanged;
    public void NotifyMonsterCountChange(int count) => OnMonsterCountChanged?.Invoke(count);


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromencer
{
    readonly int NeedKillCountForSummon;
    int _currentKillCount;
    public Necromencer(int needKillCount) => NeedKillCountForSummon = needKillCount;

    public bool TryResurrect()
    {
        _currentKillCount++;
        if (_currentKillCount >= NeedKillCountForSummon)
        {
            _currentKillCount = 0;
            return true;
        }
        else
            return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromencer
{
    public readonly int NeedKillCountForSummon;
    public int CurrentKillCount { get; private set; }
    public Necromencer(int needKillCount) => NeedKillCountForSummon = needKillCount;

    public bool TryResurrect()
    {
        CurrentKillCount++;
        if (CurrentKillCount >= NeedKillCountForSummon)
        {
            CurrentKillCount = 0;
            return true;
        }
        else
            return false;
    }
}

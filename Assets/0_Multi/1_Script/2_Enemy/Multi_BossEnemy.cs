using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class Multi_BossEnemy : Multi_NormalEnemy
{
    [SerializeField] int _level;
    public int Level => _level;

    public BossData BossData { get; private set; }
    public void Spawn(int level)
    {
        _level = level;
        BossData = Multi_Managers.Data.BossDataByLevel[_level];
        SetStatus_RPC(BossData.Hp, BossData.Speed, false);
    }
}

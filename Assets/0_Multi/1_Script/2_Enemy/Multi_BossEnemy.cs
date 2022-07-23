using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class Multi_BossEnemy : Multi_NormalEnemy
{
    [SerializeField] int _level;
    public int Level => _level;

    public void Spawn(int hp, int speed, int level)
    {
        SetStatus_RPC(hp, speed, false);
        _level = level;
    }
}

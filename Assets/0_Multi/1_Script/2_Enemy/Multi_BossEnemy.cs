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
        photonView.RPC("SetBossStatus", RpcTarget.All, level);
        SetStatus_RPC(BossData.Hp, BossData.Speed, false);
    }

    [PunRPC]
    void SetBossStatus(int level)
    {
        _level = level;
        BossData = Multi_Managers.Data.BossDataByLevel[_level];
        OnDeath += () => Multi_SpawnManagers.BossEnemy.OnDead?.Invoke(this); // 임시코드
    }
}

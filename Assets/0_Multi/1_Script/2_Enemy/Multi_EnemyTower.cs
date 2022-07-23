using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_EnemyTower : Multi_Enemy
{
    int _level;
    public int Level => _level;
    public BossData TowerData { get; private set; }

    public void Spawn(int level)
    {
        _level = level;
        TowerData = Multi_Managers.Data.TowerDataByLevel[_level];
        SetStatus_RPC(TowerData.Hp, TowerData.Speed, false);
    }

    public override void Dead()
    {
        base.Dead();

        if (PhotonNetwork.IsMasterClient)
            Multi_Managers.Resources.PhotonDestroy(gameObject);
    }

    [ContextMenu("죽음")]
    void 죽음() => Dead();
}

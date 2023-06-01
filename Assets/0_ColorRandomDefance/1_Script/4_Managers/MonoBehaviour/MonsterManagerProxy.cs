using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class MonsterManagerProxy : MonoBehaviourPun, IMonsterManager
{
    MultiMonsterManager _multiMonsterManager = new MultiMonsterManager();
    Action<int> OnNormalMonsterCountChange = null;

    public void AddNormalMonster(Multi_NormalEnemy monster) => photonView.RPC(nameof(AddNormalMonster), RpcTarget.MasterClient, monster.GetComponent<PhotonView>().ViewID);
    public void RegisterMonsterCountChange(Action<int> OnCountChange) => OnNormalMonsterCountChange += OnCountChange;

    [PunRPC]
    void AddNormalMonster(int viewId)
    {
        var monster = Managers.Multi.GetPhotonViewComponent<Multi_NormalEnemy>(viewId);
        _multiMonsterManager.AddNormalMonster(monster);
        byte newCount = (byte)_multiMonsterManager.GetNormalMonsters(monster.UsingId).Count;
        if (monster.UsingId == PlayerIdManager.MasterId)
            NotifyNormalMonsterCountChange(newCount);
        else
            photonView.RPC(nameof(NotifyNormalMonsterCountChange), RpcTarget.Others, newCount);
    }

    [PunRPC] void NotifyNormalMonsterCountChange(byte count) => OnNormalMonsterCountChange?.Invoke(count);
}

public class MultiMonsterManager
{
    MultiData<MonsterManager> _mulitMonsterManager;
    public MultiMonsterManager() => _mulitMonsterManager = MultiDataFactory.CreateMultiData<MonsterManager>();

    public MonsterManager GetMultiData(byte id) => _mulitMonsterManager.GetData(id);
    public void AddNormalMonster(Multi_NormalEnemy monster) => GetMultiData(monster.UsingId).AddNormalMonster(monster);
    public IReadOnlyList<Multi_NormalEnemy> GetNormalMonsters(byte id) => GetMultiData(id).GetNormalMonsters();
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class MonsterManagerProxy : MonoBehaviourPun, IMonsterManager
{
    readonly ServerMonsterManager _multiMonsterManager = new ServerMonsterManager();
    public ServerMonsterManager MultiMonsterManager => _multiMonsterManager;

    readonly MonsterManager _monsterManager = new MonsterManager();
    public IReadOnlyList<Multi_NormalEnemy> GetNormalMonsters()
    {
        if(PhotonNetwork.IsMasterClient)
            return _multiMonsterManager.GetMultiData(PlayerIdManager.MasterId).GetNormalMonsters();
        else
            return _monsterManager.GetNormalMonsters();
    }

    BattleEventDispatcher _eventDispatcher = null;
    public void Init(BattleEventDispatcher eventDispatcher)
    {
        _eventDispatcher = eventDispatcher;
    }

    // 지금은 마스터에서만 접근함
    public void AddNormalMonster(Multi_NormalEnemy monster) => ChangeMonsterList(monster, _multiMonsterManager.AddNormalMonster);
    public void RemoveNormalMonster(Multi_NormalEnemy monster) => ChangeMonsterList(monster, _multiMonsterManager.RemoveNormalMonster);

    void ChangeMonsterList(Multi_NormalEnemy monster, Action<Multi_NormalEnemy> changeMonsterList)
    {
        byte previousCount = (byte)_multiMonsterManager.GetNormalMonsters(monster.UsingId).Count;
        changeMonsterList?.Invoke(monster);

        byte newCount = (byte)_multiMonsterManager.GetNormalMonsters(monster.UsingId).Count;
        photonView.RPC(nameof(NotifyNormalMonsterCountChange), RpcTarget.All, monster.UsingId, newCount);

        if (monster.UsingId != PlayerIdManager.ClientId) return;
        if(newCount > previousCount)
            photonView.RPC(nameof(RPC_AddNormalMonster), RpcTarget.All, monster.GetComponent<PhotonView>().ViewID);
        else
            photonView.RPC(nameof(RPC_RemoveNormalMonster), RpcTarget.All, monster.GetComponent<PhotonView>().ViewID);
    }

    [PunRPC] void NotifyNormalMonsterCountChange(byte playerId, byte count) => _eventDispatcher.NotifyMonsterCountChange(playerId, count);
    [PunRPC] void RPC_AddNormalMonster(int viewId) => _monsterManager.AddNormalMonster(Managers.Multi.GetPhotonViewComponent<Multi_NormalEnemy>(viewId));
    [PunRPC] void RPC_RemoveNormalMonster(int viewId) => _monsterManager.RemoveNormalMonster(Managers.Multi.GetPhotonViewComponent<Multi_NormalEnemy>(viewId));
}

public class ServerMonsterManager
{
    MultiData<MonsterManager> _mulitMonsterManager;
    public ServerMonsterManager() => _mulitMonsterManager = MultiDataFactory.CreateMultiData<MonsterManager>();

    public MonsterManager GetMultiData(byte id) => _mulitMonsterManager.GetData(id);
    public void AddNormalMonster(Multi_NormalEnemy monster) => GetMultiData(monster.UsingId).AddNormalMonster(monster);
    public void RemoveNormalMonster(Multi_NormalEnemy monster) => GetMultiData(monster.UsingId).RemoveNormalMonster(monster);
    public IReadOnlyList<Multi_NormalEnemy> GetNormalMonsters(byte id) => GetMultiData(id).GetNormalMonsters();
}
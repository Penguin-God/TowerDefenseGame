using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class MonsterManagerProxy : MonoBehaviourPun, IMonsterManager
{
    MultiMonsterManager _multiMonsterManager = new MultiMonsterManager();
    BattleEventDispatcher _eventDispatcher = null;
    public void Init(BattleEventDispatcher eventDispatcher)
    {
        _eventDispatcher = eventDispatcher;
    }

    public void AddNormalMonster(Multi_NormalEnemy monster) => AddNormalMonster(monster.GetComponent<PhotonView>().ViewID);
    public void RemoveNormalMonster(Multi_NormalEnemy monster) => RemoveNormalMonster(monster.GetComponent<PhotonView>().ViewID);

    void AddNormalMonster(int viewId) => ChangeMonsterList(viewId, _multiMonsterManager.AddNormalMonster);
    void RemoveNormalMonster(int viewId) => ChangeMonsterList(viewId, _multiMonsterManager.RemoveNormalMonster);

    void ChangeMonsterList(int viewId, Action<Multi_NormalEnemy> changeMonsterList)
    {
        var monster = Managers.Multi.GetPhotonViewComponent<Multi_NormalEnemy>(viewId);
        changeMonsterList?.Invoke(monster);

        byte newCount = (byte)_multiMonsterManager.GetNormalMonsters(monster.UsingId).Count;
        if (monster.UsingId == PlayerIdManager.MasterId)
            NotifyNormalMonsterCountChange(newCount);
        else
            photonView.RPC(nameof(NotifyNormalMonsterCountChange), RpcTarget.Others, newCount);
    }

    [PunRPC] void NotifyNormalMonsterCountChange(byte count) => _eventDispatcher.NotifyMonsterCountChange(count);
}

public class MultiMonsterManager
{
    MultiData<MonsterManager> _mulitMonsterManager;
    public MultiMonsterManager() => _mulitMonsterManager = MultiDataFactory.CreateMultiData<MonsterManager>();

    public MonsterManager GetMultiData(byte id) => _mulitMonsterManager.GetData(id);
    public void AddNormalMonster(Multi_NormalEnemy monster) => GetMultiData(monster.UsingId).AddNormalMonster(monster);
    public void RemoveNormalMonster(Multi_NormalEnemy monster) => GetMultiData(monster.UsingId).RemoveNormalMonster(monster);
    public IReadOnlyList<Multi_NormalEnemy> GetNormalMonsters(byte id) => GetMultiData(id).GetNormalMonsters();
}
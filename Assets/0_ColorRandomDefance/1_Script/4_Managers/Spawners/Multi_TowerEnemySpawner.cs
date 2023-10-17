using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class Multi_TowerEnemySpawner : Multi_SpawnerBase
{
    public event Action<Multi_EnemyTower> OnDead;

    public RPCAction rpcOnDead = new RPCAction();
    RPCData<int> _towerLevel = new RPCData<int>();

    protected override void SetSpawnObj(GameObject go)
    {
        var enemy = go.GetComponent<Multi_EnemyTower>();
        enemy.OnDeath += () => OnDead(enemy);
        enemy.OnDeath += () => rpcOnDead.RaiseEvent(enemy.UsingId);
    }

    void AfterSpawn(Multi_EnemyTower tower)
    {
        if (Resources.Load<GameObject>($"Prefabs/{PathBuilder.BuildEnemyTowerPath(tower.Level + 1)}") != null)
            StartCoroutine(Co_AfterSpawn(tower.GetComponent<RPCable>().UsingId));
    }
    IEnumerator Co_AfterSpawn(int id)
    {
        yield return new WaitForSeconds(3f);
        Spawn(id);
    }

    public void Spawn(int id) => Spawn_RPC(PathBuilder.BuildEnemyTowerPath(_towerLevel.Get(id) + 1), MultiData.instance.EnemyTowerWorldPositions[id], id);

    [PunRPC]
    protected override GameObject BaseSpawn(string path, Vector3 spawnPos, Quaternion rotation, int id)
    {
        Multi_EnemyTower tower = base.BaseSpawn(path, spawnPos, rotation, id).GetComponent<Multi_EnemyTower>();
        _towerLevel.Set(id, _towerLevel.Get(id) + 1);
        tower.Setinfo(_towerLevel.Get(id));
        tower.OnDead += died => AfterSpawn(tower);
        SetSpawnObj(tower.gameObject);
        photonView.RPC(nameof(SetAA), RpcTarget.All, tower.GetComponent<PhotonView>().ViewID);
        return null;
    }

    [PunRPC] 
    void SetAA(int id)
    {
        var tower = Managers.Multi.GetPhotonViewComponent<Multi_EnemyTower>(id);
        Multi_EnemyManager.Instance.SetSpawnTower(tower.UsingId, tower);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Multi_NormalEnemySpawner : Multi_EnemySpawnerBase
{
    public event Action<Multi_NormalEnemy> OnSpawn;
    public event Action<Multi_NormalEnemy> OnDead;
    EnemySpawnNumManager _numManager;
    public void SetInfo(EnemySpawnNumManager numManager)
    {
        _numManager = numManager;
    }

    string GetCurrentEnemyPath(int enemyNum) => BuildPath(_rootPath, _enemys[enemyNum]);
    protected override void MasterInit() => CreatePool();

    void CreatePool()
    {
        foreach (var enemyPrefab in _enemys)
            CreatePoolGroup(enemyPrefab, BuildPath(_rootPath, enemyPrefab), spawnCount);
    }

    [SerializeField] bool isTest = false;
    public void Spawn(byte enemyNum, int spawnPlayerID) => SpawnEnemy_RPC(enemyNum, (spawnPlayerID == 0) ? 1 : 0);

    void EenmySpawnToOtherWorld(Multi_NormalEnemy enemy)
    {
        if (enemy.resurrection.IsResurrection) return;

        int id = enemy.UsingId == 0 ? 1 : 0;
        SpawnEnemy_RPC(_numManager.GetSpawnEnemyNum(enemy.UsingId), id);
    }

    void SpawnEnemy_RPC(byte num, int id) => pv.RPC(nameof(SpawnEnemy), RpcTarget.MasterClient, num, id);

    [PunRPC]
    Multi_NormalEnemy SpawnEnemy(byte num, int id)
    {
        var enemy = base.BaseSpawn(GetCurrentEnemyPath(num), spawnPositions[id], Quaternion.identity, id).GetComponent<Multi_NormalEnemy>();
        NormalEnemyData data = Managers.Data.NormalEnemyDataByStage[Multi_StageManager.Instance.CurrentStage];
        enemy.SetStatus_RPC(data.Hp, data.Speed, false);
        enemy.resurrection.SetSpawnStage(Multi_StageManager.Instance.CurrentStage);
        OnSpawn?.Invoke(enemy);
        return enemy;
    }

    protected override void SetPoolObj(GameObject go)
    {
        var enemy = go.GetComponent<Multi_NormalEnemy>();
        enemy.enemyType = EnemyType.Normal;

        if (PhotonNetwork.IsMasterClient == false) return;
        enemy.OnDeath += () => OnDead(enemy);
        enemy.OnDeath += () => EenmySpawnToOtherWorld(enemy);
        enemy.OnDeath += () => Managers.Multi.Instantiater.PhotonDestroy(enemy.gameObject);
    }

    public void EditorSpawn(byte enemyNum, int spawnWorldID) => SpawnEnemy_RPC(enemyNum, spawnWorldID); // 에디터용
}

public class EnemySpawnNumManager : MonoBehaviourPun
{
    byte[] _spawnEnemyNums = new byte[2];

    public byte GetSpawnEnemyNum(int id) => _spawnEnemyNums[id];
    public void SetSpawnNumber(byte num)
    {
        if (PhotonNetwork.IsMasterClient)
            _spawnEnemyNums[0] = num;
        else
            photonView.RPC(nameof(SetClientSpawnNumber), RpcTarget.MasterClient, num);
    }
    [PunRPC]
    void SetClientSpawnNumber(byte num) => _spawnEnemyNums[1] = num;
}

public class StageMonsterSpawner : MonoBehaviour
{
    EnemySpawnNumManager _numManager;
    public void SetInfo(EnemySpawnNumManager numManager)
    {
        _numManager = numManager;
    }

    public void StageSpawn(int stage)
    {
        if (stage % 10 == 0) return;
        StartCoroutine(Co_StageSpawn());
    }

    [SerializeField] float _spawnDelayTime = 2f;
    [SerializeField] int _stageSpawnCount = 15;
    IEnumerator Co_StageSpawn()
    {
        byte num = _numManager.GetSpawnEnemyNum(Multi_Data.instance.Id);
        for (int i = 0; i < _stageSpawnCount; i++)
        {
            Multi_SpawnManagers.NormalEnemy.Spawn(num, Multi_Data.instance.Id);
            yield return new WaitForSeconds(_spawnDelayTime);
        }
    }
}

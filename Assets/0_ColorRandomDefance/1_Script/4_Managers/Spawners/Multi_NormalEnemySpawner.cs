using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Multi_NormalEnemySpawner : Multi_EnemySpawnerBase
{
    public event Action<Multi_NormalEnemy> OnDead;

    protected override void MasterInit() => CreatePool();

    void CreatePool()
    {
        for (int i = 0; i < 4; i++)
            CreatePoolGroup(new SpawnPathBuilder().BuildMonsterPath(i), spawnCount);
    }

    public void Spawn(byte enemyNum, int spawnPlayerID) => SpawnEnemy_RPC(enemyNum, spawnPlayerID);

    void SpawnEnemy_RPC(byte num, int id) => photonView.RPC(nameof(SpawnEnemy), RpcTarget.MasterClient, num, id);

    [PunRPC]
    Multi_NormalEnemy SpawnEnemy(byte num, int id)
    {
        var enemy = base.BaseSpawn(new SpawnPathBuilder().BuildMonsterPath(num), spawnPositions[id], Quaternion.identity, id).GetComponent<Multi_NormalEnemy>();
        NormalEnemyData data = Managers.Data.NormalEnemyDataByStage[Multi_StageManager.Instance.CurrentStage];
        enemy.SetStatus_RPC(data.Hp, data.Speed, false);
        enemy.resurrection.SetSpawnStage(Multi_StageManager.Instance.CurrentStage);
        OnSpawn?.Invoke(enemy);
        return enemy;
    }

    protected override void SetPoolObj(GameObject go)
    {
        var enemy = go.GetComponent<Multi_NormalEnemy>();
        if (PhotonNetwork.IsMasterClient == false) return;
        enemy.OnDeath += () => OnDead(enemy);
    }

    public void EditorSpawn(byte enemyNum, int spawnWorldID) => SpawnEnemy_RPC(enemyNum, spawnWorldID); // 에디터용
}

// 이게 실시간으로 바뀌면 어쩔 수 없는데, 스테이지 들어간 후에 낙장불입이면 그냥 스테이지 시작할 때만 RPC 보내는게 좋을 듯.
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

public class MonsterSpawnerContorller : MonoBehaviour
{
    EnemySpawnNumManager _numManager;

    void Start()
    {
        _numManager = gameObject.GetOrAddComponent<EnemySpawnNumManager>();
        if (PhotonNetwork.IsMasterClient == false) return;
        Multi_SpawnManagers.NormalEnemy.OnDead += RespawnMonsterToOther;
        Multi_StageManager.Instance.OnUpdateStage += SpawnMonsterOnStageChange;
        Multi_StageManager.Instance.OnUpdateStage += SpawnBossOnStageMultipleOfTen;
        Multi_GameManager.instance.OnGameStart += SpawnTowerOnStart;
    }

    void SpawnMonsterOnStageChange(int stage)
    {
        if (IsBossStage(stage)) return;
        StartCoroutine(Co_StageSpawn(0));
        StartCoroutine(Co_StageSpawn(1));
    }

    void SpawnMonsterToOther(int id, byte monsterNumber)
    => Multi_SpawnManagers.NormalEnemy.Spawn(monsterNumber, id == 0 ? 1 : 0);

    void SpawnMonsterToOther(int id)
        => SpawnMonsterToOther(id, _numManager.GetSpawnEnemyNum(id));

    [SerializeField] float _spawnDelayTime = 2f;
    [SerializeField] int _stageSpawnCount = 15;
    IEnumerator Co_StageSpawn(byte id)
    {
        byte num = _numManager.GetSpawnEnemyNum(id);
        for (int i = 0; i < _stageSpawnCount; i++)
        {
            SpawnMonsterToOther(id, num); // num 인라인 안 한 이유는 스테이지 한 번 들어가면 못 바꾸게 할려고
            yield return new WaitForSeconds(_spawnDelayTime);
        }
    }

    void RespawnMonsterToOther(Multi_NormalEnemy enemy)
    {
        if (enemy.resurrection.IsResurrection) return;
        SpawnMonsterToOther(enemy.UsingId);
    }

    void SpawnBossOnStageMultipleOfTen(int stage)
    {
        if (IsBossStage(stage) == false) return;
        Multi_SpawnManagers.BossEnemy.Spawn(0);
        Multi_SpawnManagers.BossEnemy.Spawn(1);
    }

    void SpawnTowerOnStart()
    {
        Multi_SpawnManagers.TowerEnemy.Spawn(0);
        Multi_SpawnManagers.TowerEnemy.Spawn(1);
    }

    bool IsBossStage(int stage) => stage % 10 == 0;
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NormalMonsterSpawner : MonoBehaviourPun
{
    SpeedManagerCreater _speedManagerCreater;
    public NormalMonsterSpawner(SpeedManagerCreater speedManagerCreater) => _speedManagerCreater = speedManagerCreater;

    MonsterDecorator _monsterDecorator;
    public void Inject(MonsterDecorator monsterDecorator) => _monsterDecorator = monsterDecorator;

    public Multi_NormalEnemy SpawnMonster(byte num, byte id, int stage)
    {
        var monster = Managers.Multi.Instantiater.PhotonInstantiateInactive(new ResourcesPathBuilder().BuildMonsterPath(num), id).GetComponent<Multi_NormalEnemy>();
        NormalEnemyData data = Managers.Data.NormalEnemyDataByStage[stage];
        photonView.RPC(nameof(InjectMonster), RpcTarget.All, data.Hp, data.Speed, monster.GetComponent<PhotonView>().ViewID);
        // monster.Inject(_speedManagerCreater.CreateMonsterSpeedManager(data.Speed, monster), data.Hp);
        return monster;
    }

    [PunRPC]
    void InjectMonster(int hp, float speed, int viewId)
    {
        var monster = Managers.Multi.GetPhotonViewComponent<Multi_NormalEnemy>(viewId);
        _monsterDecorator.DecorateSpeed(speed, monster);
        monster.Inject(hp);
    }
}

public class MonsterDecorator
{
    readonly BattleDIContainer _container;
    public MonsterDecorator(BattleDIContainer container) => _container = container;

    public void DecorateSpeed(float speed, Multi_NormalEnemy monster)
    {
        var skillData = _container.GetMultiActiveSkillData().GetData(monster.UsingId);
        //if (skillData.TruGetSkillData(SkillType.썬콜, out var skillBattleData))
        //    return new SuncoldSpeedManager(speed, monster, skillBattleData.IntSkillData, _container.GetComponent<MultiEffectManager>());
        // else return new SpeedManager(speed);
        monster.gameObject.GetOrAddComponent<MonsterSpeedManager>().SetSpeed(speed);
    }
}

public class SpeedManagerCreater
{
    readonly BattleDIContainer _container;
    public SpeedManagerCreater(BattleDIContainer container) => _container = container;

    public SpeedManager CreateSpeedManager(float speed, Multi_NormalEnemy monster)
    {
        var skillData = _container.GetMultiActiveSkillData().GetData(monster.UsingId);
        if (skillData.TruGetSkillData(SkillType.썬콜, out var skillBattleData))
            return new SuncoldSpeedManager(speed, monster, skillBattleData.IntSkillData, _container.GetComponent<MultiEffectManager>());
        else return new SpeedManager(speed);
    }
}

public class EnemySpawnNumManager : MonoBehaviourPun
{
    byte[] _spawnEnemyNums = new byte[2] { 255, 255 };
    public bool IsMonsterSelect(byte playerId) => _spawnEnemyNums[playerId] != 255;
    
    public byte GetSpawnEnemyNum(int id) => _spawnEnemyNums[id];
    public void SetSpawnNumber(byte num)
    {
        if (PhotonNetwork.IsMasterClient)
            _spawnEnemyNums[0] = num;
        else
        {
            _spawnEnemyNums[1] = num;
            photonView.RPC(nameof(SetClientSpawnNumber), RpcTarget.MasterClient, num);
        }
    }
    [PunRPC]
    public void SetClientSpawnNumber(byte num) => _spawnEnemyNums[1] = num;
}

public class MonsterSpawnerContorller : MonoBehaviour
{
    IMonsterManager _monsterManager = null;
    EnemySpawnNumManager _numManager;
    BattleEventDispatcher _dispatcher;
    NormalMonsterSpawner _monsterSpawner;
    Multi_BossEnemySpawner _bossSpawner;

    public void Inject(BattleDIContainer container)
    {
        _monsterManager = container.GetComponent<IMonsterManager>();
        _numManager = container.GetComponent<EnemySpawnNumManager>();
        _dispatcher = container.GetEventDispatcher();
        _monsterSpawner = container.GetComponent<NormalMonsterSpawner>();
        _bossSpawner = Multi_SpawnManagers.BossEnemy;
        // _bossSpawner.Inject(speedManagerCreater);
    }

    public void Injection(IMonsterManager monsterManager, EnemySpawnNumManager numManager, BattleEventDispatcher dispatcher, SpeedManagerCreater speedManagerCreater)
    {
        _monsterManager = monsterManager;
        _numManager = numManager;
        _dispatcher = dispatcher;
        _monsterSpawner = new NormalMonsterSpawner(speedManagerCreater);
        _bossSpawner = Multi_SpawnManagers.BossEnemy;
        _bossSpawner.Inject(speedManagerCreater);
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        _dispatcher.OnStageUp += SpawnMonsterOnStageChange; // normal
        _dispatcher.OnStageUp += SpawnBossOnStageMultipleOfTen; // boss
        _dispatcher.OnGameStart += SpawnTowerOnStart; // tower
    }

    void SpawnMonsterOnStageChange(int stage)
    {
        if (IsBossStage(stage)) return;
        foreach (var id in PlayerIdManager.AllId)
            StartCoroutine(Co_StageSpawn(id, stage));
    }

    Multi_NormalEnemy SpawnMonsterToOther(int id, int stage) => SpawnNormalMonster(_numManager.GetSpawnEnemyNum(id), (byte)(id == 0 ? 1 : 0), stage);
    Multi_NormalEnemy SpawnNormalMonster(byte num, byte id, int stage)
    {
        var monster = _monsterSpawner.SpawnMonster(num, id, stage);
        _monsterManager.AddNormalMonster(monster);
        monster.OnDead += _ => _monsterManager.RemoveNormalMonster(monster); // event interface 맞추려는 람다식
        return monster;
    }

    [SerializeField] const float SpawnDelayTime = 1.8f;
    [SerializeField] const int StageSpawnCount = 15;
    WaitForSeconds WaitSpawnDelay = new WaitForSeconds(SpawnDelayTime);
    IEnumerator Co_StageSpawn(byte id, int stage)
    {
        for (int i = 0; i < StageSpawnCount; i++)
        {
            var enemy = SpawnMonsterToOther(id, stage);
            enemy.OnDead += (died) => ResurrectionMonsterToOther(enemy);
            yield return WaitSpawnDelay;
        }
    }

    void ResurrectionMonsterToOther(Multi_NormalEnemy enemy)
    {
        if (enemy.IsResurrection) return;
        var spawnEnemy = SpawnMonsterToOther(enemy.UsingId, enemy.SpawnStage);
        spawnEnemy.Resurrection();
    }

    void SpawnBossOnStageMultipleOfTen(int stage)
    {
        if (IsBossStage(stage) == false) return;
        foreach (var id in PlayerIdManager.AllId)
            _bossSpawner.SpawnBoss(id, stage / 10);
    }
    bool IsBossStage(int stage) => stage % 10 == 0;

    void SpawnTowerOnStart()
    {
        foreach (var id in PlayerIdManager.AllId)
            Multi_SpawnManagers.TowerEnemy.Spawn(id);
    }
}

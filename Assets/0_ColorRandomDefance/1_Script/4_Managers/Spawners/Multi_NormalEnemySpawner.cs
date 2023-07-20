using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NormalMonsterSpawner
{
    MultiData<ActiveUserSkillDataContainer> _activeSkillData;
    public NormalMonsterSpawner(MultiData<ActiveUserSkillDataContainer> skillData) => _activeSkillData = skillData;
    SpeedManagerCreater _speedManagerCreater;
    public NormalMonsterSpawner(SpeedManagerCreater speedManagerCreater) => _speedManagerCreater = speedManagerCreater;

    public Multi_NormalEnemy SpawnMonster(byte num, byte id, int stage)
    {
        var monster = Managers.Multi.Instantiater.PhotonInstantiateInactive(new ResourcesPathBuilder().BuildMonsterPath(num), id).GetComponent<Multi_NormalEnemy>();
        NormalEnemyData data = Managers.Data.NormalEnemyDataByStage[stage];
        //var speedManager = _activeSkillData.GetData(id).ActiveEquipSkill(SkillType.썬콜) ?  CreateSunCold(data.Speed, monster, _activeSkillData.GetData(id)) : new SpeedManager(data.Speed);
        monster.Injection(_speedManagerCreater.CreateSpeedManager(data.Speed, monster));
        monster.SetStatus_RPC(data.Hp, data.Speed, false);
        return monster;
    }

    SuncoldSpeedManager CreateSunCold(float speed, Multi_NormalEnemy monster, ActiveUserSkillDataContainer data) => new SuncoldSpeedManager(speed, monster, (int)data.GetFirstIntData(UserSkillClass.Main), null);
}

public class SpeedManagerCreater
{
    readonly BattleDIContainer _container;

    public SpeedManagerCreater(BattleDIContainer container) => _container = container;

    public SpeedManager CreateSpeedManager(float speed, Multi_NormalEnemy monster)
    {
        var skillData = _container.GetMultiActiveSkillData().GetData(monster.UsingId);
        if (skillData.ActiveEquipSkill(SkillType.썬콜))
            return new SuncoldSpeedManager(speed, monster, skillData.GetFirstIntData(UserSkillClass.Main), _container.GetComponent<MultiEffectManager>());
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
    public void Injection(IMonsterManager monsterManager, EnemySpawnNumManager numManager, BattleEventDispatcher dispatcher, NormalMonsterSpawner monsterSpawner)
    {
        _monsterManager = monsterManager;
        _numManager = numManager;
        _dispatcher = dispatcher;
        _monsterSpawner = monsterSpawner;
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        StageManager.Instance.OnUpdateStage += SpawnMonsterOnStageChange; // normal
        StageManager.Instance.OnUpdateStage += SpawnBossOnStageMultipleOfTen; // boss
        _dispatcher.OnGameStart += SpawnTowerOnStart; // tower
    }

    void SpawnMonsterOnStageChange(int stage)
    {
        if (IsBossStage(stage)) return;
        StartCoroutine(Co_StageSpawn(0, stage));
        StartCoroutine(Co_StageSpawn(1, stage));
    }

    Multi_NormalEnemy SpawnMonsterToOther(int id, int stage) => SpawnNormalMonster(_numManager.GetSpawnEnemyNum(id), (byte)(id == 0 ? 1 : 0), stage);
    Multi_NormalEnemy SpawnNormalMonster(byte num, byte id, int stage)
    {
        var monster = _monsterSpawner.SpawnMonster(num, id, stage);
        _monsterManager.AddNormalMonster(monster);
        monster.OnDead += _ => _monsterManager.RemoveNormalMonster(monster); // event interface 맞추려는 람다식
        return monster;
    }

    [SerializeField] float _spawnDelayTime = 1.8f;
    [SerializeField] int _stageSpawnCount = 15;
    IEnumerator Co_StageSpawn(byte id, int stage)
    {
        for (int i = 0; i < _stageSpawnCount; i++)
        {
            var enemy = SpawnMonsterToOther(id, stage);
            enemy.OnDead += (died) => ResurrectionMonsterToOther(enemy);
            yield return new WaitForSeconds(_spawnDelayTime);
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

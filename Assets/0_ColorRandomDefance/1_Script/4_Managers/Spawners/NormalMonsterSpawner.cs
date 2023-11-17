using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class NormalMonsterSpawner : MonoBehaviourPun
{
    MonsterDecorator _monsterDecorator;
    MonsterManagerController _monsterManagerController;
    public void DependencyInject(MonsterDecorator monsterDecorator, MonsterManagerController monsterManagerController)
    {
        _monsterDecorator = monsterDecorator;
        _monsterManagerController = monsterManagerController;
    }

    public Multi_NormalEnemy SpawnMonster(byte num, byte id, int stage)
    {
        var monster = Managers.Multi.Instantiater.PhotonInstantiateInactive(new ResourcesPathBuilder().BuildMonsterPath(num), id).GetComponent<Multi_NormalEnemy>();
        photonView.RPC(nameof(InjectMonster), RpcTarget.All,(byte)stage, monster.GetComponent<PhotonView>().ViewID);
        return monster;
    }

    [PunRPC]
    void InjectMonster(byte stage, int viewId)
    {
        var monster = Managers.Multi.GetPhotonViewComponent<Multi_NormalEnemy>(viewId);
        NormalEnemyData monsterData = Managers.Data.NormalEnemyDataByStage[stage];
        _monsterDecorator.DecorateSpeedSystem(monsterData.Speed, monster);
        monster.Inject(stage);
        _monsterManagerController.AddNormalMonster(monster);
        monster.OnDead += _ => _monsterManagerController.RemoveNormalMonster(monster);
    }
}

public class MonsterDecorator
{
    readonly BattleDIContainer _container;
    public MonsterDecorator(BattleDIContainer container) => _container = container;

    readonly int UnitDamageCount = System.Enum.GetValues(typeof(UnitClass)).Length;

    public void DecorateSpeedSystem(float speed, Multi_NormalEnemy monster)
    {
        var skillData = _container.GetMultiActiveSkillData().GetData(monster.UsingId);
        var speedSystem = monster.gameObject.GetOrAddComponent<MonsterSpeedSystem>();
        if (skillData.TruGetSkillData(SkillType.썬콜, out var skillBattleData))
            speedSystem.ReceiveInject(
                new SuncoldSpeedManager(speed, monster, skillBattleData.IntSkillDatas.Take(UnitDamageCount).ToArray(), _container.GetComponent<MultiEffectManager>(), _container.GetComponent<WorldAudioPlayer>())
                );
        else speedSystem.ReceiveInject(new SpeedManager(speed));
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
    EnemySpawnNumManager _numManager;
    BattleEventDispatcher _dispatcher;
    NormalMonsterSpawner _monsterSpawner;
    Multi_BossEnemySpawner _bossSpawner;

    public void Inject(BattleDIContainer container)
    {
        _numManager = container.GetComponent<EnemySpawnNumManager>();
        _dispatcher = container.GetEventDispatcher();
        _monsterSpawner = container.GetComponent<NormalMonsterSpawner>();
        _bossSpawner = container.GetComponent<Multi_BossEnemySpawner>();
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
    Multi_NormalEnemy SpawnNormalMonster(byte num, byte id, int stage) => _monsterSpawner.SpawnMonster(num, id, stage);

    WaitForSeconds WaitSpawnDelay = new WaitForSeconds(Multi_GameManager.Instance.BattleData.BattleData.MonsterSpawnDelayTime);
    IEnumerator Co_StageSpawn(byte id, int stage)
    {
        for (int i = 0; i < Multi_GameManager.Instance.BattleData.BattleData.StageMonsetSpawnCount; i++)
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

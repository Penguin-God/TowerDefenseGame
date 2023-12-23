using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class NormalMonsterSpawner : MonoBehaviourPun
{
    SpeedManagerCreator _monsterDecorator;
    MonsterManagerController _monsterManagerController;
    public void DependencyInject(SpeedManagerCreator monsterDecorator, MonsterManagerController monsterManagerController)
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
        monster.Inject(stage, _monsterDecorator.CreateSlowController(monster), new SpeedManager(monsterData.Speed));
        _monsterManagerController.AddNormalMonster(monster);
        monster.OnDead += _ => _monsterManagerController.RemoveNormalMonster(monster);
    }
}

public class SpeedManagerCreator
{
    readonly BattleDIContainer _container;
    public SpeedManagerCreator(BattleDIContainer container) => _container = container;

    readonly int UnitDamageCount = System.Enum.GetValues(typeof(UnitClass)).Length;
    public MonsterSlowController CreateSlowController(Multi_NormalEnemy monster)
    {
        var skillData = _container.GetMultiActiveSkillData().GetData(monster.UsingId);
        var slowController = monster.gameObject.GetOrAddComponent<SlowController>();
        if (skillData.TruGetSkillData(SkillType.썬콜, out var skillBattleData))
            return new SunColdMonsterSlowController(slowController, monster, skillBattleData.IntSkillDatas.Take(UnitDamageCount).ToArray(), _container.GetComponent<WorldAudioPlayer>());
        else return new MonsterSlowController(slowController);
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

public readonly struct SpawnData
{
    public readonly int OwnerId;
    public readonly int Stage;

    public SpawnData(int ownerId, int stage)
    {
        OwnerId = ownerId;
        Stage = stage; 
    }
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
        StartCoroutine(Co_ResurrectionUpdate());
    }

    void SpawnMonsterOnStageChange(int stage)
    {
        if (IsBossStage(stage)) return;
        foreach (var id in PlayerIdManager.AllId)
            StartCoroutine(Co_StageSpawn(id, stage));
    }

    Multi_NormalEnemy SpawnMonsterToOther(int id, int stage) => SpawnNormalMonster(_numManager.GetSpawnEnemyNum(id), (byte)(id == 0 ? 1 : 0), stage);
    Multi_NormalEnemy SpawnNormalMonster(byte num, byte id, int stage) => _monsterSpawner.SpawnMonster(num, id, stage);

    WaitForSeconds WaitSpawnDelay = new WaitForSeconds(Multi_GameManager.Instance.BattleData.MonsterSpawnDelayTime);
    IEnumerator Co_StageSpawn(byte id, int stage)
    {
        for (int i = 0; i < Multi_GameManager.Instance.BattleData.StageMonsetSpawnCount; i++)
        {
            var enemy = SpawnMonsterToOther(id, stage);
            enemy.OnDead += (died) => ResurrectionMonsterToOther(enemy);
            yield return WaitSpawnDelay;
        }
    }

    Queue<SpawnData> _resurrectionQueue = new Queue<SpawnData>();
    void ResurrectionMonsterToOther(Multi_NormalEnemy enemy)
    {
        if (enemy.IsResurrection) return;
        _resurrectionQueue.Enqueue(new SpawnData(enemy.UsingId, enemy.SpawnStage));
    }

    IEnumerator Co_ResurrectionUpdate()
    {
        while (true)
        {
            yield return null;
            if (_resurrectionQueue.Count == 0) continue;

            var spawnData = _resurrectionQueue.Dequeue();
            var spawnEnemy = SpawnMonsterToOther(spawnData.OwnerId, spawnData.Stage);
            spawnEnemy.Resurrection();
            yield return new WaitForSeconds(Multi_GameManager.Instance.BattleData.MonsterResurrectionDelayTime);
        }
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

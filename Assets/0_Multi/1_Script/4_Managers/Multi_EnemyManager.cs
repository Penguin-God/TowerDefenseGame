using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Multi_EnemyManager : MonoBehaviour
{
    private static Multi_EnemyManager instance;
    public static Multi_EnemyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Multi_EnemyManager>();
                if (instance == null)
                    instance = new GameObject("Multi_EnemyManager").AddComponent<Multi_EnemyManager>();
            }

            return instance;
        }
    }

    void Start()
    {
        Multi_EnemySpawner.instance.OnEnemySpawn += AddEnemy;
        Multi_EnemySpawner.instance.OnBossSpawn += SetBoss;
        Multi_EnemySpawner.instance.OnBossDead += SetBoss;
        Multi_EnemySpawner.instance.OnBossDead += GetBossReward;
        Multi_EnemySpawner.instance.OnTowerSpawn += SetTower;
        Multi_EnemySpawner.instance.OnTowerDead += SetTower;
    }

    [Header("Normal Enemy")]
    [SerializeField] List<Transform> allEnemys = new List<Transform>();
    public IReadOnlyList<Transform> AllEnemys => allEnemys;
    public int EnemyCount => allEnemys.Count;

    [Header("Boss Enemy")]
    [SerializeField] Multi_BossEnemy currentBoss;
    public Multi_BossEnemy CurrentBoss => currentBoss;
    [SerializeField] int currentBossLevel;
    public int CurrentBossLevel => currentBossLevel;
    public bool IsBossAlive => currentBoss != null;

    [SerializeField] int bossGoldReward;
    public int BossGoldReward => bossGoldReward;
    [SerializeField] int bossFoodReward;
    public int BossFoodReward => bossFoodReward;

    [Header("Enemy Tower")]
    [SerializeField] Multi_EnemyTower currentEnemyTower;
    public Multi_EnemyTower CurrentEnemyTower => currentEnemyTower;
    [SerializeField] int currentEnemyTowerLevel;
    public int CurrentEnemyTowerLevel => currentEnemyTowerLevel;

    public Transform GetProximateEnemy(Vector3 _unitPos, float _startDistance)
    {
        Transform[] _enemys = allEnemys.Select(x => x.transform).ToArray();
        float shortDistance = _startDistance;
        Transform _returnEnemy = null;
        
        if (_enemys.Length > 0)
        {
            foreach (Transform _enemy in _enemys)
            {
                if (_enemy != null && !_enemy.GetComponent<Multi_Enemy>().isDead)
                {
                    float distanceToEnemy = Vector3.Distance(_unitPos, _enemy.position);
                    if (distanceToEnemy < shortDistance)
                    {
                        shortDistance = distanceToEnemy;
                        _returnEnemy = _enemy;
                    }
                }
            }
        }

        return _returnEnemy;
    }

    public Multi_Enemy GetRandom_CurrentEnemy()
    {
        int index = Random.Range(0, allEnemys.Count);
        Multi_Enemy enemy = allEnemys[index].GetComponent<Multi_Enemy>();
        return enemy;
    }

    #region callback funtion
    void AddEnemy(Multi_Enemy _enemy) => allEnemys.Add(_enemy.transform);
    void SetBoss(Multi_BossEnemy _spawnBoss) => currentBoss = _spawnBoss;
    void SetBoss(int _level) => currentBoss = null;
    void SetTower(Multi_EnemyTower _spawnTower) => currentEnemyTower = _spawnTower;
    void SetTower(int _level) => currentEnemyTower = null;
    // TODO : 리펙토링 할 수 있나 생각해보기
    void GetBossReward(int _level)
    {
        Multi_GameManager.instance.AddGold(bossGoldReward * _level);
        Multi_GameManager.instance.AddFood(BossFoodReward * _level);
    }
    #endregion
}

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
    }

    [SerializeField] List<Transform> allEnemys = new List<Transform>();
    public IReadOnlyList<Transform> AllEnemys => allEnemys;
    void AddEnemy(Multi_Enemy _enemy) => allEnemys.Add(_enemy.transform);

    public int EnemyCount => allEnemys.Count;

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
}

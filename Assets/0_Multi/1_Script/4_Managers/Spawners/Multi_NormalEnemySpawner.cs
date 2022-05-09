using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Multi_NormalEnemySpawner
{
    public event Action<Multi_NormalEnemy> OnSpawn;
    public event Action<Multi_NormalEnemy> OnDead;

    string rootPath = "Enemy/Normal";
    public void Init(GameObject[,] allUnits)
    {

    }

    protected void CreatePool(string _path, int _count)
    {
        Transform root = Multi_Managers.Pool.CreatePool(_path, _count);

        for (int k = 0; k < root.childCount; k++)
        {
            Multi_NormalEnemy enemy = root.GetChild(k).GetComponent<Multi_NormalEnemy>();
            enemy.TurnPoints = Multi_Data.instance.EnemyTurnPoints;
            enemy.OnDeath += () => OnDead(enemy);
            enemy.OnDeath += () => Multi_Managers.Pool.Push(enemy.gameObject, _path);
        }
    }

    void Spawn(string path, int hp, float speed, Vector3 spawnPos)
    {
        Multi_NormalEnemy enemy = Multi_Managers.Resources.PhotonInsantiate(path).GetComponent<Multi_NormalEnemy>();
        RPC_Utility.Instance.RPC_Position(enemy.PV.ViewID, spawnPos);
        RPC_Utility.Instance.RPC_Active(enemy.PV.ViewID, true);
        enemy.SetStatus(RpcTarget.All, hp, speed, false);
        OnSpawn?.Invoke(enemy);
    }
}

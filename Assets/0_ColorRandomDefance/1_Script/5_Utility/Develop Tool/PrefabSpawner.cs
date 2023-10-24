using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    Multi_NormalUnitSpawner _unitSpawner;
    public void DependencyInject(Multi_NormalUnitSpawner unitSpawner)
    {
        _unitSpawner = unitSpawner;
    }

    public void SpawnUnit(UnitFlags flag) => SpawnUnit(flag, 0);
    public void SpawnUnit_ByClient(UnitFlags flag) => SpawnUnit(flag, 1);
    void SpawnUnit(UnitFlags flag, byte id) => _unitSpawner.Spawn(flag, id);
    public void SpawnNormalEnemy(byte enemyNum) { }//=> new NormalMonsterSpawner(new SpeedManagerCreater(_container));
}
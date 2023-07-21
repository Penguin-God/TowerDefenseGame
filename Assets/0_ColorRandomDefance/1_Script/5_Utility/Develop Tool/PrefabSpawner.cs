using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrawButtonUnits
{
    public GameObject[] units;
}

public class PrefabSpawner : MonoBehaviour
{
    public DrawButtonUnits[] allUnit;

    BattleDIContainer _container;
    void Start()
    {
        if(FindObjectOfType<BattleScene>() != null)
            _container = FindObjectOfType<BattleScene>().GetBattleContainer();
    }

    public void SpawnUnit(int colorNumber, int classNumber) => Multi_SpawnManagers.NormalUnit.Spawn(colorNumber, classNumber);

    public void SpawnUnit_ByClient(int colorNumber, int classNumber) => SpawnUnit_ByClientWolrd(colorNumber, classNumber);

    public void SpawnNormalEnemy(byte _enemyNum) => new NormalMonsterSpawner(new SpeedManagerCreater(_container));

    public void SpawnUnit_ByClientWolrd(int unitColorNumber, int unitClassNumber)
        => Multi_SpawnManagers.NormalUnit.Spawn(new UnitFlags(unitColorNumber, unitClassNumber), 1);
}
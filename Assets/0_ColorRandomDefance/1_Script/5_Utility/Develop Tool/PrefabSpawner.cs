using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    BattleDIContainer _container;
    void Start()
    {
        if(FindObjectOfType<BattleScene>() != null)
            _container = FindObjectOfType<BattleScene>().GetBattleContainer();
    }

    public void SpawnUnit(int colorNumber, int classNumber) => Multi_SpawnManagers.NormalUnit.Spawn(colorNumber, classNumber);

    public void SpawnUnit_ByClient(int colorNumber, int classNumber) => Multi_SpawnManagers.NormalUnit.Spawn(new UnitFlags(colorNumber, classNumber), 1);
    public void SpawnUnit(UnitFlags flag) => SpawnUnit(flag, 0);
    public void SpawnUnit_ByClient(UnitFlags flag) => SpawnUnit(flag, 1);
    void SpawnUnit(UnitFlags flag, byte id) => Multi_SpawnManagers.NormalUnit.Spawn(flag, id);
    public void SpawnNormalEnemy(byte _enemyNum) { }//=> new NormalMonsterSpawner(new SpeedManagerCreater(_container));
}
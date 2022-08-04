using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TestUtility : MonoBehaviour
{
    [SerializeField] UnitFlags flag;
    [ContextMenu("Test")]
    void Test()
    {
        Multi_Managers.Sound.PlayEffect(EffectSoundType.SwordmanAttack);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            Test();
    }

    [SerializeField] int spawnColorMax;
    [SerializeField] int spawnClassMax;
    [ContextMenu("범위 안의 Unit Spawn")]
    void UnitSpawn()
    {
        for (int i = 0; i <= spawnColorMax; i++)
        {
            for (int j = 0; j <= spawnClassMax; j++)
            {
                Multi_SpawnManagers.NormalUnit.Spawn(i, j);
            }
        }
    }
}
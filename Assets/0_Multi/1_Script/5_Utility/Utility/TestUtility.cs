using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestUtility : MonoBehaviour
{
    [SerializeField] string names;

    [ContextMenu("WeaponSpawn")]
    void Test()
    {
        Multi_SpawnManagers.Weapon.Spawn(WeaponType.MageSkills, names, transform.position);
    }
}
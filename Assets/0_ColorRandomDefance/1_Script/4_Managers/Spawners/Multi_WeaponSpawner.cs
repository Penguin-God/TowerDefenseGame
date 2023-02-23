using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Multi_WeaponSpawner : Multi_SpawnerBase
{
    public GameObject Spawn(string path, Vector3 spawnPos) => Managers.Multi.Instantiater.PhotonInstantiate($"Weapon/{path}", spawnPos);
}

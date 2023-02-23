using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner
{
    public static GameObject Spawn(string path, Vector3 spawnPos) => Managers.Multi.Instantiater.PhotonInstantiate($"Weapon/{path}", spawnPos);
}

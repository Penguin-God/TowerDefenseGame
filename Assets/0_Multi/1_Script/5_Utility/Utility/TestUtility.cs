using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ProjectileData
{
    [SerializeField] Multi_Projectile original;
    [SerializeField] Transform spawnTransform;

    public ProjectileData(Multi_Projectile original, Transform spawnPos)
    {
        this.original = original;
        this.spawnTransform = spawnPos;
    }

    public Multi_Projectile Original => original;
    public Transform SpawnTransform => spawnTransform;
    public Vector3 SpawnPos => spawnTransform.position;
}

public class TestUtility : MonoBehaviour
{

}

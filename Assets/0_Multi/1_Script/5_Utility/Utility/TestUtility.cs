using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestUtility : MonoBehaviour
{
    [SerializeField] ProjectileData arrawData;
    [SerializeField] Transform target;

    [ContextMenu("Shot Test")]
    void ShotTest()
    {
        ProjectileShotDelegate.ShotProjectile(arrawData, target, 2, null);
    }
}
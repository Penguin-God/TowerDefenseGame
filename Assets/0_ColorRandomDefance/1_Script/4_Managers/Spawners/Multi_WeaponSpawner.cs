using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Multi_WeaponSpawner : Multi_SpawnerBase
{
    protected override void MasterInit() => PoolWeapons();

    void PoolWeapons()
    {
        var unitClassByWeaponPoolingCount = new Dictionary<UnitClass, int>()
        {
            { UnitClass.Archer, 20 },
            { UnitClass.Spearman, 2 },
            { UnitClass.Mage, 0 },
        };

        foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
        {
            foreach (var classCountPair in unitClassByWeaponPoolingCount)
                CreatePoolGroup(PathBuilder.BuildUnitWeaponPath(new UnitFlags(color, classCountPair.Key)), classCountPair.Value);
        }

        foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
        {
            if (color == UnitColor.White) continue;
            CreatePoolGroup(PathBuilder.BuildMageSkillEffectPath(color), 0);
        }
    }

    public GameObject Spawn(string path, Vector3 spawnPos) => Managers.Multi.Instantiater.PhotonInstantiate($"Weapon/{path}", spawnPos);
}

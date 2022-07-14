using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestUtility : MonoBehaviour
{
    [SerializeField] string names;
    [SerializeField] Multi_Enemy target;
    [SerializeField] Vector3 offset;
    [ContextMenu("WeaponSpawn")]
    void Test()
    {
        Multi_Meteor meteor = 
            Multi_SpawnManagers.Weapon.Spawn(WeaponType.MageSkills, names, transform.position + offset).GetComponent<Multi_Meteor>();
        meteor.Shot(target, target.transform.position, (enemy) => print("안녕 세상"));
    }
}
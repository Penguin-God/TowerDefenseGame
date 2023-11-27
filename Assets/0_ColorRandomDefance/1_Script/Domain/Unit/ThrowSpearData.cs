using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct ThrowSpearData
{
    public string WeaponPath;
    public Vector3 RotateVector;
    public float WaitForVisibility;
    public float AttackRate;

    public ThrowSpearData(string weaponPath, Vector3 rotateVector, float waitForVisibility, float attackRate = 1)
    {
        WeaponPath = weaponPath;
        RotateVector = rotateVector;
        WaitForVisibility = waitForVisibility;
        AttackRate = attackRate;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct ThrowSpearData
{
    public string WeaponPath;
    public Vector3 RotateVector;
    public float WaitForVisibility;

    public ThrowSpearData(string weaponPath, Vector3 rotateVector, float waitForVisibility)
    {
        WeaponPath = weaponPath;
        RotateVector = rotateVector;
        WaitForVisibility = waitForVisibility;
    }
}
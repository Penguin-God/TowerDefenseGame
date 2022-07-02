using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpUtility
{
    public static void LerpPostition(Transform target, Vector3 pos)
    {
        if (Vector3.Distance(target.position, pos) < 10) target.position = pos;
        else target.position = Vector3.Lerp(target.position, pos, Time.deltaTime * 10);
    }

    public static void LerpRotation(Transform target, Quaternion rot)
    {
        if (Quaternion.Angle(target.rotation, rot) < 5 ) target.rotation = rot;
        else target.rotation = Quaternion.Lerp(target.rotation, rot, Time.deltaTime * 10);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterRaycastDetector
{
    // int layerMask;

    public bool CheckMonsterInDirection(Vector3 start, Vector3 dir, float range, Transform target)  
        => Physics.RaycastAll(start + Vector3.up, dir, range).Select(x => x.transform).Contains(target);
}

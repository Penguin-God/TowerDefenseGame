using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_RangeUnit : Multi_TeamSoldier
{
    protected override ChaseSystem AddCahseSystem() => gameObject.AddComponent<RangeChaser>();
}

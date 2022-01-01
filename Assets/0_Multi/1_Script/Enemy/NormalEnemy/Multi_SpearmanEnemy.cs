using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_SpearmanEnemy : Multi_NormalEnemy
{
    public override void Passive()
    {
        SetStatus(Mathf.FloorToInt(maxHp * 1.2f), speed);
    }
}

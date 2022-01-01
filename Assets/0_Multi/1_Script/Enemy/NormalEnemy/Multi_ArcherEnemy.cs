using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_ArcherEnemy : Multi_NormalEnemy
{
    public override void Passive()
    {
        SetStatus(maxHp, speed * 1.2f);
    }
}

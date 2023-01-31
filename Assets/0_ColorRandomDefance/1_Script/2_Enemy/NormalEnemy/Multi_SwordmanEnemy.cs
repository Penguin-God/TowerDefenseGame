using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_SwordmanEnemy : Multi_NormalEnemy
{
    protected override void Passive()
    {
        ChangeMaxHp((int)(maxHp * 1.5));
    }
}

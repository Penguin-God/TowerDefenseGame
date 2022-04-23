using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemy : NomalEnemy
{
    public override void Passive()
    {
        SetStatus(maxHp, speed * 1.2f);
    }
}

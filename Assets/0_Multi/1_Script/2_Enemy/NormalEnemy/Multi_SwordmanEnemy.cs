using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_SwordmanEnemy : Multi_NormalEnemy
{
    protected override void Passive()
    {
        maxHp = (int)(maxHp * 1.5);
        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
    }
}

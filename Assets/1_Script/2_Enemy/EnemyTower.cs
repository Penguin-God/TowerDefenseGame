using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTower : Enemy
{
    private void Awake()
    {
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
    }
}

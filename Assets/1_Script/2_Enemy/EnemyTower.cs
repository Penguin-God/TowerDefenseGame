using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTower : MonoBehaviour
{
    // 상태 변수
    public int maxHp;
    public int currentHp;
    public bool isDead;
    public Slider hpSlider;

    private void Awake()
    {
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
    }

    private void OnTriggerEnter(Collider other)
    {
        TeamSoldier teamSoldier = other.GetComponent<TeamSoldier>();
        if (teamSoldier != null)
        {
            teamSoldier.target = this.transform;
        }
    }
}

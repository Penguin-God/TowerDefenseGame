﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class UnitPassive : MonoBehaviour
{
    public TeamSoldier teamSoldier;

    private void Awake()
    {
        teamSoldier = GetComponent<TeamSoldier>();
        if(teamSoldier == null)
        {
            Debug.LogWarning("이상한 곳에 스크립트 배정함");
            Destroy(gameObject);
        }

        SetPassive();
    }

    public abstract void SetPassive();

    public abstract void ApplyData(float p1, float en_p1, float p2 = 0, float en_p2 = 0, float p3 = 0, float en_p3 = 0);

    public abstract void Beefup_Passive();
}

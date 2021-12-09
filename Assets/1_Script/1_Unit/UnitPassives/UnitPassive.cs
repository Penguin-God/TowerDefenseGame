using System.Collections;
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
        }

        SetPassive();
    }

    public abstract void SetPassive();

    public abstract void Beefup_Passive();
}

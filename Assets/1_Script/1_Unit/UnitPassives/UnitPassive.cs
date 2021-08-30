using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPassive : MonoBehaviour
{
    TeamSoldier teamSoldier;

    private void Start()
    {
        teamSoldier = GetComponent<TeamSoldier>();
        if(teamSoldier == null)
        {
            Debug.LogWarning("이상한 곳에 스크립트 배정함");
        }
    }
}

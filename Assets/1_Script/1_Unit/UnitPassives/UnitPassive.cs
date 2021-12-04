using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPassive : MonoBehaviour
{
    public TeamSoldier teamSoldier;
    protected bool isPassiveReinforce = false; // 패시브가 강화되었으면 true

    private void Awake()
    {
        teamSoldier = GetComponent<TeamSoldier>();
        if(teamSoldier == null)
        {
            Debug.LogWarning("이상한 곳에 스크립트 배정함");
        }

        SetPassive();
    }

    public virtual void SetPassive()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_VioletPassive : Multi_UnitPassive
{
    [SerializeField] int apply_SturnPercent;
    [SerializeField] float apply_StrunTime;
    [SerializeField] int apply_MaxPoisonDamage;

    public override void SetPassive(Multi_TeamSoldier _team)
    {
        _team.delegate_OnPassive += (Multi_Enemy enemy) => Passive_Violet(enemy);
    }

    void Passive_Violet(Multi_Enemy _enemy)
    {
        apply_SturnPercent = 60;
        apply_StrunTime = 3;
        apply_MaxPoisonDamage = 50;
        _enemy.photonView.RPC("OnStern", RpcTarget.MasterClient, apply_SturnPercent, apply_StrunTime);
        _enemy.photonView.RPC("OnPoison", RpcTarget.MasterClient, 20, 4, 0.5f, apply_MaxPoisonDamage);
    }

    public override void ApplyData(float p1, float p2 = 0, float p3 = 0)
    {
        apply_SturnPercent = (int)p1;
        apply_StrunTime = p2;
        apply_MaxPoisonDamage = (int)p3;
    }
}

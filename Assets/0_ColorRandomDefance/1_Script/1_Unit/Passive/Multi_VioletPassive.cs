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
        _team.OnPassiveHit += Passive_Violet;
    }

    void Passive_Violet(Multi_Enemy _enemy)
    {
        _enemy.OnStun_RPC(apply_SturnPercent, apply_StrunTime);
        _enemy.OnPoison_RPC(20, 4, 0.5f, apply_MaxPoisonDamage);
    }

    protected override void ApplyData()
    {
        apply_SturnPercent = (int)_stats[0];
        apply_StrunTime = _stats[1];
        apply_MaxPoisonDamage = (int)_stats[2];
    }
}

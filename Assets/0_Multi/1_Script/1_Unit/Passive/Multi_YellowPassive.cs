using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_YellowPassive : Multi_UnitPassive
{
    [SerializeField] int apply_GetGoldPercent;
    [SerializeField] int apply_AddGold;


    public override void SetPassive(Multi_TeamSoldier _team) 
    {
        _team.OnPassiveHit += enemy => Passive_Yellow(apply_AddGold, apply_GetGoldPercent); 
    }

    void Passive_Yellow(int addGold, int percent)
    {
        int random = Random.Range(0, 100);
        if (random < percent)
        {
            Multi_GameManager.instance.AddGold(addGold);
            Multi_Managers.Sound.PlayEffect(EffectSoundType.GetPassiveGold);
        }
    }

    protected override void ApplyData()
    {
        apply_GetGoldPercent = (int)_stats[0];
        apply_AddGold = (int)_stats[1];
    }
}

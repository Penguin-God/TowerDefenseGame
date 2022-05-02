using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_YellowPassive : Multi_UnitPassive
{
    [SerializeField] int apply_GetGoldPercent;
    [SerializeField] int apply_AddGold;


    public override void SetPassive(Multi_TeamSoldier _team) 
    {
        apply_GetGoldPercent = 70;
        apply_AddGold = 1;
        _team.OnPassiveHit += enemy => Passive_Yellow(apply_AddGold, apply_GetGoldPercent); 
    }

    void Passive_Yellow(int addGold, int percent)
    {
        int random = Random.Range(0, 100);
        if (random < percent)
        {
            Multi_GameManager.instance.AddGold(addGold);
            SoundManager.instance.PlayEffectSound_ByName("GetPassiveAttackGold");
        }
    }


    public override void ApplyData(float p1, float p2 = 0, float p3 = 0)
    {
        apply_GetGoldPercent = (int)p1;
        apply_AddGold = (int)p2;
    }
}

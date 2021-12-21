﻿using UnityEngine;

public class VioletPassive : UnitPassive
{
    [SerializeField] int sturnPercent;
    [SerializeField] float strunTime;
    [SerializeField] int maxPoisonDamage;

    public override void SetPassive()
    {
        teamSoldier.delegate_OnPassive += (Enemy enemy) => Passive_Violet(enemy);
    }

    void Passive_Violet(Enemy p_Enemy)
    {
        p_Enemy.EnemyStern(sturnPercent, strunTime);
        p_Enemy.EnemyPoisonAttack(20, 4, 0.5f, maxPoisonDamage);
    }

    public override void ApplyData(float P1, float P2 = 0, float P3 = 0)
    {
        sturnPercent = (int)P1;
        strunTime = P2;
        maxPoisonDamage = (int)P3;
    }

    [Space] [Space] [Space]
    [SerializeField] int enhanced_SturnPercent;
    [SerializeField] float enhanced_StrunTime;
    [SerializeField] int enhanced_MaxPoisonDamage;
    public override void Beefup_Passive()
    {
        sturnPercent = enhanced_SturnPercent;
        strunTime = enhanced_StrunTime;
        maxPoisonDamage = enhanced_MaxPoisonDamage;
    }
}

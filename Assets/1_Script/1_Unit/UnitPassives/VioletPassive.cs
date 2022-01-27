using UnityEngine;

public class VioletPassive : UnitPassive
{
    [SerializeField] int apply_SturnPercent;
    [SerializeField] float apply_StrunTime;
    [SerializeField] int apply_MaxPoisonDamage;

    //[Space]
    //[Space]
    //[SerializeField] int sturnPercent;
    //[SerializeField] float strunTime;
    //[SerializeField] int maxPoisonDamage;

    
    public override void SetPassive()
    {
        teamSoldier.delegate_OnPassive += (Enemy enemy) => Passive_Violet(enemy);
    }

    void Passive_Violet(Enemy p_Enemy)
    {
        p_Enemy.EnemyStern(apply_SturnPercent, apply_StrunTime);
        p_Enemy.EnemyPoisonAttack(20, 4, 0.5f, apply_MaxPoisonDamage);
    }

    public override void ApplyData(float p1, float p2 = 0, float p3 = 0)
    {
        apply_SturnPercent = (int)p1;
        apply_StrunTime = p2;
        apply_MaxPoisonDamage = (int)p3;
    }

    //public override void ApplyData(float p1, float en_p1, float p2 = 0, float en_p2 = 0, float p3 = 0, float en_p3 = 0)
    //{
    //    sturnPercent = (int)p1;
    //    enhanced_SturnPercent = (int)en_p1;
    //    strunTime = p2;
    //    enhanced_StrunTime = en_p2;
    //    maxPoisonDamage = (int)p3;
    //    enhanced_MaxPoisonDamage = (int)en_p3;

    //    apply_SturnPercent = sturnPercent;
    //    apply_StrunTime = strunTime;
    //    apply_MaxPoisonDamage = maxPoisonDamage;
    //}

    //[Space] [Space] [Space]
    //[SerializeField] int enhanced_SturnPercent;
    //[SerializeField] float enhanced_StrunTime;
    //[SerializeField] int enhanced_MaxPoisonDamage;
    //public override void Beefup_Passive()
    //{
    //    apply_SturnPercent = enhanced_SturnPercent;
    //    apply_StrunTime = enhanced_StrunTime;
    //    apply_MaxPoisonDamage = enhanced_MaxPoisonDamage;
    //}
}

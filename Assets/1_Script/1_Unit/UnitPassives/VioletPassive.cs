using UnityEngine;

public class VioletPassive : UnitPassive
{
    [SerializeField] int apply_SturnPercent;
    [SerializeField] float apply_StrunTime;
    [SerializeField] int apply_MaxPoisonDamage;
    
    public override void SetPassive(TeamSoldier _team)
    {
        _team.delegate_OnPassive += (Enemy enemy) => Passive_Violet(enemy);
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
}

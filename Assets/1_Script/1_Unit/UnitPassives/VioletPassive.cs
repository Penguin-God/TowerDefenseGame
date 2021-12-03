using UnityEngine;

public class VioletPassive : UnitPassive
{
    [SerializeField] int sturnPercent;
    [SerializeField] float strunTime;
    
    [Space]
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
}

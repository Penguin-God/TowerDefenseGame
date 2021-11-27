using UnityEngine;
public class FreezeEnemy : MageSkill
{
    public override void HitSkile(Enemy enemy) 
    {
        enemy.EnemyFreeze(5f);
    }
}
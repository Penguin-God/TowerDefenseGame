public class VioletSkill : MageSkill
{
    public override void HitSkile(Enemy enemy)
    {
        enemy.EnemyPoisonAttack(25, 8, 0.3f, 120000);
    }
}

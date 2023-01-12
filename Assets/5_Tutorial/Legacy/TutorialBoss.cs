public class TutorialBoss : TutorialGuideTrigger
{
    public override bool TutorialCondition()
    {
        if (EnemySpawn.instance == null) return false;
        else return EnemySpawn.instance.BossRespawn;
    }
}

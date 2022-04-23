using UnityEngine;

public class YellowPassive : UnitPassive
{
    [SerializeField] int apply_GetGoldPercent;
    [SerializeField] int apply_AddGold;


    public override void SetPassive(TeamSoldier _team) => _team.delegate_OnPassive += (Enemy enemy) => Passive_Yellow(apply_AddGold, apply_GetGoldPercent);

    void Passive_Yellow(int addGold, int percent)
    {
        int random = Random.Range(0, 100);
        if (random < percent)
        {
            SoundManager.instance.PlayEffectSound_ByName("GetPassiveAttackGold");
            GameManager.instance.Gold += addGold;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
        }
    }


    public override void ApplyData(float p1, float p2 = 0, float p3 = 0)
    {
        apply_GetGoldPercent = (int)p1;
        apply_AddGold = (int)p2;
    }
}

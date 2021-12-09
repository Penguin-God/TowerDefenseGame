using UnityEngine;

public class YellowPassive : UnitPassive
{
    [SerializeField] int addGold;
    [SerializeField] int getGoldPercent;

    public override void SetPassive()
    {
        teamSoldier.delegate_OnPassive += (Enemy enemy) => Passive_Yellow(addGold, getGoldPercent);
    }

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

    [Space] [Space]
    [SerializeField] int enhanced_AddGold;
    [SerializeField] int enhanced_GetGoldPercent;
    public override void Beefup_Passive()
    {
        addGold = enhanced_AddGold;
        getGoldPercent = enhanced_GetGoldPercent;
    }
}

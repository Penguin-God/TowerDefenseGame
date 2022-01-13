using UnityEngine;

public class YellowPassive : UnitPassive
{
    [SerializeField] int apply_GetGoldPercent;
    [SerializeField] int apply_AddGold;

    //[Space]
    //[Space]
    //[SerializeField] int getGoldPercent;
    //[SerializeField] int addGold;


    public override void SetPassive() => teamSoldier.delegate_OnPassive += (Enemy enemy) => Passive_Yellow(apply_AddGold, apply_GetGoldPercent);

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

    //public override void ApplyData(float p1, float en_p1, float p2 = 0, float en_p2 = 0, float p3 = 0, float en_p3 = 0)
    //{
    //    getGoldPercent = (int)p1;
    //    enhanced_GetGoldPercent = (int)en_p1;
    //    addGold = (int)p2;
    //    enhanced_AddGold = (int)en_p2;

    //    apply_GetGoldPercent = getGoldPercent;
    //    apply_AddGold = addGold;
    //}

    //[Space] [Space]    
    //[SerializeField] int enhanced_GetGoldPercent;
    //[SerializeField] int enhanced_AddGold;

    //public override void Beefup_Passive()
    //{
    //    apply_GetGoldPercent = enhanced_GetGoldPercent;
    //    apply_AddGold = enhanced_AddGold;
    //}
}

using UnityEngine;
public class OrangePassive : UnitPassive
{
    [SerializeField] float upBossDamageWeigh;

    public override void SetPassive()
    {
        EventManager.instance.ChangeUnitBossDamage(teamSoldier, upBossDamageWeigh);
    }

    [Space]
    [SerializeField] float enhanced_UpBossDamageWeigh;
    public override void Beefup_Passive()
    {
        upBossDamageWeigh = enhanced_UpBossDamageWeigh;
    }
}

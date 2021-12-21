using UnityEngine;
public class OrangePassive : UnitPassive
{
    [SerializeField] float upBossDamageWeigh;

    public override void SetPassive()
    {
        EventManager.instance.ChangeUnitBossDamage(teamSoldier, upBossDamageWeigh);
    }

    public override void ApplyData(float P1, float P2 = 0, float P3 = 0)
    {
        upBossDamageWeigh = P1;
    }

    [Space]
    [SerializeField] float enhanced_UpBossDamageWeigh;
    public override void Beefup_Passive()
    {
        upBossDamageWeigh = enhanced_UpBossDamageWeigh;
    }
}

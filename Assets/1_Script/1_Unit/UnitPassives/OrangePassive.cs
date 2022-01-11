using UnityEngine;
public class OrangePassive : UnitPassive
{
    [SerializeField] float apply_UpBossDamageWeigh;

    [Space]
    [Space]
    [SerializeField] float upBossDamageWeigh;

    public override void SetPassive()
    {
        EventManager.instance.ChangeUnitBossDamage(teamSoldier, apply_UpBossDamageWeigh);
    }

    public override void ApplyData(float p1, float en_p1, float p2 = 0, float en_p2 = 0, float p3 = 0, float en_p3 = 0)
    {
        upBossDamageWeigh = p1;
        enhanced_UpBossDamageWeigh = en_p1;

        apply_UpBossDamageWeigh = upBossDamageWeigh;
    }

    [Space]
    [SerializeField] float enhanced_UpBossDamageWeigh;
    public override void Beefup_Passive()
    {
        apply_UpBossDamageWeigh = enhanced_UpBossDamageWeigh;
    }
}

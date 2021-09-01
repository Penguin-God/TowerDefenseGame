using UnityEngine;
public class OrangePassive : UnitPassive
{
    [SerializeField] float upBossDamageWeigh;

    public override void SetPassive()
    {
        base.SetPassive();
        EventManager.instance.ChangeUnitBossDamage(teamSoldier, upBossDamageWeigh);
    }
}

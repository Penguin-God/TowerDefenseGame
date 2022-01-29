using UnityEngine;
public class OrangePassive : UnitPassive
{
    [SerializeField] float apply_UpBossDamageWeigh;

    public override void SetPassive(TeamSoldier _team)
    {
        EventManager.instance.ChangeUnitBossDamage(_team, apply_UpBossDamageWeigh);
    }

    public override void ApplyData(float p1, float p2 = 0, float p3 = 0)
    {
        apply_UpBossDamageWeigh = p1;
    }
}

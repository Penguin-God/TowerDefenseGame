using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum UnitPassvieType
{
    Attack,
    Upgrade,
}

abstract public class Multi_UnitPassive : MonoBehaviourPun
{
    [SerializeField] protected IReadOnlyList<float> _stats;
    public void LoadStat(UnitFlags flag)
    {
        _stats = Managers.Data.GetUnitPassiveStats(flag);
        ApplyData();
    }
    protected virtual void ApplyData() { }

    public abstract void SetPassive(Multi_TeamSoldier _team);
}

public abstract class UnitPassive
{
    public readonly UnitPassvieType PassvieType;
    public void DoUnitPassive(Unit unit) => CreatePassive(unit.UnitFlags).DoUnitPassive(unit);

    protected abstract void DoPassive();

    UnitPassive CreatePassive(UnitFlags unitFlags)
    {
        return null;
    }
}
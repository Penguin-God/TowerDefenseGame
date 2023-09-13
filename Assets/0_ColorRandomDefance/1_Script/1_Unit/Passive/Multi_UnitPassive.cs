using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Multi_UnitPassive : MonoBehaviour
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
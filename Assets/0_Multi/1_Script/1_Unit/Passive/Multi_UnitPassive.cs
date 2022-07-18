using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

abstract public class Multi_UnitPassive : MonoBehaviourPun
{
    [SerializeField] protected IReadOnlyList<float> _stats;
    public void LoadStat(UnitFlags flag)
    {
        _stats = Multi_Managers.Data.GetUnitPassiveStats(flag);
        ApplyData();
    }

    public abstract void SetPassive(Multi_TeamSoldier _team);
    public virtual void ApplyData(float p1, float p2 = 0, float p3 = 0) { }

    protected abstract void ApplyData();
}
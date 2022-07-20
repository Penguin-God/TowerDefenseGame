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
    protected abstract void ApplyData();

    public abstract void SetPassive(Multi_TeamSoldier _team);
}
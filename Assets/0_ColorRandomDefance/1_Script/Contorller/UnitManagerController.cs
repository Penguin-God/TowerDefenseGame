using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UnitManagerController : MonoBehaviourPun
{
    readonly MultiManager<UnitManager> _multiUnitManagerService = new MultiManager<UnitManager>(() => new UnitManager());
    UnitManager MasterUnitManager => _multiUnitManagerService.GetServiece();
    readonly Dictionary<Unit, Multi_TeamSoldier> _controllerByUnit = new Dictionary<Unit, Multi_TeamSoldier>();

    void Awake()
    {
        foreach (var service in _multiUnitManagerService.Services)
        {
            service.OnRemoveUnit -= RemoveUnit;
            service.OnRemoveUnit += RemoveUnit;
        }
    }

    public void RegisterUnit(Multi_TeamSoldier unit)
    {
        _controllerByUnit.Add(unit.Unit, unit);
        _multiUnitManagerService.GetServiece(unit.UsingID).RegisterUnit(unit.Unit);
    }

    void RemoveUnit(Unit unit) => _controllerByUnit.Remove(unit);

    public bool TryFindUnit(byte id, UnitFlags flag, out Multi_TeamSoldier result)
    {
        var unit = _multiUnitManagerService.GetServiece(id).GetUnit(flag);
        if (unit == null)
        {
            result = null;
            return false;
        }
        else
        {
            result = _controllerByUnit[unit];
            return true;
        }
    }

    public bool TryCombine(byte id, UnitFlags flag)
    {
        bool result = _multiUnitManagerService.GetServiece(id).CheckCombinealbe(flag);
        //if (result)
        //    Combine(flag);
        return result;
    }
}

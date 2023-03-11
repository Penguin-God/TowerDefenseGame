using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UnitManagerController : MonoBehaviourPun
{
    readonly MultiManager<UnitManager> _multiUnitManagerService = new MultiManager<UnitManager>(() => new UnitManager());
    UnitManager UnitManager => _multiUnitManagerService.GetServiece();
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
}

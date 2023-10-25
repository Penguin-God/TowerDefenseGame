using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManagerController
{
    public readonly WorldUnitManager WorldUnitManager;
    HashSet<Multi_TeamSoldier> _unitControllers = new HashSet<Multi_TeamSoldier>();
    public UnitManagerController(BattleEventDispatcher dispatcher)
    {
        WorldUnitManager = new WorldUnitManager();
        dispatcher.OnUnitSpawn += AddUnit;
    }

    public void AddUnit(Multi_TeamSoldier unit)
    {
        _unitControllers.Add(unit);
        WorldUnitManager.AddUnit(unit.Unit, unit.UsingID);
        unit.OnDead += RemoveUnit;
    }

    public void RemoveUnit(Multi_TeamSoldier unit)
    {
        _unitControllers.Remove(unit);
        WorldUnitManager.RemoveUnit(unit.Unit, unit.UsingID);
    }

    Multi_TeamSoldier GetUnit(Unit unit) => _unitControllers.Where(x => x.Unit == unit).FirstOrDefault();
    public Multi_TeamSoldier GetUnit(byte id, UnitFlags flag) => GetUnit(WorldUnitManager.GetUnit(id, flag));
}

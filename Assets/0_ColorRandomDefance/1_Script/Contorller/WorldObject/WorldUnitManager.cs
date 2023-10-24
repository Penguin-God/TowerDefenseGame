using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUnitManager
{
    readonly WorldObjectManager<Multi_TeamSoldier> _units = new();
    readonly BattleEventDispatcher _battleEventDispatcher;
    public WorldUnitManager(BattleEventDispatcher battleEventDispatcher) => _battleEventDispatcher = battleEventDispatcher;

    public void AddUnit(Multi_TeamSoldier unit)
    {
        _units.AddObject(unit, unit.UsingID);
        // event
    }

    public void RemoveUnit(Multi_TeamSoldier unit)
    {
        _units.RemoveObject(unit, unit.UsingID);
        // event
    }

    public IEnumerable GetUnits(byte worldId) => _units.GetList(worldId);
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class WorldUnitManager
{
    readonly WorldObjectManager<Unit> _units = new();
    public void AddUnit(Unit unit, byte id)
    {
        _units.AddObject(unit, id);
    }

    public void RemoveUnit(Unit unit, byte id)
    {
        _units.RemoveObject(unit, id);
    }

    public IEnumerable<Unit> GetUnits(byte worldId) => _units.GetList(worldId);
    public IEnumerable<UnitFlags> GetUnitFlags(byte worldId) => GetUnits(worldId).Select(x => x.UnitFlags);
    public Unit GetUnit(byte worldId, UnitFlags flag) => GetUnits(worldId).Where(x => x.UnitFlags == flag).FirstOrDefault();
    public int GetUnitCount(byte worldId, Func<Unit, bool> condition) => GetUnits(worldId).Where(condition).Count();
    public int GetUnitCount(byte worldId) => GetUnitCount(worldId, _ => true);
}

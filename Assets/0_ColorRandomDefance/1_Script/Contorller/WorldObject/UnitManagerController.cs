using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UnitManagerController
{
    public readonly WorldUnitManager WorldUnitManager;
    HashSet<Multi_TeamSoldier> _unitControllers = new HashSet<Multi_TeamSoldier>();
    BattleEventDispatcher _dispatcher;
    public UnitManagerController(BattleEventDispatcher dispatcher)
    {
        WorldUnitManager = new WorldUnitManager();
        _dispatcher = dispatcher;
        _dispatcher.OnUnitSpawn += AddUnit;
    }

    public void AddUnit(Multi_TeamSoldier unit)
    {
        _unitControllers.Add(unit);
        WorldUnitManager.AddUnit(unit.Unit, unit.UsingID);
        NotifyChangeUnitCount(unit);
        unit.OnDead += RemoveUnit;
    }

    public void RemoveUnit(Multi_TeamSoldier unit)
    {
        _unitControllers.Remove(unit);
        WorldUnitManager.RemoveUnit(unit.Unit, unit.UsingID);
        NotifyChangeUnitCount(unit);
    }

    void NotifyChangeUnitCount(Multi_TeamSoldier unit)
    {
        _dispatcher.NotifyUnitCountChange(WorldUnitManager.GetUnitCount(unit.UsingID, x => true));
        _dispatcher.NotifyUnitCountChangeWithFlag(unit.UnitFlags, WorldUnitManager.GetUnitCount(unit.UsingID, x => x.UnitFlags == unit.UnitFlags));
        _dispatcher.NotifyUnitCountChangeWithClass(unit.UnitClass, WorldUnitManager.GetUnitCount(unit.UsingID, x => x.UnitFlags.UnitClass == unit.UnitClass));
    }

    Multi_TeamSoldier GetUnit(Unit unit) => _unitControllers.Where(x => x.Unit == unit).FirstOrDefault();
    public Multi_TeamSoldier GetUnit(byte id, UnitFlags flag) => GetUnit(WorldUnitManager.GetUnit(id, flag));
}

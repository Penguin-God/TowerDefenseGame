using System.Collections;
using System.Collections.Generic;

public class UnitsData
{
    public int CurrentUnitCount { get; private set; }
    public List<Multi_TeamSoldier> _units = new List<Multi_TeamSoldier>();
    public IReadOnlyList<Multi_TeamSoldier> Units => _units;
    
    public void AddUnit(Multi_TeamSoldier unit)
    {
        _units.Add(unit);
        CurrentUnitCount = _units.Count;
    }

    public void RemoveUnit(Multi_TeamSoldier unit)
    {
        _units.Remove(unit);
        CurrentUnitCount = _units.Count;
    }
}

public class ServerManager
{
    public IReadOnlyList<Multi_TeamSoldier> GetUnits(byte playerId) => _unitsData.GetData(playerId).Units;
    public void AddUnit(Multi_TeamSoldier unit)
    {
        _unitsData.GetData(unit.UsingID).AddUnit(unit);
        unit.OnDead += _unitsData.GetData(unit.UsingID).RemoveUnit;
    }

    readonly MultiData<UnitsData> _unitsData = new MultiData<UnitsData>(() => new UnitsData());
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UnitManager : ObjectManager<Unit>
{
    public Unit FindUnit(UnitFlags flag) => List.Where(x => x.UnitFlags == flag).FirstOrDefault();
}

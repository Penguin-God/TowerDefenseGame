using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPassiveCreator
{
    DataManager _data;
    public UnitPassiveCreator(DataManager data)
    {
        _data = data;
    }

    public IUnitAttackPassive CreateAttackPassive(Unit unit, byte ownerId)
    {
        IReadOnlyList<float> passiveDatas = _data.GetUnitPassiveStats(unit.UnitFlags);

        if (unit.UnitFlags == new UnitFlags(UnitColor.Blue, UnitClass.Mage))
            return new MonsterSlower(0, 0, unit.UnitFlags);
        else if (unit.UnitFlags == new UnitFlags(UnitColor.Yellow, UnitClass.Swordman))
            return null;

        switch (unit.UnitFlags.UnitColor)
        {
            case UnitColor.Blue: return new MonsterSlower((int)passiveDatas[0], (int)passiveDatas[1], unit.UnitFlags);
            case UnitColor.Yellow: return new GoldenAttacker((int)passiveDatas[0], (int)passiveDatas[1], ownerId);
            case UnitColor.Violet: return new PosionAndStunActor((int)passiveDatas[0], passiveDatas[1], (int)passiveDatas[2], passiveDatas[3], unit);
            default: return null;
        }
    }

    public void AttachedPassive(GameObject obj, UnitFlags flag)
    {
        IReadOnlyList<float> passiveDatas = _data.GetUnitPassiveStats(flag);

        if (flag == new UnitFlags(UnitColor.Blue, UnitClass.Mage))
            obj.GetOrAddComponent<AreaSlowApplier>().Inject(passiveDatas[0], passiveDatas[1]);
        else if (flag.UnitColor == UnitColor.Red)
            obj.GetComponentInChildren<AreaAttackSpeedBuffProvider>().DependecyInject(passiveDatas[0], passiveDatas[1]);
    }
}

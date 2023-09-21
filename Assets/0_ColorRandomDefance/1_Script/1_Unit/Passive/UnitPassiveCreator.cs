using System.Collections;
using System.Collections.Generic;

public class UnitPassiveCreator
{
    DataManager _data;
    public UnitPassiveCreator(DataManager data)
    {
        _data = data;
    }

    public IUnitAttackPassive CreatePassive(UnitFlags flag)
    {
        IReadOnlyList<float> passiveDatas = _data.GetUnitPassiveStats(flag);

        if (flag == new UnitFlags(UnitColor.Blue, UnitClass.Mage))
            return new MonsterSlower(0, 0);
        else if (flag == new UnitFlags(UnitColor.Yellow, UnitClass.Swordman))
            return null;

        switch (flag.UnitColor)
        {
            case UnitColor.Blue: return new MonsterSlower((int)passiveDatas[0], (int)passiveDatas[1]);
            case UnitColor.Yellow: return new GoldenAttacker((int)passiveDatas[0], (int)passiveDatas[1]);
            case UnitColor.Violet: return new PosionAndStunActor((int)passiveDatas[0], passiveDatas[1], (int)passiveDatas[2], (int)passiveDatas[3]);
            default: return null;
        }
    }
}
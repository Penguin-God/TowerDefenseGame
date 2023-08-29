using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataConvertUtili
{
    public UnitFlags ToUnitFlag(float[] datas)
    {
        int[] unitDatas = ToInts(datas, 2);
        return new UnitFlags(unitDatas[0], unitDatas[1]);
    }
    public UnitUpgradeData ToUnitUpgradeData(float[] datas)
    {
        int[] upgradeDatas = ToInts(datas, 3);
        return new UnitUpgradeData((UnitUpgradeType)upgradeDatas[0], (UnitColor)upgradeDatas[1], upgradeDatas[2], null);
    }

    int[] ToInts(float[] datas, int useCount)
    {
        if (datas.Length != useCount)
            throw new ArgumentException("�迭�� ���̰� �����͸� �����ϱ⿡ �������� ����");
        return datas.Select(x => Mathf.RoundToInt(x)).ToArray();
    }
}

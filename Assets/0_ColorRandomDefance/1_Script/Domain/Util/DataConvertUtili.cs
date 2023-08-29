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
            throw new ArgumentException("배열의 길이가 데이터를 생성하기에 적절하지 않음");
        return datas.Select(x => Mathf.RoundToInt(x)).ToArray();
    }
}

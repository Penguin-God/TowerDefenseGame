using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EnemyData
{

}

[Serializable]
public struct BossData
{
    [SerializeField] int level;
    [SerializeField] int hp;
    [SerializeField] int speed;

    [SerializeField] int goldReward;
    [SerializeField] int foodReward;
    UnitFlags flag; // null감지할 수 있게 구현 후 보상에 추가하기

    public int Level => level;
    public int Hp => hp;
    public int Speed => speed;
    public int Gold => goldReward;
    public int Food => foodReward;
}

public class BossDatas : ICsvLoader<int, BossData>
{
    public Dictionary<int, BossData> MakeDict(string csv)
        => CsvUtility.GetEnumerableFromCsv<BossData>(csv).ToDictionary(x => x.Level, x => x);
}

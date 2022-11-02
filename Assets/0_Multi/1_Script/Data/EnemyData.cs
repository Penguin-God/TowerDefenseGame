using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public struct NormalEnemyData
{
    [SerializeField] int _stage;
    int _number;
    [SerializeField] int _hp;
    [SerializeField] float _speed;

    public NormalEnemyData(int number, int hp, float speed)
    {
        _stage = 0;
        _number = number;
        _hp = hp;
        _speed = speed;
    }

    public void SetNumber(int _newNumber) => _number = _newNumber;

    public int Stage => _stage;
    public int Number => _number;
    public int Hp => _hp;
    public float Speed => _speed;
}

public class NormalEnemyDatas : ICsvLoader<int, NormalEnemyData>
{
    public Dictionary<int, NormalEnemyData> MakeDict(string csv)
        => CsvUtility.CsvToArray<NormalEnemyData>(csv).ToDictionary(x => x.Stage, x => x);
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
        => CsvUtility.CsvToArray<BossData>(csv).ToDictionary(x => x.Level, x => x);
}

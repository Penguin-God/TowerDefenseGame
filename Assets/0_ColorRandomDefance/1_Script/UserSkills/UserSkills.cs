﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public abstract class UserSkill
{
    public void SetInfo(SkillType skillType) => _skillType = skillType;
    SkillType _skillType;

    public abstract void InitSkill();
    protected float[] GetData() => Managers.ClientData.GetSkillLevelData(_skillType).BattleDatas;
}

public class UserSkillFactory
{
    Dictionary<SkillType, UserSkill> _typeBySkill = new Dictionary<SkillType, UserSkill>();

    public UserSkillFactory()
    {
        _typeBySkill.Add(SkillType.시작골드증가, new StartGold());
        _typeBySkill.Add(SkillType.시작고기증가, new StartFood());
        _typeBySkill.Add(SkillType.최대유닛증가, new MaxUnit());
        _typeBySkill.Add(SkillType.태극스킬, new Taegeuk());
        _typeBySkill.Add(SkillType.검은유닛강화, new BlackUnitUpgrade());
        _typeBySkill.Add(SkillType.노란기사강화, new YellowSowrdmanUpgrade());
        _typeBySkill.Add(SkillType.상대색깔변경, new ColorChange());
        _typeBySkill.Add(SkillType.판매보상증가, new SellUpgrade());
        _typeBySkill.Add(SkillType.보스데미지증가, new BossDamageUpgrade());
        _typeBySkill.Add(SkillType.고기혐오자, new FoodHater());
    }

    public UserSkill GetSkill(SkillType type)
    {
        _typeBySkill[type].SetInfo(type);
        return _typeBySkill[type];
    }
}

// ================= 스킬 세부 구현 =====================

public class StartGold : UserSkill
{
    public override void InitSkill()
        => Multi_GameManager.instance.AddGold((int)GetData()[0]);
}

public class StartFood : UserSkill
{
    public override void InitSkill()
        => Multi_GameManager.instance.AddFood((int)GetData()[0]);
}

public class MaxUnit : UserSkill
{
    public override void InitSkill()
        => Multi_GameManager.instance.BattleData.MaxUnit += (int)GetData()[0];
}

public class Taegeuk : UserSkill
{
    public event Action<UnitClass, bool> OnTaegeukDamageChanged;
    static readonly int UnitClassCount = Enum.GetValues(typeof(UnitClass)).Length;

    int[] _taegeukDamages = new int[UnitClassCount];
    int[] _originDamages = new int[UnitClassCount];

    public override void InitSkill()
    {
        Multi_UnitManager.Instance.OnUnitFlagCountChanged += (flag, count) => UseSkill(flag.UnitClass);
        EnrichDamagesData();

        void EnrichDamagesData()
        {
            _taegeukDamages = GetData().Select(x => (int)x).ToArray();

            for (int i = 0; i < _originDamages.Length; i++)
                _originDamages[i] = Managers.Data.GetUnitStat(new UnitFlags(0, i)).Damage;
        }
    }

    TaegeukConditionChecker _taegeukConditionChecker = new TaegeukConditionChecker();
    bool[] _currentTaegeukFlags = new bool[UnitClassCount];

    void UseSkill(UnitClass unitClass)
    {
        if (TaegeukUnitDamageChangeCondition(unitClass) == false) return;

        _currentTaegeukFlags[(int)unitClass] = _taegeukConditionChecker.GetTaegeukFlagByUnitClass(unitClass);
        ApplyUnitDamge(unitClass, _currentTaegeukFlags[(int)unitClass]);
        OnTaegeukDamageChanged?.Invoke(unitClass, _currentTaegeukFlags[(int)unitClass]);
    }

    bool TaegeukUnitDamageChangeCondition(UnitClass unitClass) // false => false만 아니면 true
        => _currentTaegeukFlags[(int)unitClass] != false || _taegeukConditionChecker.GetTaegeukFlagByUnitClass(unitClass) != false;

    void ApplyUnitDamge(UnitClass unitClass, bool isTaegeukConditionMet)
    {
        int applyDamage = isTaegeukConditionMet ? _taegeukDamages[(int)unitClass] : _originDamages[(int)unitClass];
        Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(UnitColor.Red, unitClass), applyDamage);
        Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(UnitColor.Blue, unitClass), applyDamage);
    }
}

public class TaegeukConditionChecker
{
    public bool GetTaegeukFlagByUnitClass(UnitClass unitClass)
        => GetCounts(UnitColor.Red)[(int)unitClass] >= 1 && GetCounts(UnitColor.Blue)[(int)unitClass] >= 1 && TaegeukOtherColorsCounts[(int)unitClass] == 0;

    int[] TaegeukOtherColorsCounts
    {
        get
        {
            int[] counts = new int[4];
            for (int i = 2; i < 6; i++)
                counts = counts.Zip(GetCounts((UnitColor)i), (a, b) => a + b).ToArray();
            return counts;
        }
    }

    int[] GetCounts(UnitColor unitColor) => new int[]
    {
        Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags((int)unitColor, 0)],
        Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags((int)unitColor, 1)],
        Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags((int)unitColor, 2)],
        Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags((int)unitColor, 3)],
    };
}

public class BlackUnitUpgrade : UserSkill
{
    public event Action<UnitFlags> OnBlackUnitReinforce;
    UnitDamages strongDamages;
    public override void InitSkill()
    {
        int[] datas = GetData().Select(x => (int)x).ToArray();
        strongDamages = new UnitDamages(datas[0], datas[1], datas[2], datas[3]);
        Multi_UnitManager.Instance.OnUnitFlagCountChanged += (flag, count) => UseSkill(flag);
    }

    void UseSkill(UnitFlags unitFlags)
    {
        if (unitFlags.UnitColor != UnitColor.Black) return;

        Debug.Assert(strongDamages.ArcherDamage == 100000, $"검은 궁수 버그 발현!! 버그난 대미지는 {strongDamages.ArcherDamage}");
        var flag = new UnitFlags(UnitColor.Black, unitFlags.UnitClass);
        Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, flag, strongDamages.Damages[(int)unitFlags.UnitClass]);
        OnBlackUnitReinforce?.Invoke(flag);
    }
}

public class YellowSowrdmanUpgrade : UserSkill
{
    public override void InitSkill()
    {
        // 노란 기사 패시브 골드 변경
        Multi_GameManager.instance.BattleData.YellowKnightRewardGold = (int)GetData()[0];
    }
}

public class ColorChange : UserSkill // 하얀 유닛을 뽑을 때 뽑은 직업과 같은 상대 유닛의 색깔을 다른 색깔로 변경
{
    readonly int MAX_SPAWN_COLOR_NUMBER = 6;
    int[] _whiteUnitCounts = new int[4];
    public event Action<byte, byte> OnUnitColorChanaged; // 변하기 전 색깔, 변한 후 색깔
    SkillColorChanger colorChanger;
    public override void InitSkill()
    {
        // 얘는 패시브로 기사 소환 범위도 늘어남
        Multi_GameManager.instance.BattleData.UnitSummonData.maxColorNumber = MAX_SPAWN_COLOR_NUMBER;
        Multi_UnitManager.Instance.OnUnitFlagCountChanged += UseSkill;
        colorChanger = Managers.Multi.Instantiater.PhotonInstantiate("RPCObjects/SkillColorChanger", Vector3.one * 500).GetComponent<SkillColorChanger>();
    }

    void UseSkill(UnitFlags flag, int newCount)
    {
        if (flag.UnitColor != UnitColor.White) return;

        if (UnitCountIncreased(flag, newCount))
            colorChanger.ColorChangeSkill(flag.UnitClass);
        _whiteUnitCounts[flag.ClassNumber] = newCount;
    }

    bool UnitCountIncreased(UnitFlags flag, int newCount) => newCount > _whiteUnitCounts[flag.ClassNumber];
}

public class FoodHater : UserSkill
{
    int _rate;
    public override void InitSkill()
    {
        _rate = (int)GetData()[0];
        ChangeShopCurrency();
        Multi_GameManager.instance.OnFoodChanged += FoodToGold;
    }

    void ChangeShopCurrency()
    {
        var battleData = Multi_GameManager.instance.BattleData;
        battleData.GetAllPriceDatas()
                .Where(x => x.CurrencyType == GameCurrencyType.Food)
                .ToList()
                .ForEach(x => x.ChangedCurrencyType(GameCurrencyType.Gold));

        battleData.WhiteUnitPriceRecord.PriceDatas.ToList().ForEach(x => x.ChangePrice(x.Price * _rate));
        battleData.MaxUnitIncreaseRecord.ChangePrice(battleData.MaxUnitIncreaseRecord.Price * _rate);
    }

    void FoodToGold(int food)
    {
        if (food <= 0) return;

        if (Multi_GameManager.instance.TryUseFood(food))
            Multi_GameManager.instance.AddGold(food * _rate);
    }
}

public class SellUpgrade : UserSkill
{
    public override void InitSkill()
    {
        // 유닛 판매 보상 증가 (유닛별로 증가폭 별도)
        int[] sellData = GetData().Select(x => (int)x).ToArray();
        var sellRewardDatas = Multi_GameManager.instance.BattleData.UnitSellPriceRecord.PriceDatas;
        for (int i = 0; i < sellRewardDatas.Length; i++)
            sellRewardDatas[i].ChangePrice(sellData[i]);
    }
}

public class BossDamageUpgrade : UserSkill
{
    public override void InitSkill()
    {
        float rate = GetData()[0];
        Multi_SpawnManagers.NormalUnit.OnSpawn += (unit) => unit.BossDamage = Mathf.RoundToInt(unit.BossDamage * rate);
    }
}

public struct UnitDamages
{
    public UnitDamages(int sword, int archer, int spear, int mage)
    {
        _swordmanDamage = sword;
        _archerDamage = archer;
        _spearmanDamage = spear;
        _mageDamage = mage;
    }

    int _swordmanDamage;
    int _archerDamage;
    int _spearmanDamage;
    int _mageDamage;

    public int SwordmanDamage => _swordmanDamage;
    public int ArcherDamage => _archerDamage;
    public int SpearmanDamage => _spearmanDamage;
    public int MageDamage => _mageDamage;
    public int[] Damages => new int[] { SwordmanDamage, ArcherDamage, SpearmanDamage, MageDamage };
}
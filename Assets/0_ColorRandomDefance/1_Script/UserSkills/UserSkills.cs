using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public abstract class UserSkill
{
    public void SetInfo(SkillType skillType) => _skillType = skillType;
    SkillType _skillType;

    public abstract void InitSkill();
    protected float[] SkillDatas => Managers.ClientData.GetSkillLevelData(_skillType).BattleDatas;
    protected float SkillData => SkillDatas[0];
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
        _typeBySkill.Add(SkillType.컬러마스터, new ColorMaster());
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
        => Multi_GameManager.Instance.AddGold((int)SkillData);
}

public class StartFood : UserSkill
{
    public override void InitSkill()
        => Multi_GameManager.Instance.AddFood((int)SkillData);
}

public class MaxUnit : UserSkill
{
    public override void InitSkill()
        => Multi_GameManager.Instance.BattleData.MaxUnit += (int)SkillData;
}

public class Taegeuk : UserSkill
{
    enum TaegeukStateChangeType
    {
        NoChange,
        AddNewTaegeukUnit,
        TrueToFalse,
        FalseToTrue,
    }

    public event Action<UnitClass, bool> OnTaegeukDamageChanged;
    static readonly int UnitClassCount = Enum.GetValues(typeof(UnitClass)).Length;

    int[] _taegeukDamages = new int[UnitClassCount];
    public override void InitSkill()
    {
        Multi_SpawnManagers.NormalUnit.OnSpawn += CheckAndApplyTaegeuk;
        _taegeukDamages = SkillDatas.Select(x => (int)x).ToArray();
    }

    bool[] _currentTaegeukFlags = new bool[UnitClassCount];

    void CheckAndApplyTaegeuk(Multi_TeamSoldier unit)
    {
        var stateChange = GetTaegeukStateChangeType(unit.UnitClass);
        var unitClass = unit.UnitClass;
        switch (stateChange)
        {
            case TaegeukStateChangeType.NoChange:
                return;
            case TaegeukStateChangeType.TrueToFalse:
                ApplyChangeTaeguekState(unitClass, false); break;
            case TaegeukStateChangeType.FalseToTrue:
                ApplyChangeTaeguekState(unitClass, true); break;
        }

        OnTaegeukDamageChanged?.Invoke(unitClass, _currentTaegeukFlags[(int)unitClass]);
    }

    TaegeukStateChangeType GetTaegeukStateChangeType(UnitClass unitClass)
    {
        bool prevTaegeukFlag = _currentTaegeukFlags[(int)unitClass];
        bool newTaegeukFlag = new TaegeukConditionChecker().CheckTaegeuk(unitClass, Multi_UnitManager.Instance.ExsitUnitFlags);

        if (prevTaegeukFlag && newTaegeukFlag)
            return TaegeukStateChangeType.AddNewTaegeukUnit;
        else if (prevTaegeukFlag && newTaegeukFlag == false)
            return TaegeukStateChangeType.TrueToFalse;
        else if (prevTaegeukFlag == false && newTaegeukFlag)
            return TaegeukStateChangeType.FalseToTrue;
        else
            return TaegeukStateChangeType.NoChange;
    }

    void ApplyChangeTaeguekState(UnitClass unitClass, bool newFlag)
    {
        _currentTaegeukFlags[(int)unitClass] = newFlag;
        ApplyTaegeukToUnit(unitClass, _currentTaegeukFlags[(int)unitClass]);
    }

    void ApplyTaegeukToUnit(UnitClass unitClass, bool isTaegeukConditionMet)
    {
        int applyDamage = _taegeukDamages[(int)unitClass] * (isTaegeukConditionMet ? 1 : -1);
        SetTaeguekUnitStat(UnitColor.Red);
        SetTaeguekUnitStat(UnitColor.Blue);

        void SetTaeguekUnitStat(UnitColor unitColor)
            => MultiServiceMidiator.UnitUpgrade.AddUnitDamageValue(new UnitFlags(unitColor, unitClass), applyDamage, UnitStatType.All);
    }
}

public class TaegeukConditionChecker
{
    public bool CheckTaegeuk(UnitClass unitClass, HashSet<UnitFlags> existUnitFlags)
        => ExistRedAndBlue(unitClass, existUnitFlags) && CountZeroTaegeukOther(unitClass, existUnitFlags);

    bool ExistRedAndBlue(UnitClass unitClass, HashSet<UnitFlags> existUnitFlags)
        => existUnitFlags.Contains(new UnitFlags(UnitColor.Red, unitClass)) && existUnitFlags.Contains(new UnitFlags(UnitColor.Blue, unitClass));

    bool CountZeroTaegeukOther(UnitClass unitClass, HashSet<UnitFlags> existUnitFlags)
    {
        var otherColors = new UnitColor[] { UnitColor.Yellow, UnitColor.Green, UnitColor.Orange, UnitColor.Violet };
        return !otherColors.Any(color => existUnitFlags.Contains(new UnitFlags(color, unitClass)));
    }
}

public class BlackUnitUpgrade : UserSkill
{
    public event Action<UnitFlags> OnBlackUnitReinforce;
    UnitDamages strongDamages;
    public override void InitSkill()
    {
        int[] datas = SkillDatas.Select(x => (int)x).ToArray();
        strongDamages = new UnitDamages(datas[0], datas[1], datas[2], datas[3]);
        Multi_UnitManager.Instance.OnUnitCountChangeByFlag += UseSkill;
    }

    void UseSkill(UnitFlags unitFlags, int count)
    {
        if (unitFlags.UnitColor != UnitColor.Black && count == 0) return;

        var flag = new UnitFlags(UnitColor.Black, unitFlags.UnitClass);
        MultiServiceMidiator.UnitUpgrade.AddUnitDamageValue(flag, strongDamages.Damages[(int)unitFlags.UnitClass], UnitStatType.All);
        OnBlackUnitReinforce?.Invoke(flag);
    }
}

public class YellowSowrdmanUpgrade : UserSkill
{
    // 노란 기사 패시브 골드 변경
    public override void InitSkill()
        => Multi_GameManager.Instance.BattleData.YellowKnightRewardGold = (int)SkillData;
}

public class ColorMaster : UserSkill
{
    readonly int MAX_SPAWN_COLOR_NUMBER = 6;
    public override void InitSkill()
        => Multi_GameManager.Instance.BattleData.UnitSummonData.maxColorNumber = MAX_SPAWN_COLOR_NUMBER;
}

public class ColorChange : UserSkill // 하얀 유닛을 뽑을 때 뽑은 직업과 같은 상대 유닛의 색깔을 다른 색깔로 변경
{
    int[] _whiteUnitCounts = new int[4];
    public event Action<byte, byte> OnUnitColorChanaged; // 변하기 전 색깔, 변한 후 색깔
    SkillColorChanger colorChanger;
    public override void InitSkill()
    {
        Multi_UnitManager.Instance.OnUnitCountChangeByFlag += UseSkill;
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
        _rate = (int)SkillData;
        ChangeShopCurrency();
        Multi_GameManager.Instance.OnFoodChanged += FoodToGold;
    }

    void ChangeShopCurrency()
    {
        var battleData = Multi_GameManager.Instance.BattleData;
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

        if (Multi_GameManager.Instance.TryUseFood(food))
            Multi_GameManager.Instance.AddGold(food * _rate);
    }
}

public class SellUpgrade : UserSkill
{
    public override void InitSkill()
    {
        // 유닛 판매 보상 증가 (유닛별로 증가폭 별도)
        int[] sellData = SkillDatas.Select(x => (int)x).ToArray();
        var sellRewardDatas = Multi_GameManager.Instance.BattleData.UnitSellPriceRecord.PriceDatas;
        for (int i = 0; i < sellRewardDatas.Length; i++)
            sellRewardDatas[i].ChangePrice(sellData[i]);
    }
}

public class BossDamageUpgrade : UserSkill
{
    public override void InitSkill() => MultiServiceMidiator.UnitUpgrade.ScaleUnitDamageValue(SkillData, UnitStatType.BossDamage);
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
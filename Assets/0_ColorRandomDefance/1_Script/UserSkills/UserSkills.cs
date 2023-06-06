using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public abstract class UserSkill
{
    // public UserSkill(SkillType skillType) => _skillType = skillType;
    public void SetInfo(SkillType skillType) => _skillType = skillType;
    SkillType _skillType;

    public abstract void InitSkill();
    float[] SkillDatas => Managers.ClientData.GetSkillLevelData(_skillType).BattleDatas;
    protected int[] IntSkillDatas => SkillDatas.Select(x => (int)x).ToArray();
    protected float SkillData => SkillDatas[0];
    protected int IntSkillData => (int)SkillData;
}

public class UserSkillFactory
{
    Dictionary<SkillType, UserSkill> _typeBySkill = new Dictionary<SkillType, UserSkill>();

    public static UserSkill CreateUserSkill(SkillType skillType, BattleDIContainer container)
    {
        switch (skillType)
        {
            case SkillType.시작골드증가: return new StartGold();
            case SkillType.시작고기증가: return new StartFood();
            case SkillType.최대유닛증가: return new MaxUnit();
            case SkillType.태극스킬: return new Taegeuk();
            case SkillType.검은유닛강화: return new BlackUnitUpgrade();
            case SkillType.노란기사강화: return new YellowSowrdmanUpgrade();
            case SkillType.컬러마스터: return new ColorMaster(container.GetService<SwordmanGachaController>());
            case SkillType.상대색깔변경: return new ColorChange();
            case SkillType.고기혐오자: return new FoodHater();
            case SkillType.판매보상증가: return new SellUpgrade();
            case SkillType.보스데미지증가: return new BossDamageUpgrade();
            case SkillType.장사꾼: return new DiscountMerchant();
            default: return null;
        }
    }

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
        _typeBySkill.Add(SkillType.장사꾼, new DiscountMerchant());
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
        => Multi_GameManager.Instance.AddGold(IntSkillData);
}

public class StartFood : UserSkill
{
    public override void InitSkill()
        => Multi_GameManager.Instance.AddFood(IntSkillData);
}

public class MaxUnit : UserSkill
{
    public override void InitSkill()
        => Multi_GameManager.Instance.IncreasedMaxUnitCount(IntSkillData);
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
        Managers.Unit.OnUnitCountChangeByFlag += (flag, count) => CheckAndApplyTaegeuk(flag);
        _taegeukDamages = IntSkillDatas;
    }

    bool[] _currentTaegeukFlags = new bool[UnitClassCount];

    void CheckAndApplyTaegeuk(UnitFlags unitFlag)
    {
        var unitClass = unitFlag.UnitClass;
        var stateChange = GetTaegeukStateChangeType(unitClass);
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
        bool newTaegeukFlag = new TaegeukConditionChecker().CheckTaegeuk(unitClass, Managers.Unit.ExsitUnitFlags);
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
    int[] _upgradeDamages;
    public override void InitSkill()
    {
        _upgradeDamages = IntSkillDatas;
        Managers.Unit.OnUnitCountChangeByFlag += UseSkill;
    }

    void UseSkill(UnitFlags unitFlags, int count)
    {
        if (unitFlags.UnitColor != UnitColor.Black && count == 0) return;

        var flag = new UnitFlags(UnitColor.Black, unitFlags.UnitClass);
        MultiServiceMidiator.UnitUpgrade.AddUnitDamageValue(flag, _upgradeDamages[(int)unitFlags.UnitClass], UnitStatType.All);
        OnBlackUnitReinforce?.Invoke(flag);
    }
}

public class YellowSowrdmanUpgrade : UserSkill
{
    // 노란 기사 패시브 골드 변경
    public override void InitSkill()
        => Multi_GameManager.Instance.BattleData.YellowKnightRewardGold = IntSkillData;
}

public class ColorMaster : UserSkill
{
    SwordmanGachaController _swordmanGachaController;
    public ColorMaster(SwordmanGachaController swordmanGachaController) => _swordmanGachaController = swordmanGachaController;
    public override void InitSkill() => _swordmanGachaController.ChangeUnitSummonMaxColor(UnitColor.Violet);
}

public class ColorChange : UserSkill // 하얀 유닛을 뽑을 때 뽑은 직업과 같은 상대 유닛의 색깔을 다른 색깔로 변경
{
    int[] _whiteUnitCounts = new int[4];
    public event Action<byte, byte> OnUnitColorChanaged; // 변하기 전 색깔, 변한 후 색깔
    SkillColorChanger colorChanger;
    public override void InitSkill()
    {
        Managers.Unit.OnUnitCountChangeByFlag += UseSkill;
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
    int _rewardRate; // 얻는 고기가 몇 골드로 바뀌는가
    int _priceRate; // 기존에 고기로 팔던 상품을 몇 배의 골드로 바꿀건가
    public override void InitSkill()
    {
        _rewardRate = IntSkillDatas[0];
        _priceRate = IntSkillDatas[1];
        Multi_GameManager.Instance.OnFoodChanged += FoodToGold;
        ChangeShopPriceData();
    }

    void FoodToGold(int food)
    {
        if (food <= 0) return;

        if (Multi_GameManager.Instance.TryUseFood(food))
            Multi_GameManager.Instance.AddGold(food * _rewardRate);
    }

    void ChangeShopPriceData()
    {
        Multi_GameManager.Instance.BattleData.GetAllShopPriceDatas()
                .Where(x => x.CurrencyType == GameCurrencyType.Food)
                .ToList()
                .ForEach(FoodDataToGoldData);
    }

    void FoodDataToGoldData(CurrencyData priceData)
    {
        priceData.ChangedCurrencyType(GameCurrencyType.Gold);
        priceData.ChangeAmount(priceData.Amount * _priceRate);
    }
}

public class SellUpgrade : UserSkill
{
    public override void InitSkill()
    {
        // 유닛 판매 보상 증가 (유닛별로 증가폭 별도)
        var sellRewardDatas = Multi_GameManager.Instance.BattleData.UnitSellRewardDatas;
        for (int i = 0; i < sellRewardDatas.Count; i++)
            sellRewardDatas[i].ChangeAmount(IntSkillDatas[i]);
    }
}

public class BossDamageUpgrade : UserSkill
{
    public override void InitSkill() => MultiServiceMidiator.UnitUpgrade.ScaleUnitDamageValue(SkillData, UnitStatType.BossDamage);
}

public class DiscountMerchant : UserSkill
{
    public override void InitSkill()
    {
        Multi_GameManager.Instance.BattleData
            .ShopPriceDataByUnitUpgradeData
            .Where(x => x.Key.UpgradeType == UnitUpgradeType.Value)
            .Select(x => x.Value)
            .ToList()
            .ForEach(x => x.ChangeAmount(0));
        Multi_GameManager.Instance.BattleData.UnitUpgradeShopData.ResetPrice = IntSkillData;
    }
}

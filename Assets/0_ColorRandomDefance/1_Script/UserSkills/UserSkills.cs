using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public abstract class UserSkill
{
    SkillType _skillType;
    public UserSkill(SkillType skillType) => _skillType = skillType;

    public abstract void InitSkill();
    protected float[] SkillDatas => Managers.ClientData.GetSkillLevelData(_skillType).BattleDatas;
    protected int[] IntSkillDatas => SkillDatas.Select(x => (int)x).ToArray();
    protected float SkillData => SkillDatas[0];
    protected int IntSkillData => (int)SkillData;
}

public class UserSkillFactory
{
    public static UserSkill CreateUserSkill(SkillType skillType, BattleDIContainer container)
    {
        switch (skillType)
        {
            case SkillType.시작골드증가: return new StartGold(skillType);
            case SkillType.시작고기증가: return new StartFood(skillType);
            case SkillType.최대유닛증가: return new MaxUnit(skillType);
            case SkillType.태극스킬: return new TaegeukController(skillType);
            case SkillType.검은유닛강화: return new BlackUnitUpgrade(skillType);
            case SkillType.노란기사강화: return new YellowSowrdmanUpgrade(skillType);
            case SkillType.컬러마스터: return new ColorMaster(skillType, container.GetComponent<SwordmanGachaController>());
            case SkillType.상대색깔변경: return new ColorChange(skillType);
            case SkillType.고기혐오자: return new FoodHater(skillType);
            case SkillType.판매보상증가: return new SellUpgrade(skillType);
            case SkillType.보스데미지증가: return new BossDamageUpgrade(skillType);
            case SkillType.장사꾼: return new DiscountMerchant(skillType);
            case SkillType.조합메테오: return new CombineMeteorController(skillType, container.GetComponent<MeteorController>(), container.GetComponent<IMonsterManager>());
            case SkillType.네크로맨서: 
                return new NecromancerController(skillType, container.GetService<BattleEventDispatcher>(), container.GetComponent<EffectSynchronizer>());
            case SkillType.마창사: return new MagicSpearman(skillType, Managers.Data);
            default: return null;
        }
    }
}

// ================= 스킬 세부 구현 =====================

public class StartGold : UserSkill
{
    public StartGold(SkillType skillType) : base(skillType) { }
    public override void InitSkill() => Multi_GameManager.Instance.AddGold(IntSkillData);
}

public class StartFood : UserSkill
{
    public StartFood(SkillType skillType) : base(skillType) { }
    public override void InitSkill() => Multi_GameManager.Instance.AddFood(IntSkillData);
}

public class MaxUnit : UserSkill
{
    public MaxUnit(SkillType skillType) : base(skillType) { }
    public override void InitSkill() => Multi_GameManager.Instance.IncreasedMaxUnitCount(IntSkillData);
}

public class TaegeukController : UserSkill
{
    public TaegeukController(SkillType skillType) : base(skillType) { }

    public event Action<UnitClass, bool> OnTaegeukDamageChanged;

    int[] _taegeukDamages = new int[Enum.GetValues(typeof(UnitClass)).Length];
    public override void InitSkill()
    {
        Managers.Unit.OnUnitCountChangeByFlag += (flag, count) => CheckAndApplyTaegeuk(flag);
        _taegeukDamages = IntSkillDatas;
    }

    readonly TaegeukStateManager _taegeukStateManager = new TaegeukStateManager();
    void CheckAndApplyTaegeuk(UnitFlags unitFlag)
    {
        var unitClass = unitFlag.UnitClass;
        var taeguekState = _taegeukStateManager.GetTaegeukState(unitClass, Managers.Unit.ExsitUnitFlags);
        if (taeguekState.ChangeState == TaegeukStateChangeType.NoChange) return;
        else if(taeguekState.ChangeState == TaegeukStateChangeType.AddNewUnit)
        {
            OnTaegeukDamageChanged?.Invoke(unitClass, taeguekState.IsActive);
            return;
        }

        ApplyTaegeukToUnit(unitClass, taeguekState.IsActive);
        OnTaegeukDamageChanged?.Invoke(unitClass, taeguekState.IsActive);
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

public class BlackUnitUpgrade : UserSkill
{
    public BlackUnitUpgrade(SkillType skillType) : base(skillType) { }

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
    public YellowSowrdmanUpgrade(SkillType skillType) : base(skillType) { }
    // 노란 기사 패시브 골드 변경
    public override void InitSkill()
        => Multi_GameManager.Instance.BattleData.YellowKnightRewardGold = IntSkillData;
}

public class ColorMaster : UserSkill
{
    SwordmanGachaController _swordmanGachaController;
    public ColorMaster(SkillType skillType, SwordmanGachaController swordmanGachaController) : base(skillType)
        => _swordmanGachaController = swordmanGachaController;
    public override void InitSkill() => _swordmanGachaController.ChangeUnitSummonMaxColor(UnitColor.Violet);
}

public class ColorChange : UserSkill // 하얀 유닛을 뽑을 때 뽑은 직업과 같은 상대 유닛의 색깔을 다른 색깔로 변경
{
    public ColorChange(SkillType skillType) : base(skillType) { }

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
    public FoodHater(SkillType skillType) : base(skillType) { }
    int _rewardRate; // 얻는 고기가 몇 골드로 바뀌는가
    int _priceRate; // 기존에 고기로 팔던 상품을 몇 배의 골드로 바꿀건가
    Multi_GameManager _game;
    public override void InitSkill()
    {
        _rewardRate = IntSkillDatas[0];
        _priceRate = IntSkillDatas[1];
        _game = Multi_GameManager.Instance;

        _game.OnFoodChanged += FoodToGold;
        if (_game.CurrencyManager.Food > 0)
            FoodToGold(_game.CurrencyManager.Food);
        ChangeShopPriceData();
    }

    void FoodToGold(int food)
    {
        if (0 >= food) return;

        if (_game.TryUseFood(food))
            _game.AddGold(food * _rewardRate);
    }

    void ChangeShopPriceData()
    {
        _game.BattleData.GetAllShopPriceDatas()
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
    public SellUpgrade(SkillType skillType) : base(skillType) { }
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
    public BossDamageUpgrade(SkillType skillType) : base(skillType) { }
    public override void InitSkill() => MultiServiceMidiator.UnitUpgrade.ScaleUnitDamageValue(SkillData, UnitStatType.BossDamage);
}

public class DiscountMerchant : UserSkill
{
    public DiscountMerchant(SkillType skillType) : base(skillType) { }
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

public class CombineMeteorController : UserSkill
{
    MeteorController _meteorController;
    readonly Vector3 MeteorShotPoint;
    IMonsterManager _monsterManager;
    public CombineMeteorController(SkillType skillType, MeteorController meteorController, IMonsterManager monsterManager) : base(skillType) 
    {
        _monsterManager = monsterManager;
        MeteorShotPoint = PlayerIdManager.Id == PlayerIdManager.MasterId ? new Vector3(0, 30, 0) : new Vector3(0, 30, 500);
        _meteorController = meteorController;
        _attack = IntSkillDatas[0];
        _stunTime = SkillDatas[1];
    }

    int _attack;
    float _stunTime;
    
    public override void InitSkill()
    {
        Managers.Unit.OnCombine += ShotMeteor;
    }

    void ShotMeteor(UnitFlags combineUnitFlag)
    {
        int score = CalculateRedScore(combineUnitFlag);
        if (score > 0)
            _meteorController.ShotMeteor(FindMonster(), _attack * score, _stunTime * score, MeteorShotPoint);
    }
    Multi_NormalEnemy FindMonster()
    {
        var monsters = _monsterManager.GetNormalMonsters();
        if (monsters.Count == 0) return null;
        else return monsters[UnityEngine.Random.Range(0, monsters.Count)];
    }

    int CalculateRedScore(UnitFlags combineUnitFlag)
    {
        int result = 0;
        foreach(var flag in new UnitCombineSystem(Managers.Data.CombineConditionByUnitFalg).GetNeedFlags(combineUnitFlag))
        {
            if(flag.UnitColor == UnitColor.Red)
                result += GetClassScore(flag.UnitClass);
        }
        return result;
    }

    int GetClassScore(UnitClass unitClass)
    {
        switch (unitClass)
        {
            case UnitClass.Swordman: return 1;
            case UnitClass.Archer: return 4;
            case UnitClass.Spearman: return 20;
            case UnitClass.Mage: return 0;
            default: return 0;
        }
    }
}

public class NecromancerController : UserSkill
{
    readonly Necromencer _necromencer;
    readonly EffectSynchronizer _effectSynchronizer;
    BattleEventDispatcher _dispatcher;
    public NecromancerController(SkillType skillType, BattleEventDispatcher dispatcher, EffectSynchronizer effectSynchronizer) : base(skillType)
    {
        _necromencer = new Necromencer(IntSkillData);
        _dispatcher = dispatcher;
        _effectSynchronizer = effectSynchronizer;
    }

    UI_UserSkillStatus statusUI;
    public override void InitSkill()
    {
        _dispatcher.OnNormalMonsterDead += _ => ResurrectOnKillCount();
        statusUI = Managers.UI.ShowSceneUI<UI_UserSkillStatus>();
        statusUI.Injction(_necromencer);
    }

    readonly Vector3 EffectOffst = new Vector3(0, 0.6f, 0);
    readonly UnitFlags ResurrectUnit = new UnitFlags(UnitColor.Violet, UnitClass.Swordman);
    void ResurrectOnKillCount()
    {
        if (_necromencer.TryResurrect())
        {
            var spawnPos = new WorldSpawnPositionCalculator(20, 0, 0, 0).CalculateWorldPostion(Multi_Data.instance.GetWorldPosition(PlayerIdManager.Id));
            Multi_SpawnManagers.NormalUnit.Spawn(ResurrectUnit, spawnPos);
            _effectSynchronizer.PlayOneShotEffect("PosionMagicCircle", spawnPos + EffectOffst);
            Managers.Sound.PlayEffect(EffectSoundType.YellowMageSkill);
        }
        statusUI.UpdateKillCount();
    }
}

public class SlowTrapSpawner : UserSkill
{
    readonly MonsterPathLocationFinder _locationFinder;
    BattleEventDispatcher _dispatcher;
    public SlowTrapSpawner(SkillType skillType, Transform[] wayPoints, BattleEventDispatcher dispatcher) : base(skillType)
    {
        _locationFinder = new MonsterPathLocationFinder(wayPoints.Select(x => x.position).ToArray());
        _dispatcher = dispatcher;
    }

    public override void InitSkill()
    {
        _dispatcher.OnStageUp += _ => SpawnTrap();
    }

    const int SpawnCount = 2;
    void SpawnTrap()
    {
        for (int i = 0; i < SpawnCount; i++)
        {
            // spawn, _locationFinder.CalculateMonsterPathLocation();
        }
    }
}

public class MagicSpearman : UserSkill
{
    readonly DataManager _dataManager;
    public MagicSpearman(SkillType skillType, DataManager data) : base(skillType) => _dataManager = data;
    public override void InitSkill()
        => _dataManager.Unit.SetThrowSpearData(Managers.Resources.Load<ThrowSpearDataContainer>("Data/ScriptableObject/MagicThrowSpearData").ChangeAttackRate(SkillData));
}
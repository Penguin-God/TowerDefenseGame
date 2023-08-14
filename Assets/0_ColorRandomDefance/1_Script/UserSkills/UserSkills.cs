using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.ObjectModel;

public class SkillBattleDataContainer
{
    public UserSkillBattleData MainSkill { get; private set; }
    public UserSkillBattleData SubSkill{ get; private set; }
    readonly Dictionary<UserSkillClass, UserSkillBattleData> BattleDataBySKillType = new Dictionary<UserSkillClass, UserSkillBattleData>();
    public IEnumerable<SkillType> AllSKills => BattleDataBySKillType.Values.Select(x => x.SkillType);

    public void ChangeEquipSkill(UserSkillBattleData newData)
    {
        BattleDataBySKillType[newData.SkillClass] = newData;
        switch (newData.SkillClass)
        {
            case UserSkillClass.Main: MainSkill = newData; break;
            case UserSkillClass.Sub: SubSkill = newData; break;
        }
    }

    public bool TruGetSkillData(SkillType skill, out UserSkillBattleData result)
    {
        if (ContainSKill(skill))
        {
            result = BattleDataBySKillType.Values.Where(x => x.SkillType == skill).FirstOrDefault();
            return true;
        }
        else
        {
            result = new UserSkillBattleData();
            return false;
        }
    }
    public bool ContainSKill(SkillType skill) => BattleDataBySKillType.Values.Where(x => x.SkillType == skill).Count() > 0;
}

public static class BattleSkillDataCreater
{
    public static SkillBattleDataContainer CreateSkillData(ClientDataManager client, DataManager data)
    {
        var main = Managers.ClientData.EquipSkillManager.MainSkill;
        var sub = Managers.ClientData.EquipSkillManager.SubSkill;
        return CreateSkillData(main, client.GetSkillLevel(main), sub, client.GetSkillLevel(sub), data.UserSkill);
    }

    public static SkillBattleDataContainer CreateSkillData(SkillType mainSkill, int mainLevel, SkillType subSkill, int subLevel, DataManager.UserSkillData data)
    {
        var result = new SkillBattleDataContainer();
        if (mainSkill == SkillType.None || subSkill == SkillType.None) return result;
        result.ChangeEquipSkill(data.GetSkillBattleData(mainSkill, mainLevel));
        result.ChangeEquipSkill(data.GetSkillBattleData(subSkill, subLevel));
        return result;
    }
}

public abstract class UserSkill
{
    public UserSkillBattleData UserSkillBattleData { get; private set; }
    public UserSkill(UserSkillBattleData userSkillBattleData) => UserSkillBattleData = userSkillBattleData;

    internal abstract void InitSkill();

    protected int[] IntSkillDatas => UserSkillBattleData.IntSkillDatas;
    protected float[] SkillDatas => UserSkillBattleData.SkillDatas.ToArray();
    protected int IntSkillData => UserSkillBattleData.IntSkillData;
}

public struct UserSkillBattleData
{
    public readonly SkillType SkillType;
    public readonly UserSkillClass SkillClass;
    public readonly IReadOnlyList<float> SkillDatas;
    public float SkillData => SkillDatas[0];
    public int[] IntSkillDatas => SkillDatas.Select(x => (int)x).ToArray();
    public int IntSkillData => (int)SkillData;

    public UserSkillBattleData(SkillType skillType, UserSkillClass skillClass, IReadOnlyList<float> skillDatas)
    {
        SkillType = skillType;
        SkillClass = skillClass;
        SkillDatas = skillDatas;
    }
}

public class UserSkillFactory
{
    IReadOnlyList<SkillType> SimpleSkills = new ReadOnlyCollection<SkillType>(new List<SkillType>() 
    {
        SkillType.시작골드증가, SkillType.마나물약, SkillType.최대유닛증가, SkillType.황금빛기사, SkillType.컬러마스터, SkillType.거인학살자,
        // SkillType.흑의결속,
    });

    IReadOnlyList<SkillType> ComplexSkills = new ReadOnlyCollection<SkillType>(new List<SkillType>()
    {
        SkillType.태극스킬, SkillType.마나변이, SkillType.마나불능, SkillType.장사꾼, SkillType.도박사, SkillType.메테오,
        SkillType.네크로맨서, SkillType.덫, SkillType.백의결속, SkillType.흑의결속,
    });

    IReadOnlyList<SkillType> ExistSkills = new ReadOnlyCollection<SkillType>(new List<SkillType>()
    {
        SkillType.마창사, SkillType.썬콜 // 존재하기만 하면 알아서 작동하는 스킬들
    });

    public UserSkill ActiveSkill(SkillType skillType, BattleDIContainer container)
    {
        if (ExistSkills.Contains(skillType)) return null;

        UserSkillBattleData skillBattleData = Managers.Data.UserSkill.GetSkillBattleData(skillType, 1);
        if (SimpleSkills.Contains(skillType))
        {
            ActiveSimpleSkill(skillBattleData, container);
            return null;
        }
        else if(ComplexSkills.Contains(skillType))
            return ActiveComplexSkill(skillBattleData, container);
        Debug.LogError($"정의되지 않은 스킬 : {skillType}을 사용하려고 함");
        return null;
    }

    void ActiveSimpleSkill(UserSkillBattleData skillBattleData, BattleDIContainer container)
    {
        switch (skillBattleData.SkillType)
        {
            case SkillType.시작골드증가: Multi_GameManager.Instance.AddGold(skillBattleData.IntSkillData); break;
            case SkillType.마나물약: Multi_GameManager.Instance.AddFood(skillBattleData.IntSkillData); break;
            case SkillType.최대유닛증가: Multi_GameManager.Instance.IncreasedMaxUnitCount(skillBattleData.IntSkillData); break;
            case SkillType.황금빛기사: Multi_GameManager.Instance.BattleData.YellowKnightRewardGold = skillBattleData.IntSkillData; break;
            case SkillType.컬러마스터: container.GetComponent<SwordmanGachaController>().ChangeUnitSummonMaxColor(UnitColor.Violet); break;
            case SkillType.거인학살자: MultiServiceMidiator.UnitUpgrade.ScaleUnitDamageValue(skillBattleData.SkillData, UnitStatType.BossDamage); break;
            // case SkillType.흑의결속: new UnitUpgradeHandler().UpgradeUnit(UnitColor.Black, skillBattleData.IntSkillDatas); break;
        }
    }

    UserSkill ActiveComplexSkill(UserSkillBattleData skillBattleData, BattleDIContainer container)
    {
        UserSkill result;
        switch (skillBattleData.SkillType)
        {
            case SkillType.태극스킬: result = new TaegeukController(skillBattleData); break;
            case SkillType.흑의결속: result = new BlackUnitUpgrade(skillBattleData); break;
            case SkillType.마나변이: result = new ManaMutation(skillBattleData, container.GetComponent<SkillColorChanger>()); break;
            case SkillType.마나불능: result = new ManaImpotence(skillBattleData); break;
            case SkillType.장사꾼: result = new UnitMerchant(skillBattleData); break;
            case SkillType.도박사: result = new GamblerController(skillBattleData, container.GetService<BattleUI_Mediator>()); break;
            case SkillType.메테오: result = new CombineMeteorController(skillBattleData, container.GetComponent<MeteorController>(), container.GetComponent<IMonsterManager>()); break;
            case SkillType.네크로맨서:
                result = new NecromancerController(skillBattleData, container.GetEventDispatcher(), container.GetComponent<MultiEffectManager>()); break;
            case SkillType.덫:
                result = new SlowTrapSpawner(skillBattleData, Multi_Data.instance.GetEnemyTurnPoints(PlayerIdManager.Id) ,container.GetEventDispatcher()); break;
            case SkillType.백의결속: result = new BondofWhite(skillBattleData, container.GetEventDispatcher(), Multi_GameManager.Instance); break;
            default: result = null; break;
        }
        result.InitSkill();
        return result;
    }
}

// ================= 스킬 세부 구현 =====================

public class TaegeukController : UserSkill
{
    public TaegeukController(UserSkillBattleData userSkillBattleData) : base(userSkillBattleData) { }

    public event Action<UnitClass, bool> OnTaegeukDamageChanged;

    int[] _taegeukDamages = new int[Enum.GetValues(typeof(UnitClass)).Length];
    internal override void InitSkill()
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
    public BlackUnitUpgrade(UserSkillBattleData userSkillBattleData) : base(userSkillBattleData) { }
    internal override void InitSkill() => new UnitUpgradeHandler().UpgradeUnit(UnitColor.Black, IntSkillDatas);
}

public class ManaMutation : UserSkill // 하얀 유닛을 뽑을 때 뽑은 직업과 같은 상대 유닛의 색깔을 다른 색깔로 변경
{
    int[] _whiteUnitCounts = new int[4];
    public event Action<byte, byte> OnUnitColorChanaged; // 변하기 전 색깔, 변한 후 색깔
    readonly SkillColorChanger _colorChanger;

    public ManaMutation(UserSkillBattleData userSkillBattleData, SkillColorChanger colorChanger) : base(userSkillBattleData)
        => _colorChanger = colorChanger;

    internal override void InitSkill()
    {
        Managers.Unit.OnUnitCountChangeByFlag += UseSkill;
    }

    void UseSkill(UnitFlags flag, int newCount)
    {
        if (flag.UnitColor != UnitColor.White) return;

        if (UnitCountIncreased(flag, newCount))
            _colorChanger.ColorChangeSkill(flag.UnitClass);
        _whiteUnitCounts[flag.ClassNumber] = newCount;
    }

    bool UnitCountIncreased(UnitFlags flag, int newCount) => newCount > _whiteUnitCounts[flag.ClassNumber];
}

public class ManaImpotence : UserSkill
{
    public ManaImpotence(UserSkillBattleData userSkillBattleData) : base(userSkillBattleData) { }

    int _rewardRate; // 얻는 룬이 몇 골드로 바뀌는가
    int _priceRate; // 기존에 룬으로 팔던 상품을 몇 배의 골드로 바꿀건가
    Multi_GameManager _game;
    internal override void InitSkill()
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

public class UnitMerchant : UserSkill
{
    public UnitMerchant(UserSkillBattleData userSkillBattleData) : base(userSkillBattleData) { }
    internal override void InitSkill()
    {
        // 유닛 판매 보상 증가 (유닛별로 증가폭 별도)
        var sellRewardDatas = Multi_GameManager.Instance.BattleData.UnitSellRewardDatas;
        for (int i = 0; i < sellRewardDatas.Count; i++)
            sellRewardDatas[i].ChangeAmount(IntSkillDatas[i]);
    }
}

public class GamblerController : UserSkill
{
    public GamblerController(UserSkillBattleData userSkillBattleData, BattleUI_Mediator uiMediator) : base(userSkillBattleData) 
    {
        uiMediator.RegisterUI(BattleUI_Type.UnitUpgrdeShop, "InGameShop/UI_UnitUpgradeShopWithGamble");
    }
    internal override void InitSkill()
    {
        Multi_GameManager.Instance.BattleData
            .ShopPriceDataByUnitUpgradeData
            .Where(x => x.Key.UpgradeType == UnitUpgradeType.Value)
            .Select(x => x.Value)
            .ToList().ForEach(x => x.ChangeAmount(0));
        Multi_GameManager.Instance.BattleData.UnitUpgradeShopData.ResetPrice = IntSkillData;
    }
}

public class CombineMeteorController : UserSkill
{
    const int SwordmanStack = 1;
    const int ArcherStack = 4;
    const int SpearmanStack = 20;

    MeteorController _meteorController;
    readonly Vector3 MeteorShotPoint;
    IMonsterManager _monsterManager;

    readonly CombineMeteorStackManager _stackManager;
    UI_UserSkillStatus _stackUI;
    public CombineMeteorController(UserSkillBattleData userSkillBattleData, MeteorController meteorController, IMonsterManager monsterManager) : base(userSkillBattleData)
    {
        _monsterManager = monsterManager;
        MeteorShotPoint = PlayerIdManager.Id == PlayerIdManager.MasterId ? new Vector3(0, 30, 0) : new Vector3(0, 30, 500);
        _meteorController = meteorController;

        DefaultDamage = IntSkillDatas[0];
        StunTimePerStack = SkillDatas[1];
        DamagePerStack = IntSkillDatas[2];
        var meteorStackData = new MeteorStackData(SwordmanStack, ArcherStack, SpearmanStack);
        _stackManager = new CombineMeteorStackManager(Managers.Data.CombineConditionByUnitFalg, meteorStackData);

        _stackUI = Managers.UI.ShowSceneUI<UI_UserSkillStatus>();
        _stackUI.UpdateText(0);
    }

    internal override void InitSkill()
    {
        Managers.Unit.OnCombine += AddStack;
        Managers.Unit.OnCombine += CombineMeteor;
        Managers.Unit.OnUnitAdd += MeteorWhenSpawnRedSwordman;
    }

    int MeteorStack => _stackManager.CurrentStack;
    readonly float StunTimePerStack;
    void AddStack(UnitFlags combineUnitFlag)
    {
        _stackManager.AddCombineStack(combineUnitFlag);
        _stackUI.UpdateText(MeteorStack);
    }

    readonly UnitFlags RedSwordman = new UnitFlags(0, 0);
    void MeteorWhenSpawnRedSwordman(UnitFlags addUnitFlag)
    {
        if (addUnitFlag == RedSwordman)
            _meteorController.ShotMeteor(FindMonster(), CalculateMeteorDamage(), StunTimePerStack * MeteorStack, MeteorShotPoint);
    }
    void ShotMeteor() => _meteorController.ShotMeteor(FindMonster(), CalculateMeteorDamage(), StunTimePerStack * MeteorStack, MeteorShotPoint);

    void CombineMeteor(UnitFlags combineUnitFlag)
    {
        if (new UnitCombineSystem(Managers.Data.CombineConditionByUnitFalg).GetNeedFlags(combineUnitFlag).Where(x => x.UnitColor == UnitColor.Red).Count() > 0)
            ShotMeteor();
    }

    readonly int DamagePerStack;
    readonly int DefaultDamage;
    public int CalculateMeteorDamage() => DefaultDamage + (MeteorStack * DamagePerStack);

    Multi_NormalEnemy FindMonster()
    {
        if (Multi_EnemyManager.Instance.TryGetCurrentBoss(PlayerIdManager.Id, out Multi_BossEnemy boss)) return boss;

        var monsters = _monsterManager.GetNormalMonsters();
        if (monsters.Count == 0) return null;
        else return monsters[UnityEngine.Random.Range(0, monsters.Count)];
    }
}

public class NecromancerController : UserSkill
{
    readonly Necromencer _necromencer;
    BattleEventDispatcher _dispatcher;
    MultiEffectManager _multiEffectManager;

    public NecromancerController(UserSkillBattleData userSkillBattleData, BattleEventDispatcher dispatcher, MultiEffectManager multiEffectManager)
        : base(userSkillBattleData)
    {
        _necromencer = new Necromencer(IntSkillData);
        _dispatcher = dispatcher;
        _multiEffectManager = multiEffectManager;
    }

    UI_UserSkillStatus statusUI;
    internal override void InitSkill()
    {
        _dispatcher.OnNormalMonsterDead += _ => ResurrectOnKillCount();
        statusUI = Managers.UI.ShowSceneUI<UI_UserSkillStatus>();
        UpdateText();
    }

    readonly Vector3 EffectOffst = new Vector3(0, 0.6f, 0);
    readonly UnitFlags ResurrectUnit = new UnitFlags(UnitColor.Violet, UnitClass.Swordman);
    void ResurrectOnKillCount()
    {
        if (_necromencer.TryResurrect())
        {
            var spawnPos = SpawnPositionCalculator.CalculateWorldSpawnPostion();
            Multi_SpawnManagers.NormalUnit.Spawn(ResurrectUnit, spawnPos);
            _multiEffectManager.PlayOneShotEffect("PosionMagicCircle", spawnPos + EffectOffst);
            Managers.Sound.PlayEffect(EffectSoundType.YellowMageSkill);
        }
        UpdateText();
    }

    void UpdateText() => statusUI.UpdateText($"{_necromencer.CurrentKillCount}/{_necromencer.NeedKillCountForSummon}");
}

public class SlowTrapSpawner : UserSkill
{
    readonly MonsterPathLocationFinder _locationFinder;
    BattleEventDispatcher _dispatcher;
    readonly float DefaultSlowRate;
    readonly float SlowRatePerStage;
    readonly float MaxSlowRate;
    const float TrapRange = 5f;
    readonly Vector3 Offset = new Vector3(0, 5, 0);
    public SlowTrapSpawner(UserSkillBattleData userSkillBattleData, Transform[] wayPoints, BattleEventDispatcher dispatcher) : base(userSkillBattleData)
    {
        _locationFinder = new MonsterPathLocationFinder(wayPoints.Select(x => x.position).ToArray());
        _dispatcher = dispatcher;
        DefaultSlowRate = SkillDatas[0];
        SlowRatePerStage = SkillDatas[1];
        MaxSlowRate = SkillDatas[2];
    }

    internal override void InitSkill()
    {
        _dispatcher.OnStageUp += SpawnTrap;
        for (int i = 0; i < _traps.Length; i++)
            _traps[i] = Managers.Multi.Instantiater.PhotonInstantiateInactive("SlowTrap", PlayerIdManager.Id).GetComponent<AreaSlowApplier>();
    }

    const int SpawnCount = 2;
    AreaSlowApplier[] _traps = new AreaSlowApplier[SpawnCount];
    void SpawnTrap(int stage)
    {
        foreach (var trap in _traps)
        {
            trap.GetComponent<RPCable>().SetActive_RPC(false);
            trap.GetComponent<RPCable>().SetPosition_RPC(_locationFinder.CalculateMonsterPathLocation() + Offset);
            trap.GetComponent<RPCable>().SetActive_RPC(true);
            trap.Inject(CalculateTrapSlow(stage), TrapRange);
        }
    }

    float CalculateTrapSlow(int stage) => Mathf.Min(DefaultSlowRate + (stage * SlowRatePerStage), MaxSlowRate);
}

public class BondofWhite : UserSkill
{
    BattleEventDispatcher _dispatcher;
    Multi_GameManager _game;
    readonly int[] _upgradeDamages;
    readonly int NeedUpStageForGetFood;
    readonly int RewardFoodWhenStageUp;
    public BondofWhite(UserSkillBattleData userSkillBattleData, BattleEventDispatcher dispatcher, Multi_GameManager game) : base(userSkillBattleData) 
    {
        _dispatcher = dispatcher;
        _game = game;
        _upgradeDamages = IntSkillDatas.Take(4).ToArray();
        _game.BattleData.BattleData.WhiteUnitTime *= SkillDatas[4];
        NeedUpStageForGetFood = IntSkillDatas[5];
        RewardFoodWhenStageUp = IntSkillDatas[6];
    }
    
    internal override void InitSkill()
    {
        new UnitUpgradeHandler().UpgradeUnit(UnitColor.White, _upgradeDamages);
        _dispatcher.OnStageUp += GetFoodOnStageMultiple;
    }

    void GetFoodOnStageMultiple(int stage)
    {
        if(stage % NeedUpStageForGetFood == 0)
            _game.AddFood(RewardFoodWhenStageUp);
    }
}

public class UnitUpgradeHandler
{
    public void UpgradeUnit(UnitColor color, int[] upgradeDamages)
    {
        foreach (UnitClass unitClass in Enum.GetValues(typeof(UnitClass)))
            MultiServiceMidiator.UnitUpgrade.AddUnitDamageValue(new UnitFlags(color, unitClass), upgradeDamages[(int)unitClass], UnitStatType.All);
    }
}

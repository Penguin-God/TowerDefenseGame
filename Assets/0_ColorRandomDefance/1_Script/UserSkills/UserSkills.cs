using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.ObjectModel;

public class ActiveUserSkillDataContainer
{
    public readonly SkillType MainSkill;
    public readonly int MainSkillLevle;
    public readonly SkillType SubSkill;
    public readonly int SubSkillLevle;
    public readonly DataManager _data;

    public ActiveUserSkillDataContainer(SkillType mainSkill, int mainSkillLevel,  SkillType subSkill, int subSkillLevle, DataManager data)
    {
        MainSkill = mainSkill;
        MainSkillLevle = mainSkillLevel;
        SubSkill = subSkill;
        SubSkillLevle = subSkillLevle;
        _data = data;
    }

    public bool ActiveEquipSkill(SkillType skill) => skill == MainSkill || skill == SubSkill;
    public int GetFirstIntData(UserSkillClass skillClass)
    {
        if (skillClass == UserSkillClass.Main)
            return (int)_data.UserSkill.GetSkillLevelData(MainSkill, MainSkillLevle).BattleDatas[0];
        else if(skillClass == UserSkillClass.Sub)
            return (int)_data.UserSkill.GetSkillLevelData(SubSkill, SubSkillLevle).BattleDatas[0];
        return 0;
    }

    public static ActiveUserSkillDataContainer CreateSkillData(ClientDataManager client, DataManager data)
    {
        var main = Managers.ClientData.EquipSkillManager.MainSkill;
        var sub = Managers.ClientData.EquipSkillManager.SubSkill;
        return new ActiveUserSkillDataContainer(main, client.GetSkillLevel(main), sub, client.GetSkillLevel(sub), data);
    }
}

public abstract class UserSkill
{
    SkillType _skillType;
    public UserSkill(SkillType skillType) => _skillType = skillType;
    public UserSkill(UserSkillBattleData userSkillBattleData) { }

    internal abstract void InitSkill();
    protected float[] SkillDatas => Managers.ClientData.GetSkillLevelData(_skillType).BattleDatas;
    protected int[] IntSkillDatas => SkillDatas.Select(x => (int)x).ToArray();
    protected float SkillData => SkillDatas[0];
    protected int IntSkillData => (int)SkillData;
}

public struct UserSkillBattleData
{
    public readonly SkillType SkillType;
    public readonly UserSkillClass SkillClass;
    public readonly IReadOnlyList<float> SkillDatas;
    public float SkillData => SkillDatas[0];
    public IReadOnlyList<int> IntSkillDatas => SkillDatas.Select(x => (int)x).ToArray();
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
        SkillType.시작골드증가, SkillType.시작고기증가, SkillType.최대유닛증가, SkillType.노란기사강화, SkillType.컬러마스터, SkillType.보스데미지증가, SkillType.마창사, SkillType.썬콜 
    });

    public UserSkill ActiveSkill(SkillType skillType, BattleDIContainer container)
    {
        UserSkillBattleData skillBattleData = Managers.Data.UserSkill.GetSkillBattleData(skillType, 1);
        if (SimpleSkills.Contains(skillType))
        {
            ActiveSimpleSkill(skillBattleData, container);
            return null;
        }
        else
            return ActiveComplexSkill(skillBattleData, container);
    }

    void ActiveSimpleSkill(UserSkillBattleData skillBattleData, BattleDIContainer container)
    {
        switch (skillBattleData.SkillType)
        {
            case SkillType.시작골드증가: Multi_GameManager.Instance.AddGold(skillBattleData.IntSkillData); break;
            case SkillType.시작고기증가: Multi_GameManager.Instance.AddFood(skillBattleData.IntSkillData); break;
            case SkillType.최대유닛증가: Multi_GameManager.Instance.IncreasedMaxUnitCount(skillBattleData.IntSkillData); break;
            case SkillType.노란기사강화: Multi_GameManager.Instance.BattleData.YellowKnightRewardGold = skillBattleData.IntSkillData; break;
            case SkillType.컬러마스터: container.GetComponent<SwordmanGachaController>().ChangeUnitSummonMaxColor(UnitColor.Violet); break;
            case SkillType.보스데미지증가: MultiServiceMidiator.UnitUpgrade.ScaleUnitDamageValue(skillBattleData.SkillData, UnitStatType.BossDamage); break;
            case SkillType.마창사:
                Managers.Data.Unit.SetThrowSpearData(Managers.Resources.Load<ThrowSpearDataContainer>("Data/ScriptableObject/MagicThrowSpearData").ChangeAttackRate(skillBattleData.SkillData)); break;
            case SkillType.썬콜: break; // 썬콜은 존재만 하면 스포너에서 알아서 할당함
        }
    }

    UserSkill ActiveComplexSkill(UserSkillBattleData skillBattleData, BattleDIContainer container)
    {
        UserSkill result;
        switch (skillBattleData.SkillType)
        {
            case SkillType.태극스킬: result = new TaegeukController(skillBattleData.SkillType); break;
            case SkillType.검은유닛강화: result = new BlackUnitUpgrade(skillBattleData.SkillType); break;
            case SkillType.상대색깔변경: result = new ColorChange(skillBattleData.SkillType); break;
            case SkillType.고기혐오자: result = new FoodHater(skillBattleData.SkillType); break;
            case SkillType.판매보상증가: result = new SellUpgrade(skillBattleData.SkillType); break;
            case SkillType.장사꾼: result = new DiscountMerchant(skillBattleData.SkillType); break;
            case SkillType.조합메테오: result = new CombineMeteorController(skillBattleData.SkillType, container.GetComponent<MeteorController>(), container.GetComponent<IMonsterManager>()); break;
            case SkillType.네크로맨서:
                result = new NecromancerController(skillBattleData.SkillType, container.GetService<BattleEventDispatcher>(), container.GetComponent<EffectSynchronizer>()); break;
            default: result = null; break;
        }
        result.InitSkill();
        return result;
    }
}

// ================= 스킬 세부 구현 =====================

public class TaegeukController : UserSkill
{
    public TaegeukController(SkillType skillType) : base(skillType) { }

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
    public BlackUnitUpgrade(SkillType skillType) : base(skillType) { }

    public event Action<UnitFlags> OnBlackUnitReinforce;
    int[] _upgradeDamages;
    internal override void InitSkill()
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

public class ColorChange : UserSkill // 하얀 유닛을 뽑을 때 뽑은 직업과 같은 상대 유닛의 색깔을 다른 색깔로 변경
{
    public ColorChange(SkillType skillType) : base(skillType) { }

    int[] _whiteUnitCounts = new int[4];
    public event Action<byte, byte> OnUnitColorChanaged; // 변하기 전 색깔, 변한 후 색깔
    SkillColorChanger colorChanger;
    internal override void InitSkill()
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

public class SellUpgrade : UserSkill
{
    public SellUpgrade(SkillType skillType) : base(skillType) { }
    internal override void InitSkill()
    {
        // 유닛 판매 보상 증가 (유닛별로 증가폭 별도)
        var sellRewardDatas = Multi_GameManager.Instance.BattleData.UnitSellRewardDatas;
        for (int i = 0; i < sellRewardDatas.Count; i++)
            sellRewardDatas[i].ChangeAmount(IntSkillDatas[i]);
    }
}

public class DiscountMerchant : UserSkill
{
    public DiscountMerchant(SkillType skillType) : base(skillType) { }
    internal override void InitSkill()
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
    CombineMeteor _combineMeteor;
    const int SwordmanScore = 1;
    const int ArcherScore = 4;
    const int SpearmanScore = 20;

    MeteorController _meteorController;
    readonly Vector3 MeteorShotPoint;
    IMonsterManager _monsterManager;

    readonly int DamagePerScore;
    readonly float StunTimePerScore;
    readonly int DamagePerStack;
    UI_UserSkillStatus _stackUI;
    public CombineMeteorController(SkillType skillType, MeteorController meteorController, IMonsterManager monsterManager) : base(skillType) 
    {
        _combineMeteor = new CombineMeteor(new MeteorScoreData(SwordmanScore, ArcherScore, SpearmanScore), Managers.Data.CombineConditionByUnitFalg);
        _monsterManager = monsterManager;
        MeteorShotPoint = PlayerIdManager.Id == PlayerIdManager.MasterId ? new Vector3(0, 30, 0) : new Vector3(0, 30, 500);
        _meteorController = meteorController;
        DamagePerScore = IntSkillDatas[0];
        StunTimePerScore = SkillDatas[1];
        DamagePerStack = IntSkillDatas[2];
        _stackUI = Managers.UI.ShowSceneUI<UI_UserSkillStatus>();
        UpdateStackText();
    }

    internal override void InitSkill()
    {
        Managers.Unit.OnCombine += ShotMeteor;
    }

    int _meteorStack;
    void ShotMeteor(UnitFlags combineUnitFlag)
    {
        int score = _combineMeteor.CalculateMeteorScore(combineUnitFlag);
        if (score > 0)
        {
            _meteorController.ShotMeteor(FindMonster(), CalculateMeteorDamage(score), StunTimePerScore * score, MeteorShotPoint);
            _meteorStack += score;
            UpdateStackText();
        }
    }

    int CalculateMeteorDamage(int combineScore) => _combineMeteor.CalculateMeteorDamage(combineScore, DamagePerScore, _meteorStack, DamagePerStack);
    void UpdateStackText() => _stackUI.UpdateText(_meteorStack);
    Multi_NormalEnemy FindMonster()
    {
        var monsters = _monsterManager.GetNormalMonsters();
        if (monsters.Count == 0) return null;
        else return monsters[UnityEngine.Random.Range(0, monsters.Count)];
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
            var spawnPos = new WorldSpawnPositionCalculator(20, 0, 0, 0).CalculateWorldPostion(Multi_Data.instance.GetWorldPosition(PlayerIdManager.Id));
            Multi_SpawnManagers.NormalUnit.Spawn(ResurrectUnit, spawnPos);
            _effectSynchronizer.PlayOneShotEffect("PosionMagicCircle", spawnPos + EffectOffst);
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
    public SlowTrapSpawner(SkillType skillType, Transform[] wayPoints, BattleEventDispatcher dispatcher) : base(skillType)
    {
        _locationFinder = new MonsterPathLocationFinder(wayPoints.Select(x => x.position).ToArray());
        _dispatcher = dispatcher;
    }

    internal override void InitSkill()
    {
        _dispatcher.OnStageUp += _ => SpawnTrap();
        for (int i = 0; i < _traps.Length; i++)
            _traps[i] = Managers.Multi.Instantiater.PhotonInstantiateInactive("", PlayerIdManager.Id);
    }

    const int SpawnCount = 2;
    GameObject[] _traps = new GameObject[SpawnCount];
    void SpawnTrap()
    {
        foreach (var trap in _traps)
            trap.GetComponent<RPCable>().SetPosition_RPC(_locationFinder.CalculateMonsterPathLocation());
    }
}

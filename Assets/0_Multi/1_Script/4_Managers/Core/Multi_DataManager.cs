using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
    void LoadCSV(TextAsset csv);
}

public interface ICsvLoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict(string csv);
}

public class Multi_DataManager
{
    #region UI Data
    // 조합 조건
    Dictionary<UnitFlags, CombineCondition> _combineConditionByUnitFalg = new Dictionary<UnitFlags, CombineCondition>();
    public IReadOnlyDictionary<UnitFlags, CombineCondition> CombineConditionByUnitFalg => _combineConditionByUnitFalg;

    // 유닛 창 정보
    Dictionary<UnitFlags, UI_UnitWindowData> _unitWindowDataByUnitFlags = new Dictionary<UnitFlags, UI_UnitWindowData>();
    public IReadOnlyDictionary<UnitFlags, UI_UnitWindowData> UnitWindowDataByUnitFlags => _unitWindowDataByUnitFlags;

    public IEnumerable<UI_RandomShopGoodsData> RandomShopDatas { get; private set; }
    #endregion

    #region Unit Data
    Dictionary<string, UnitNameData> _unitNameDataByUnitKoreaName = new Dictionary<string, UnitNameData>();
    public IReadOnlyDictionary<string, UnitNameData> UnitNameDataByUnitKoreaName => _unitNameDataByUnitKoreaName;

    Dictionary<UnitFlags, UnitNameData> _unitNameDataByFlag = new Dictionary<UnitFlags, UnitNameData>();
    public IReadOnlyDictionary<UnitFlags, UnitNameData> UnitNameDataByFlag => _unitNameDataByFlag;

    Dictionary<UnitFlags, WeaponData> _weaponDataByUnitFlag = new Dictionary<UnitFlags, WeaponData>();
    public IReadOnlyDictionary<UnitFlags, WeaponData> WeaponDataByUnitFlag => _weaponDataByUnitFlag;

    Dictionary<UnitFlags, UnitStat> _unitStatByFlag = new Dictionary<UnitFlags, UnitStat>();
    public UnitStat GetUnitStat(UnitFlags flag) => _unitStatByFlag[flag].GetClone();

    Dictionary<UnitFlags, UnitPassiveStat> _unitPassiveStatByFlag = new Dictionary<UnitFlags, UnitPassiveStat>();
    public IReadOnlyList<float> GetUnitPassiveStats(UnitFlags flag) => _unitPassiveStatByFlag[flag].Stats;

    Dictionary<UnitFlags, MageUnitStat> _mageStatByFlag = new Dictionary<UnitFlags, MageUnitStat>();
    public IReadOnlyDictionary<UnitFlags, MageUnitStat> MageStatByFlag => _mageStatByFlag;
    #endregion

    #region Enemy Data
    Dictionary<int, NormalEnemyData> _normalEnemyDataByStage = new Dictionary<int, NormalEnemyData>();
    public IReadOnlyDictionary<int, NormalEnemyData> NormalEnemyDataByStage => _normalEnemyDataByStage;

    Dictionary<int, BossData> _bossDataByLevel = new Dictionary<int, BossData>();
    public IReadOnlyDictionary<int, BossData> BossDataByLevel => _bossDataByLevel;

    Dictionary<int, BossData> _towerDataByLevel = new Dictionary<int, BossData>();
    public IReadOnlyDictionary<int, BossData> TowerDataByLevel => _towerDataByLevel;
    #endregion

    #region Sound Data
    public Dictionary<EffectSoundType, EffectSound> EffectBySound { get; private set; }
    public Dictionary<BgmType, BgmSound> BgmBySound { get; private set; }
    #endregion

    public BattleGameData BattleGameData;

    public void Init()
    {
        Clears();

        // 무조건 가장먼저 실행되어야 함
        _unitNameDataByUnitKoreaName = MakeCsvDict<UnitNameDatas, string, UnitNameData>("UnitData/UnitNameData");
        _unitNameDataByFlag = _unitNameDataByUnitKoreaName.ToDictionary(x => x.Value.UnitFlags, x => x.Value);

        // Unit
        _mageStatByFlag = MakeCsvDict<MageUnitStats, UnitFlags, MageUnitStat>("UnitData/MageUnitStat");
        _unitPassiveStatByFlag = MakeCsvDict<UnitPassiveStats, UnitFlags, UnitPassiveStat>("UnitData/UnitPassiveStat");
        _unitStatByFlag = MakeCsvDict<UnitStats, UnitFlags, UnitStat>("UnitData/UnitStat");
        _weaponDataByUnitFlag = MakeCsvDict<WeaponDatas, UnitFlags, WeaponData>("UnitData/UnitWeaponData");

        // UI
        _combineConditionByUnitFalg = MakeCsvDict<CombineConditions, UnitFlags, CombineCondition>("UnitData/CombineConditionData");
        _unitWindowDataByUnitFlags = MakeCsvDict<UI_UnitWindowDatas, UnitFlags, UI_UnitWindowData>("UIData/UI_UnitWindowData");
        RandomShopDatas = LoadData<UI_RandomShopGoodsData>("UIData/RandomShopData");

        // enemy
        _normalEnemyDataByStage = MakeCsvDict<NormalEnemyDatas, int, NormalEnemyData>("EnemyData/NormalEnemyData");
        _bossDataByLevel = MakeCsvDict<BossDatas, int, BossData>("EnemyData/BossData");
        _towerDataByLevel = MakeCsvDict<BossDatas, int, BossData>("EnemyData/TowerData");

        // Player
        BattleGameData = JsonUtility.FromJson<BattleGameData>(Resources.Load<TextAsset>("Data/ClientData/BattleGameData").text);

        // Sound 
        EffectBySound = MakeCsvDict<EffectSoundLoder, EffectSoundType, EffectSound>("SoundData/EffectSoundData");
        BgmBySound = MakeCsvDict<BgmSoundLoder, BgmType, BgmSound>("SoundData/BgmSoundData");
    }


    void Clears()
    {
        _unitNameDataByUnitKoreaName.Clear();
        _unitNameDataByFlag.Clear();

        _mageStatByFlag.Clear();
        _unitPassiveStatByFlag.Clear();
        _unitStatByFlag.Clear();
        _weaponDataByUnitFlag.Clear();

        _combineConditionByUnitFalg.Clear();
        _unitWindowDataByUnitFlags.Clear();
        RandomShopDatas = null;

        _bossDataByLevel.Clear();
        _towerDataByLevel.Clear();
    }

    IEnumerable<T> LoadData<T>(string path) => CsvUtility.GetEnumerableFromCsv<T>(Multi_Managers.Resources.Load<TextAsset>($"Data/{path}").text);

    Dictionary<Key, Value> MakeCsvDict<ICsvLoader, Key, Value>(string path) where ICsvLoader : ICsvLoader<Key, Value>, new()
    {
        ICsvLoader loder = new ICsvLoader();
        return loder.MakeDict(Multi_Managers.Resources.Load<TextAsset>($"Data/{path}").text);
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Multi_Managers.Resources.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}

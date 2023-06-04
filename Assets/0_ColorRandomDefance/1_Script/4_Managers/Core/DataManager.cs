﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
    void LoadCSV(TextAsset csv);
}

public interface ICsvLoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict(string csv);
}

public class DataManager
{
    UnitData _unit = new UnitData();
    public UnitData Unit => _unit;

    UI_Data _ui = new UI_Data();
    EnemyData _enemy = new EnemyData();

    UserSkillData _userSkill = new UserSkillData();
    public UserSkillData UserSkill => _userSkill;

    #region UI Data
    // 조합 조건
    public IReadOnlyDictionary<UnitFlags, CombineCondition> CombineConditionByUnitFalg => _ui.CombineConditionByUnitFalg;
    // 유닛 창 정보
    public IReadOnlyDictionary<UnitFlags, UI_UnitWindowData> UnitWindowDataByUnitFlags => _ui.UnitWindowDataByUnitFlags;
    #endregion

    #region Unit Data
    public IReadOnlyDictionary<string, UnitNameData> UnitNameDataByUnitKoreaName => _unit.UnitNameDataByUnitKoreaName;
    public IReadOnlyDictionary<UnitFlags, UnitNameData> UnitNameDataByFlag => _unit.UnitNameDataByFlag;

    public IReadOnlyList<float> GetUnitPassiveStats(UnitFlags flag) => _unit.GetUnitPassiveStats(flag);
    public IReadOnlyDictionary<UnitFlags, MageUnitStat> MageStatByFlag => _unit.MageStatByFlag;
    #endregion

    #region Enemy Data
    public IReadOnlyDictionary<int, NormalEnemyData> NormalEnemyDataByStage => _enemy.NormalEnemyDataByStage;
    public IReadOnlyDictionary<int, BossData> BossDataByLevel => _enemy.BossDataByLevel;
    public IReadOnlyDictionary<int, BossData> TowerDataByLevel => _enemy.TowerDataByLevel;
    #endregion

    #region Sound Data
    public Dictionary<EffectSoundType, EffectSound> EffectBySound { get; private set; }
    public Dictionary<BgmType, BgmSound> BgmBySound { get; private set; }
    #endregion

    public IEnumerable<string> TextKeys;

    public void Init()
    {
        Clears();

        _unit.Init(this); // 무조건 유닛 먼저
        _ui.Init(this);
        _enemy.Init(this);
        _userSkill.Init(this);

        // Sound 
        EffectBySound = MakeCsvDict<EffectSoundLoder, EffectSoundType, EffectSound>("SoundData/EffectSoundData");
        BgmBySound = MakeCsvDict<BgmSoundLoder, BgmType, BgmSound>("SoundData/BgmSoundData");

        TextKeys = Managers.Resources.Load<TextAsset>("TextKeyData").text.Split('\n').Skip(1).Where(x => string.IsNullOrEmpty(x) == false);
    }


    void Clears()
    {
        _unit.Clear();
        _ui.Clear();
        _enemy.Clear();
    }

    IEnumerable<T> LoadData<T>(string path) => CsvUtility.CsvToArray<T>(Managers.Resources.Load<TextAsset>($"Data/{path}").text);

    Dictionary<Key, Value> MakeCsvDict<ICsvLoader, Key, Value>(string path) where ICsvLoader : ICsvLoader<Key, Value>, new()
    {
        ICsvLoader loder = new ICsvLoader();
        return loder.MakeDict(Managers.Resources.Load<TextAsset>($"Data/{path}").text);
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resources.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }


    public class UnitData
    {
        Dictionary<string, UnitNameData> _unitNameDataByUnitKoreaName = new Dictionary<string, UnitNameData>();
        public IReadOnlyDictionary<string, UnitNameData> UnitNameDataByUnitKoreaName => _unitNameDataByUnitKoreaName;

        Dictionary<UnitFlags, UnitNameData> _unitNameDataByFlag = new Dictionary<UnitFlags, UnitNameData>();
        public IReadOnlyDictionary<UnitFlags, UnitNameData> UnitNameDataByFlag => _unitNameDataByFlag;

        Dictionary<UnitFlags, UnitStat> _unitStatByFlag = new Dictionary<UnitFlags, UnitStat>();
        public IReadOnlyDictionary<UnitFlags, UnitStat> UnitStatByFlag => _unitStatByFlag;
        public Dictionary<UnitFlags, UnitDamageInfo> DamageInfoByFlag => UnitStatByFlag.ToDictionary(x => x.Key, x => new UnitDamageInfo(x.Value.Damage, x.Value.BossDamage));

        Dictionary<UnitFlags, UnitPassiveStat> _unitPassiveStatByFlag = new Dictionary<UnitFlags, UnitPassiveStat>();
        public IReadOnlyList<float> GetUnitPassiveStats(UnitFlags flag) => _unitPassiveStatByFlag[flag].Stats;

        Dictionary<UnitFlags, MageUnitStat> _mageStatByFlag = new Dictionary<UnitFlags, MageUnitStat>();
        public IReadOnlyDictionary<UnitFlags, MageUnitStat> MageStatByFlag => _mageStatByFlag;

        public void Init(DataManager manager)
        {
            _unitNameDataByUnitKoreaName = manager.MakeCsvDict<UnitNameDatas, string, UnitNameData>("UnitData/UnitNameData");
            _unitNameDataByFlag = _unitNameDataByUnitKoreaName.ToDictionary(x => x.Value.UnitFlags, x => x.Value);
            _mageStatByFlag = manager.MakeCsvDict<MageUnitStats, UnitFlags, MageUnitStat>("UnitData/MageUnitStat");
            _unitPassiveStatByFlag = manager.MakeCsvDict<UnitPassiveStats, UnitFlags, UnitPassiveStat>("UnitData/UnitPassiveStat");
            _unitStatByFlag = manager.MakeCsvDict<UnitStats, UnitFlags, UnitStat>("UnitData/UnitStat");
        }

        public void Clear()
        {
            _unitNameDataByUnitKoreaName.Clear();
            _unitNameDataByFlag.Clear();
            _mageStatByFlag.Clear();
            _unitPassiveStatByFlag.Clear();
            _unitStatByFlag.Clear();
        }
    }

    class UI_Data
    {
        Dictionary<UnitFlags, CombineCondition> _combineConditionByUnitFalg = new Dictionary<UnitFlags, CombineCondition>();
        public IReadOnlyDictionary<UnitFlags, CombineCondition> CombineConditionByUnitFalg => _combineConditionByUnitFalg;

        // 유닛 창 정보
        Dictionary<UnitFlags, UI_UnitWindowData> _unitWindowDataByUnitFlags = new Dictionary<UnitFlags, UI_UnitWindowData>();
        public IReadOnlyDictionary<UnitFlags, UI_UnitWindowData> UnitWindowDataByUnitFlags => _unitWindowDataByUnitFlags;

        public void Init(DataManager manager)
        {
            _combineConditionByUnitFalg = manager.MakeCsvDict<CombineConditions, UnitFlags, CombineCondition>("UnitData/CombineConditionData");
            _unitWindowDataByUnitFlags = manager.MakeCsvDict<UI_UnitWindowDatas, UnitFlags, UI_UnitWindowData>("UIData/UI_UnitWindowData");
        }

        public void Clear()
        {
            _combineConditionByUnitFalg.Clear();
            _unitWindowDataByUnitFlags.Clear();
        }
    }

    class EnemyData
    {
        Dictionary<int, NormalEnemyData> _normalEnemyDataByStage = new Dictionary<int, NormalEnemyData>();
        public IReadOnlyDictionary<int, NormalEnemyData> NormalEnemyDataByStage => _normalEnemyDataByStage;

        Dictionary<int, BossData> _bossDataByLevel = new Dictionary<int, BossData>();
        public IReadOnlyDictionary<int, BossData> BossDataByLevel => _bossDataByLevel;

        Dictionary<int, BossData> _towerDataByLevel = new Dictionary<int, BossData>();
        public IReadOnlyDictionary<int, BossData> TowerDataByLevel => _towerDataByLevel;

        public void Init(DataManager manager)
        {
            _normalEnemyDataByStage = manager.MakeCsvDict<NormalEnemyDatas, int, NormalEnemyData>("EnemyData/NormalEnemyData");
            _bossDataByLevel = manager.MakeCsvDict<BossDatas, int, BossData>("EnemyData/BossData");
            _towerDataByLevel = manager.MakeCsvDict<BossDatas, int, BossData>("EnemyData/TowerData");
        }

        public void Clear()
        {
            _bossDataByLevel.Clear();
            _towerDataByLevel.Clear();
        }
    }

    public class UserSkillData
    {
        public Dictionary<SkillType, UserSkillGoodsData> _typeByGoodsData;
        public IEnumerable<UserSkillGoodsData> AllSkills => _typeByGoodsData.Values;
        public void Init(DataManager manager)
        {
            _typeByGoodsData = manager.MakeCsvDict<UserSkillGoodsLoder, SkillType, UserSkillGoodsData>("SkillData/SkillGoodsData");
        }

        public UserSkillLevelData GetSkillLevelData(SkillType type, int level)
        {
            if (_typeByGoodsData.TryGetValue(type, out UserSkillGoodsData data) == false)
                Debug.LogError($"유저 스킬 배틀 데이터 {type} : {level} 로드 실패");
            return data.LevelDatas[level - 1];
        }

        public UserSkillGoodsData GetSkillGoodsData(SkillType skillType)
        {
            if (_typeByGoodsData.TryGetValue(skillType, out UserSkillGoodsData result) == false)
                Debug.LogError($"유저 스킬 데이터 {skillType} : 로드 실패");
            return result;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SerializablePlayerData
{
    public int GoldAmount;
    public int GemAmount;
    public int Score;
    public SerializableSkillData[] SkillDatas;
    public SkillType EquipMainSKill;
    public SkillType EquipSubSKill;
    public SerializablePlayerData(PlayerDataManager manager)
    {
        GoldAmount = manager.Gold.Amount;
        GemAmount = manager.Gem.Amount;
        Score = manager.Score;
        SkillDatas = CreateSkillDatas(manager.SkillInventroy).ToArray();
        EquipMainSKill = manager.EquipSkillManager.MainSkill;
        EquipSubSKill = manager.EquipSkillManager.SubSkill;
    }

    List<SerializableSkillData> CreateSkillDatas(SkillInventroy inventory)
    {
        var result = new List<SerializableSkillData>();
        foreach (var skill in inventory.GetAllHasSkills())
        {
            var skillInfo = inventory.GetSkillInfo(skill);
            result.Add(new SerializableSkillData(skill, skillInfo.Level, skillInfo.HasAmount));
        }
        return result;
    }
}

[Serializable]
public struct SerializableSkillData
{
    public SkillType SkillType;
    public int Level;
    public int HasAmount;

    public SerializableSkillData(SkillType skillType, int level, int hasAmount)
    {
        SkillType = skillType;
        Level = level;
        HasAmount = hasAmount;
    }
}

public class PlayerPrefabsSaver : IPlayerDataPersistence
{
    public void Save(PlayerDataManager playerData)
    {
        var data = new SerializablePlayerData(playerData);
        string json = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();
    }
}

public class PlayerPrefabsLoder
{
    public bool Load(out PlayerDataManager playerData)
    {
        playerData = null;
        if (PlayerPrefs.HasKey("PlayerData") == false) return false;

        string json = PlayerPrefs.GetString("PlayerData");
        var data = JsonUtility.FromJson<SerializablePlayerData>(json);
        playerData = new PlayerDataManager(CreateSkillInventroy(data.SkillDatas), data.GoldAmount, data.GemAmount, data.Score, data.EquipMainSKill, data.EquipSubSKill);
        return true; 
    }

    public PlayerDataManager Load()
    {
        Load(out var result);
        return result;
    }

    SkillInventroy CreateSkillInventroy(IEnumerable<SerializableSkillData> skillDatas) 
        => new (skillDatas.ToDictionary(x => x.SkillType, x => new PlayerOwnedSkillInfo(x.Level, x.HasAmount)));
}

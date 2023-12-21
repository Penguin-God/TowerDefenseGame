using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializablePlayerData
{
    public int GoldAmount;
    public int GemAmount;
    public int Score;
    public SerializableSkillData[] SkillDatas;

    public SerializablePlayerData(PlayerDataManager manager)
    {
        GoldAmount = manager.Gold.Amount;
        GemAmount = manager.Gem.Amount;
        Score = manager.Score;
        SkillDatas = CreateSkillDatas(manager.SkillInventroy).ToArray();
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

public class PlayerPrefabsPersistenceManager : IPlayerDataPersistence
{
    public void Save(PlayerDataManager playerData)
    {
        var data = new SerializablePlayerData(playerData);
        // string json = JsonUtility.ToJson(data);
        string json = JsonUtility.ToJson(data, true);
        Debug.Log(json);
        //PlayerPrefs.SetString("PlayerData", json);
        //PlayerPrefs.Save();
    }
}

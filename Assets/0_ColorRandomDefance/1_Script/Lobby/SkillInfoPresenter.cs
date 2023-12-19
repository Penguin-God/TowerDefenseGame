using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfoPresenter
{
    public readonly SkillType SkillType;
    readonly SkillDataGetter _skillDataGetter;
    
    public SkillInfoPresenter(SkillType skillType, SkillDataGetter skillDataGetter)
    {
        SkillType = skillType;
        _skillDataGetter = skillDataGetter;
    }

    UserSkillGoodsData GetSkillData() => Managers.Data.UserSkill.GetSkillGoodsData(SkillType);
    public string GetSkillName() => GetSkillData().SkillName;
    public string GetSkillDescription() => GetSkillData().Description;
    public UserSkillClass GetSkillClass() => GetSkillData().SkillClass;
    public Sprite GetSkillImage() => SpriteUtility.GetSkillImage(SkillType);
    public int GetSkillLevel() => _skillDataGetter.GetSkillLevel(SkillType);
    public int GetGoldForUpgrade() => _skillDataGetter.GetSkillUpgradeData(SkillType).NeedGold;
    public string GetExpGaugeText() => $"{_skillDataGetter.GetSkillExp(SkillType)} / {_skillDataGetter.GetNeedLevelUpExp(SkillType)}";
    public float GetExpGaugeAmount() => (float)_skillDataGetter.GetSkillExp(SkillType) / _skillDataGetter.GetNeedLevelUpExp(SkillType);
    public IEnumerable<string> GetSkillStatTexts()
    {
        var result = new List<string>();
        IReadOnlyList<string> statTexts = GetSkillData().StatInfoFraems;
        for (int i = 0; i < statTexts.Count; i++)
        {
            string text = statTexts[i].Replace("{data}", StatToText(GetUnitStats(GetSkillLevel())[i], i));
            if (string.IsNullOrEmpty(GetUpgradeStatText(i)) == false)
                text += GetUpgradeStatText(i);
            result.Add(TextUtility.RelpaceKeyToValue(text));
        }
        return result;
    }

    float[] GetUnitStats(int level) => Managers.Data.UserSkill.GetSkillLevelData(SkillType, level).BattleDatas;

    string GetUpgradeStatText(int index)
    {
        float currentStat = GetUnitStats(GetSkillLevel())[index];
        float nextStat = GetUnitStats(GetSkillLevel() + 1)[index];
        float delta = (float)Math.Round(nextStat - currentStat, 2);
        if (0.001f >= Mathf.Abs(delta)) return "";

        string changeSign = delta > 0 ? "+" : "-";
        return $"<color=#00ff00> {changeSign} {Mathf.Abs(delta)}</color>";
    }

    string StatToText(float stat, int index)
    {
        switch (SkillType)
        {
            case SkillType.거인학살자: stat += 1; break;
            case SkillType.태극스킬: stat += Managers.Data.Unit.UnitStatByFlag[new UnitFlags(0, index)].Damage; break;
            case SkillType.흑의결속: stat += Managers.Data.Unit.UnitStatByFlag[new UnitFlags(7, index)].Damage; break;
        }
        if (stat >= 1000) return stat.ToString("#,##0");
        else return stat.ToString();
    }
}

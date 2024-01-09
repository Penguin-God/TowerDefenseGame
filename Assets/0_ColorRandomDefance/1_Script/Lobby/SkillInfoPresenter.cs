using System;
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
    public bool IsSkillAtLevelBoundary() => SkillIsMax() || IsHasSkill() == false;
    public bool SkillIsMax() => _skillDataGetter.SkillIsMax(SkillType);
    public bool IsHasSkill() => GetSkillLevel() > 0;
    public int GetGoldForUpgrade() => _skillDataGetter.GetSkillUpgradeData(SkillType).NeedGold;
    public string GetExpGaugeText()
    {
        if (IsSkillAtLevelBoundary()) return "";
        return $"{_skillDataGetter.GetSkillExp(SkillType)} / {_skillDataGetter.GetNeedLevelUpExp(SkillType)}";
    }
    public float GetExpGaugeAmount()
    {
        if (IsHasSkill() == false) return 0;
        else if (SkillIsMax()) return 1;
        return (float)_skillDataGetter.GetSkillExp(SkillType) / _skillDataGetter.GetNeedLevelUpExp(SkillType);
    }
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

    float[] GetUnitStats(int level)
    {
        if (level == 0) level = 1;
        return Managers.Data.UserSkill.GetSkillLevelData(SkillType, level).BattleDatas;
    }

    string GetUpgradeStatText(int index)
    {
        if (IsSkillAtLevelBoundary()) return "";

        float currentStat = GetUnitStats(GetSkillLevel())[index];
        float nextStat = GetUnitStats(GetSkillLevel() + 1)[index];
        float delta = (float)Math.Round(nextStat - currentStat, 2);
        if (0.001f >= Mathf.Abs(delta)) return "";

        string changeSign = delta > 0 ? "+" : "-";
        return $"<color=#00ff00> {changeSign} {StatToText(Mathf.Abs(delta), index)}</color>";
    }

    string StatToText(float stat, int index)
    {
        switch (SkillType)
        {
            case SkillType.거인학살자: stat = (float)Math.Round(stat * 100); break;
            case SkillType.마창사: stat = Mathf.Abs((float)Math.Round(stat * 100) - 100); break;
            case SkillType.황금빛기사: if(index == 1) stat = (float)Math.Round(stat * 100); break;
            case SkillType.전설의기사: stat = (float)Math.Round(stat * 100); break;
            case SkillType.태극스킬: stat += Managers.Data.Unit.UnitStatByFlag[new UnitFlags(0, index)].Damage; break;
            case SkillType.흑의결속: stat += Managers.Data.Unit.UnitStatByFlag[new UnitFlags(7, index)].Damage; break;
        }
        if (stat >= 1000) return stat.ToString("#,##0");
        else return stat.ToString();
    }
}

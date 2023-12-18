using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public string GetSkillName() => Managers.Data.UserSkill.GetSkillGoodsData(SkillType).SkillName;
    public string GetSkillDescription() => Managers.Data.UserSkill.GetSkillGoodsData(SkillType).Description;
    public UserSkillClass GetSkillClass() => Managers.Data.UserSkill.GetSkillGoodsData(SkillType).SkillClass;
    public Sprite GetSkillImage() => SpriteUtility.GetSkillImage(SkillType);
    public string GetExpGaugeText() => $"{_skillDataGetter.GetSkillExp(SkillType)} / {_skillDataGetter.GetNeedLevelUpExp(SkillType)}";
    public float GetExpGaugeAmount() => (float)_skillDataGetter.GetSkillExp(SkillType) / _skillDataGetter.GetNeedLevelUpExp(SkillType);
}

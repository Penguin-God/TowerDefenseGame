using System.Collections;
using System.Collections.Generic;

public class SkillUpgradeUseCase
{
    readonly SkillDataGetter _skillDataGetter;
    readonly PlayerDataManager _playerDataManager;
    public SkillUpgradeUseCase(SkillDataGetter skillDataGetter, PlayerDataManager playerDataManager)
    {
        _skillDataGetter = skillDataGetter;
        _playerDataManager = playerDataManager;
    }

    public bool CanUpgrade(SkillType skillType)
    {
        if(_skillDataGetter.SkillIsMax(skillType)) return false;

        var upgradeData = _skillDataGetter.GetSkillUpgradeData(skillType);
        return _skillDataGetter.GetSkillExp(skillType) > upgradeData.NeedExp && _playerDataManager.HasGold(upgradeData.NeedGold);
    }

    public void Upgrade(SkillType skillType)
    {
        if (CanUpgrade(skillType) == false) return;
        var upgradeData = _skillDataGetter.GetSkillUpgradeData(skillType);
        _playerDataManager.TryUseGold(upgradeData.NeedGold);
        _playerDataManager.SkillInventroy.LevelUpSkill(skillType, upgradeData.NeedExp);
    }
}

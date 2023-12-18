using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SkillUpgradeUseCase
{
    readonly PlayerDataManager _playerDataManager;
    readonly IEnumerable<SkillUpgradeData> _skillUpgradeDatas;
    public SkillUpgradeUseCase(PlayerDataManager playerDataManager, IEnumerable<SkillUpgradeData> skillUpgradeDatas)
    {
        _playerDataManager = playerDataManager;
        _skillUpgradeDatas = skillUpgradeDatas;
    }

    SkillUpgradeData GetSkillUpgradeData(SkillType skillType) => _skillUpgradeDatas.First(x => x.Level == GetSkillOwnedInfo(skillType).Level);
    PlayerOwnedSkillInfo GetSkillOwnedInfo(SkillType skillType) => _playerDataManager.SkillInventroy.GetSkillInfo(skillType);
    public bool CanUpgrade(SkillType skillType)
    {
        var upgradeData = GetSkillUpgradeData(skillType);
        return GetSkillOwnedInfo(skillType).HasAmount >= upgradeData.NeedExp && _playerDataManager.HasGold(upgradeData.NeedGold);
    }

    public void Upgrade(SkillType skillType)
    {
        if (CanUpgrade(skillType) == false) return;
        var upgradeData = GetSkillUpgradeData(skillType);
        _playerDataManager.TryUseGold(upgradeData.NeedGold);
        _playerDataManager.SkillInventroy.LevelUpSkill(skillType, upgradeData.NeedExp);
    }
}

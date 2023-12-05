using System.Collections;
using System.Collections.Generic;

public interface IDataPersistence
{
    void Save(PlayerDataManager playerData);
}

public class SkillDrawUseCase
{
    readonly SkillDrawer _skillDrawer;
    readonly PlayerDataManager _playerDataManager;
    readonly IDataPersistence _dataPersistence;
    public SkillDrawUseCase(SkillDrawer skillDrawer, PlayerDataManager playerDataManager, IDataPersistence dataPersistence)
    {
        _skillDrawer = skillDrawer;
        _playerDataManager = playerDataManager;
        _dataPersistence = dataPersistence;
    }

    public IEnumerable<SkillDrawResultInfo> DrawSkills(IEnumerable<SkillDrawInfo> drawInfos)
    {
        var result = _skillDrawer.DrawSkills(drawInfos);
        foreach (var resultInfo in result)
            _playerDataManager.SkillInventroy.AddSkill(resultInfo.SkillType, resultInfo.Amount);
        _dataPersistence?.Save(_playerDataManager);
        return result;
    }
}

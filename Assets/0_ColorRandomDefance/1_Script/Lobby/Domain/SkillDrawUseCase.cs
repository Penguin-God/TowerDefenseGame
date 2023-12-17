using System.Collections;
using System.Collections.Generic;

public class SkillDrawUseCase : IProductGiver
{
    readonly SkillDrawer _skillDrawer;
    readonly IEnumerable<SkillDrawInfo> _drawInfos;
    public SkillDrawUseCase(SkillDrawer skillDrawer, IEnumerable<SkillDrawInfo> drawInfos)
    {
        _skillDrawer = skillDrawer;
        _drawInfos = drawInfos;
    }

    public void DrawSkills(IEnumerable<SkillDrawInfo> drawInfos)
    {
        var result = _skillDrawer.DrawSkills(drawInfos);
        //foreach (var resultInfo in result)
        //    _playerDataManager.SkillInventroy.AddSkill(resultInfo.SkillType, resultInfo.Amount);
        // _dataPersistence?.Save(_playerDataManager);
    }

    public void GiveProduct(PlayerDataManager playerDataManager)
    {
        var result = _skillDrawer.DrawSkills(_drawInfos);
        foreach (var resultInfo in result)
            playerDataManager.SkillInventroy.AddSkill(resultInfo.SkillType, resultInfo.Amount);
    }
}

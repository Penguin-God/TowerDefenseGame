using System.Collections;
using System.Collections.Generic;

public class SkillDrawUseCase : IProductGiver
{
    readonly IEnumerable<SkillDrawResultInfo> _resultInfos;
    public SkillDrawUseCase(IEnumerable<SkillDrawResultInfo> resultInfos) => _resultInfos = resultInfos;

    public void GiveProduct(PlayerDataManager playerDataManager)
    {
        foreach (var resultInfo in _resultInfos)
            playerDataManager.SkillInventroy.AddSkill(resultInfo.SkillType, resultInfo.Amount);
    }
}

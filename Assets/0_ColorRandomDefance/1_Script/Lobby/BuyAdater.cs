using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyAdater
{
    readonly IEnumerable<SkillAmountData> _resultInfos;
    public BuyAdater(IEnumerable<SkillAmountData> resultInfos) => _resultInfos = resultInfos;

    public void GiveProduct(PlayerDataManager playerDataManager)
    {
        // »Ì±â
        foreach (var resultInfo in _resultInfos)
            playerDataManager.SkillInventroy.AddSkill(resultInfo.SkillType, resultInfo.Amount);
        Debug.Log("±×¸®±â");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillBoxGoods : UI_Base
{
    enum Texts
    {

    }

    protected override void Init()
    {
        GetComponent<Button>().onClick.AddListener(Buy);
    }

    SkillBoxType _skillBoxType;
    BuyUseCase _buyUseCase;
    public void DependencyInject(SkillBoxType skillBoxType, BuyUseCase buyUseCase)
    {
        _skillBoxType = skillBoxType;
        _buyUseCase = buyUseCase;
    }

    void Buy()
    {
        var drawInfos = new List<SkillDrawInfo>
        {
            new SkillDrawInfo(UserSkillClass.Main, 1, 10),
            new SkillDrawInfo(UserSkillClass.Main, 20, 40),
            new SkillDrawInfo(UserSkillClass.Sub, 30, 80),
        };
    }

    public IEnumerable<SkillDrawInfo> GetDrawInfo()
    {
        var drawInfos = new List<SkillDrawInfo>
        {
            new SkillDrawInfo(UserSkillClass.Main, 1, 10),
            new SkillDrawInfo(UserSkillClass.Main, 20, 40),
            new SkillDrawInfo(UserSkillClass.Sub, 30, 80),
        };
        return drawInfos;
        switch (_skillBoxType)
        {
            case SkillBoxType.희귀상자:
                break;
            case SkillBoxType.고급상자:
                break;
            case SkillBoxType.전설상자:
                break;
        }
    }
}

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

    SkillDrawer _skillDrawer;
    PlayerDataManager _playerDataManager;
    public void DependencyInject(SkillDrawer skillDrawer, PlayerDataManager playerDataManager)
    {
        _skillDrawer = skillDrawer;
        _playerDataManager = playerDataManager;
    }

    void Buy()
    {
        if (_playerDataManager.UseMoney(PlayerMoneyType.Gem, 1000) == false) return;

        var drawInfos = new List<SkillDrawInfo>
        {
            new SkillDrawInfo(UserSkillClass.Main, 1, 10),
            new SkillDrawInfo(UserSkillClass.Main, 20, 40),
            new SkillDrawInfo(UserSkillClass.Sub, 30, 80),
        };
        var result = _skillDrawer.DrawSkills(drawInfos);
        _playerDataManager.AddSkills(result);
    }
}

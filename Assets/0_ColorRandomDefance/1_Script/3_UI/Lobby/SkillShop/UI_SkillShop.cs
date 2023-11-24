using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SkillBoxType
{
    희귀상자,
    고급상자,
    전설상자,
}

public class UI_SkillShop : UI_Popup
{
    enum GameObjects
    {
        BoxGoodsParnet,
    }

    protected override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        CreateBoxGoods();
    }

    SkillDrawUseCase _skillDrawUseCase;
    public void DependencyInject(SkillDrawUseCase skillDrawUseCase)
    {
        _skillDrawUseCase = skillDrawUseCase;
    }

    void CreateBoxGoods()
    {
        foreach (SkillBoxType item in Enum.GetValues(typeof(SkillBoxType)))
            Managers.UI.MakeSubItem<UI_SkillBoxGoods>(GetObject((int)GameObjects.BoxGoodsParnet).transform).DependencyInject(item, _skillDrawUseCase);
    }
}

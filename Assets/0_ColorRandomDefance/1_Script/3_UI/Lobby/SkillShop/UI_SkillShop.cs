using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillShop : UI_Popup
{
    enum GameObjects
    {
        ScrollView,
    }

    protected override void Init()
    {
        base.Init();
        // Bind<GameObject>(typeof(GameObjects));
    }

    public void RefreshUI()
    {
        if (_initDone == false)
        {
            Init();
            _initDone = true;
        }

        var goodsParent = GetObject((int)GameObjects.ScrollView).transform;
        foreach (Transform item in goodsParent)
            Destroy(item.gameObject);

        foreach (var skillData in Managers.Data.UserSkill.AllSkills)
            Managers.UI.MakeSubItem<UI_SkillGoods>(goodsParent).SetInfo(skillData);
    }
}

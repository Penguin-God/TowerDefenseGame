using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShop_UI : UI_Popup
{
    enum GameObjects
    {
        SkillGoodsParent,
    }

    protected override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
    }

    public void RefreshUI()
    {
        if (_initDone == false)
        {
            Init();
            _initDone = true;
        }

        var goodsParent = GetObject((int)GameObjects.SkillGoodsParent).transform;
        foreach (Transform item in goodsParent)
            Destroy(item.gameObject);

        foreach (var skillData in Managers.Data.UserSkill.AllSkills)
            Managers.UI.MakeSubItem<SkillGoods_UI>(goodsParent).SetInfo(skillData);
    }
}

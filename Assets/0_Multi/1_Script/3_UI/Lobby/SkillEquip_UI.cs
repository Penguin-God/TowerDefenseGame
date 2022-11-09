using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEquip_UI : Multi_UI_Popup
{
    enum GameObjects
    {
        SkillFramesParent,
    }

    protected override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        RefreshUI(); // 테스트로 일단 박음
    }

    void RefreshUI()
    {
        RefreshFrames();
    }

    void RefreshFrames()
    {
        var frameParent = GetObject((int)GameObjects.SkillFramesParent).transform;
        foreach (Transform item in frameParent)
            Destroy(item.gameObject);
        foreach (SkillType skillType in Multi_Managers.ClientData.HasSkill)
            Multi_Managers.UI.MakeSubItem<SkillGoodsFrame_UI>(frameParent).SetInfo(new UserSkillMetaData(skillType, 1));
    }
}

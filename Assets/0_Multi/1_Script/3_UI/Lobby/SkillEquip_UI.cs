using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillEquip_UI : Multi_UI_Popup
{
    enum GameObjects
    {
        HasSkillFramesParent,
    }

    enum Images
    {
        EquipSkill1_Image,
        EquipSkill2_Image,
    }

    protected override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        RefreshUI(); // 테스트로 일단 박음
        Multi_Managers.ClientData.EquipSkillManager.OnEquipSkillChanged -= RefreshEquipSkillFrame;
        Multi_Managers.ClientData.EquipSkillManager.OnEquipSkillChanged += RefreshEquipSkillFrame;
    }

    void OnDestroy()
    {
        Multi_Managers.ClientData.EquipSkillManager.OnEquipSkillChanged -= RefreshEquipSkillFrame;
    }

    void RefreshUI()
    {
        RefreshHasSkillsFrame();
    }

    void RefreshHasSkillsFrame()
    {
        var frameParent = GetObject((int)GameObjects.HasSkillFramesParent).transform;
        foreach (Transform item in frameParent)
            Destroy(item.gameObject);
        foreach (SkillType skillType in Multi_Managers.ClientData.HasSkill)
            Multi_Managers.UI.MakeSubItem<SkillGoodsFrame_UI>(frameParent).SetInfo(new UserSkillMetaData(skillType, 1));
    }

    void RefreshEquipSkillFrame(SkillEquipData equipData)
    {
        if (equipData.IsEquip == false) return;

        switch (equipData.SkillClass)
        {
            case UserSkillClass.Main:
                GetImage((int)Images.EquipSkill1_Image).sprite = Multi_Managers.Resources.Load<Sprite>(Multi_Managers.Data.GetUserSkillGoodsData(new UserSkillMetaData(equipData.SkillType, 1)).ImagePath);
                break;
            case UserSkillClass.Sub:
                GetImage((int)Images.EquipSkill2_Image).sprite = Multi_Managers.Resources.Load<Sprite>(Multi_Managers.Data.GetUserSkillGoodsData(new UserSkillMetaData(equipData.SkillType, 1)).ImagePath);
                break;
        }
    }
}

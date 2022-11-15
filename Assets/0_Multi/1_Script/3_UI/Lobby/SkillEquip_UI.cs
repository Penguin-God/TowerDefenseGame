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

    enum Buttons
    {
        UnEquipButton,
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
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.UnEquipButton).onClick.AddListener(Multi_Managers.ClientData.EquipSkillManager.AllUnEquip);

        Multi_Managers.ClientData.EquipSkillManager.OnEquipSkillChanged -= RefreshEquipSkillFrame;
        Multi_Managers.ClientData.EquipSkillManager.OnEquipSkillChanged += RefreshEquipSkillFrame;
    }

    void OnDestroy()
    {
        Multi_Managers.ClientData.EquipSkillManager.OnEquipSkillChanged -= RefreshEquipSkillFrame;
    }

    public void RefreshUI()
    {
        if (_initDone == false)
        {
            Init();
            _initDone = true;
        }
        RefreshHasSkillsFrame();
    }

    void RefreshHasSkillsFrame()
    {
        var frameParent = GetObject((int)GameObjects.HasSkillFramesParent).transform;
        foreach (Transform item in frameParent)
            Destroy(item.gameObject);
        foreach (SkillType skillType in Multi_Managers.ClientData.HasSkills)
            Multi_Managers.UI.MakeSubItem<SkillGoodsFrame_UI>(frameParent).SetInfo(skillType);
    }

    void RefreshEquipSkillFrame(SkillEquipData equipData)
    {
        switch (equipData.SkillClass)
        {
            case UserSkillClass.Main:
                SetEquipImage(equipData, GetImage((int)Images.EquipSkill1_Image));
                break;
            case UserSkillClass.Sub:
                SetEquipImage(equipData, GetImage((int)Images.EquipSkill2_Image));
                break;
        }
    }

    void SetEquipImage(SkillEquipData equipData, Image image)
    {
        if(equipData.IsEquip == false)
        {
            image.color = new Color(1, 1, 1, 0);
            return;
        }

        image.color = new Color(1, 1, 1, 1);
        image.sprite = GetSkillImage(equipData.SkillType);
    }

    Sprite GetSkillImage(SkillType skillType) => Multi_Managers.Resources.Load<Sprite>(Multi_Managers.Data.UserSkill.GetSkillGoodsData(skillType).ImagePath);
}

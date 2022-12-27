using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillEquip_UI : UI_Popup
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

        GetButton((int)Buttons.UnEquipButton).onClick.AddListener(Managers.ClientData.EquipSkillManager.AllUnEquip);

        Managers.ClientData.EquipSkillManager.OnEquipSkillChanged -= RefreshEquipSkillFrame;
        Managers.ClientData.EquipSkillManager.OnEquipSkillChanged += RefreshEquipSkillFrame;
    }

    void OnDestroy()
    {
        Managers.ClientData.EquipSkillManager.OnEquipSkillChanged -= RefreshEquipSkillFrame;
    }

    public void RefreshUI()
    {
        if (_initDone == false)
        {
            Init();
            _initDone = true;
        }
        RefreshHasSkillsFrame();
        RefreshEquipSkillFrame();
    }

    void RefreshHasSkillsFrame()
    {
        var frameParent = GetObject((int)GameObjects.HasSkillFramesParent).transform;
        foreach (Transform item in frameParent)
            Destroy(item.gameObject);
        foreach (SkillType skillType in Managers.ClientData.HasSkills)
            Managers.UI.MakeSubItem<SkillFrame_UI>(frameParent).SetInfo(skillType);
    }

    void RefreshEquipSkillFrame()
    {
        SetEquipImage(Managers.ClientData.EquipSkillManager.MainSkill, GetImage((int)Images.EquipSkill1_Image));
        SetEquipImage(Managers.ClientData.EquipSkillManager.SubSkill, GetImage((int)Images.EquipSkill2_Image));
    }

    void RefreshEquipSkillFrame(SkillEquipData equipData)
    {
        switch (equipData.SkillClass)
        {
            case UserSkillClass.Main:
                SetEquipImage(equipData.SkillType, GetImage((int)Images.EquipSkill1_Image));
                break;
            case UserSkillClass.Sub:
                SetEquipImage(equipData.SkillType, GetImage((int)Images.EquipSkill2_Image));
                break;
        }
    }

    void SetEquipImage(SkillType skillType, Image image)
    {
        if (skillType == SkillType.None)
        {
            image.color = new Color(1, 1, 1, 0);
            return;
        }

        image.color = new Color(1, 1, 1, 1);
        image.sprite = GetSkillImage(skillType);
    }

    Sprite GetSkillImage(SkillType skillType) => Managers.Resources.Load<Sprite>(Managers.Data.UserSkill.GetSkillGoodsData(skillType).ImagePath);
}

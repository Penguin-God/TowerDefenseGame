using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillManagementWindow : UI_Popup
{
    enum GameObjects
    {
        HasSkillFramesParent,
    }

    enum Buttons
    {
        UnEquipButton,
        MainTabBtn,
        SubTabBtn,
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
        GetButton((int)Buttons.MainTabBtn).onClick.AddListener(() => ChangeTab(UserSkillClass.Main));
        GetButton((int)Buttons.SubTabBtn).onClick.AddListener(() => ChangeTab(UserSkillClass.Sub));

        Managers.ClientData.EquipSkillManager.OnEquipSkillChanged -= DrawEquipSkillFrame;
        Managers.ClientData.EquipSkillManager.OnEquipSkillChanged += DrawEquipSkillFrame;
    }

    void OnDestroy()
    {
        Managers.ClientData.EquipSkillManager.OnEquipSkillChanged -= DrawEquipSkillFrame;
    }

    public void RefreshUI()
    {
        if (_initDone == false)
        {
            Init();
            _initDone = true;
        }

        ChangeTab(UserSkillClass.Main);
        RefreshEquipSkillFrame();
    }

    [SerializeField] Sprite _defualtSp;
    [SerializeField] Sprite _selectSp;
    void ChangeTab(UserSkillClass skillClass)
    {
        switch (skillClass)
        {
            case UserSkillClass.Main:
                GetButton((int)Buttons.MainTabBtn).image.sprite = _selectSp;
                GetButton((int)Buttons.SubTabBtn).image.sprite = _defualtSp;
                break;
            case UserSkillClass.Sub:
                GetButton((int)Buttons.MainTabBtn).image.sprite = _defualtSp;
                GetButton((int)Buttons.SubTabBtn).image.sprite = _selectSp;
                break;
        }
        
        DrawSkillImages(Managers.ClientData.HasSkills.Where(x => Managers.Data.UserSkill.GetSkillGoodsData(x).SkillClass == skillClass));
    }

    void DrawSkillImages(IEnumerable<SkillType> skills)
    {
        var frameParent = GetObject((int)GameObjects.HasSkillFramesParent).transform;
        foreach (Transform item in frameParent)
            Destroy(item.gameObject);
        foreach (SkillType skillType in skills)
            Managers.UI.MakeSubItem<UI_SkillFrame>(frameParent).SetInfo(skillType);
    }

    void RefreshEquipSkillFrame()
    {
        DrawEquipSkillFrame(UserSkillClass.Main, Managers.ClientData.EquipSkillManager.MainSkill);
        DrawEquipSkillFrame(UserSkillClass.Sub, Managers.ClientData.EquipSkillManager.SubSkill);
    }

    void DrawEquipSkillFrame(UserSkillClass skillClass, SkillType skillType)
    {
        switch (skillClass)
        {
            case UserSkillClass.Main:
                SetEquipImage(skillType, GetImage((int)Images.EquipSkill1_Image));
                break;
            case UserSkillClass.Sub:
                SetEquipImage(skillType, GetImage((int)Images.EquipSkill2_Image));
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

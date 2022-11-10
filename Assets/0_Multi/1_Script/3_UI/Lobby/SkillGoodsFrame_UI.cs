using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillGoodsFrame_UI : Multi_UI_Base
{
    enum Buttons
    {
        EquipButton,
    }

    enum Texts
    {
        NameText,
    }

    enum Images
    {
        Skill_Image,
    }

    protected override void Init()
    {
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        _initDone = true;
    }

    public void SetInfo(UserSkillMetaData data)
    {
        if (_initDone == false)
            Init();
        RefreshUI(Multi_Managers.Data.GetUserSkillGoodsData(data));
    }

    void RefreshUI(UserSkillGoodsData data)
    {
        GetText((int)Texts.NameText).text = data.SkillName;

        GetButton((int)Buttons.EquipButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.EquipButton).onClick.AddListener(() => Multi_Managers.ClientData.EquipSkillManager.ChangedEquipSkill(data.SkillClass, data.SkillType));

        GetImage((int)Images.Skill_Image).sprite = Multi_Managers.Resources.Load<Sprite>(data.ImagePath);
    }
}

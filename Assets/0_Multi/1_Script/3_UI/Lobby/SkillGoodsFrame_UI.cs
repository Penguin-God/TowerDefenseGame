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

    protected void _Init()
    {
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
    }

    public void SetInfo(UserSkillMetaData data)
    {
        _Init();
        RefreshUI(Multi_Managers.Data.GetUserSkillGoodsData(data));
    }

    void RefreshUI(UserSkillGoodsData data)
    {
        GetText((int)Texts.NameText).text = data.SkillName;

        GetButton((int)Buttons.EquipButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.EquipButton).onClick.AddListener(() => new UserSkillBuyUseCase().Buy(data.MetaData));

        GetImage((int)Images.Skill_Image).sprite = Multi_Managers.Resources.Load<Sprite>(data.ImagePath);
    }
}

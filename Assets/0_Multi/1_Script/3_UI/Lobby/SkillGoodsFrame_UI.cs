using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillGoodsFrame_UI : Multi_UI_Base
{
    enum Buttons
    {
        EquipButton,
        Skill_ImageButton,
    }

    enum Texts
    {
        NameText,
    }

    enum Images
    {
        Skill_ImageButton,
    }

    protected override void Init()
    {
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        _initDone = true;
        RefreshUI();
    }

    UserSkillGoodsData _skillData = null;
    public void SetInfo(SkillType skill)
    {
        _skillData = Multi_Managers.Data.UserSkill.GetSkillGoodsData(skill);
        RefreshUI();
    }

    void RefreshUI()
    {
        if (_initDone == false || _skillData == null) return;

        GetText((int)Texts.NameText).text = _skillData.SkillName;

        GetButton((int)Buttons.EquipButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.EquipButton).onClick.AddListener(() => Multi_Managers.ClientData.EquipSkillManager.ChangedEquipSkill(_skillData.SkillClass, _skillData.SkillType));

        GetImage((int)Images.Skill_ImageButton).sprite = Multi_Managers.Resources.Load<Sprite>(_skillData.ImagePath);

        GetButton((int)Buttons.Skill_ImageButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.Skill_ImageButton).onClick.AddListener(() => Multi_Managers.UI.ShowPopupUI<Skill_Info_UI>().SetInfo(_skillData));
    }
}

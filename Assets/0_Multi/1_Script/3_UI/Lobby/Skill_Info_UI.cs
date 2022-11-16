using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Info_UI : Multi_UI_Popup
{
    enum Images
    {
        Barrier,
        Skill_Image,
        FillMask,
    }

    enum Texts
    {
        SkillName,
        SkillExplaneText,
        Exp_Text,
    }

    enum Buttons
    {
        UpgradeButton,
    }

    protected override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        _initDone = true;
        RefreshUI();
    }

    public void SetInfo(UserSkillGoodsData newData)
    {
        _skillData = newData;
        RefreshUI();
    }

    UserSkillGoodsData _skillData = null;
    public void RefreshUI()
    {
        if (_initDone == false || _skillData == null) return;

        GetText((int)Texts.SkillName).text = _skillData.SkillName;
        GetText((int)Texts.SkillExplaneText).text = _skillData.Description;
        GetImage((int)Images.Skill_Image).sprite = Multi_Managers.Resources.Load<Sprite>(_skillData.ImagePath);

        var levelData = Multi_Managers.ClientData.GetSkillLevelData(_skillData.SkillType);
        GetText((int)Texts.Exp_Text).text = $"{Multi_Managers.ClientData.SkillByExp[_skillData.SkillType]} / {levelData.Exp}";
        GetImage((int)Images.FillMask).fillAmount = (float)Multi_Managers.ClientData.SkillByExp[_skillData.SkillType] / levelData.Exp;

        GetButton((int)Buttons.UpgradeButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.UpgradeButton).onClick.AddListener(UpgradeSkill);
    }

    void UpgradeSkill()
    {
        if (_skillData == null) return;

        if (Multi_Managers.ClientData.UpgradeSkill(_skillData.SkillType))
            RefreshUI();
    }
}

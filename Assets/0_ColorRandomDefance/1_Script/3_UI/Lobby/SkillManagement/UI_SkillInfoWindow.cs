using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillInfoWindow : UI_Popup
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

    enum GameObjects
    {
        SkillStatInfoRoot,
    }

    protected override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
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
        GetImage((int)Images.Skill_Image).sprite = SpriteUtility.GetSkillImage(_skillData.SkillType);

        var levelData = Managers.ClientData.GetSkillLevelData(_skillData.SkillType);
        GetText((int)Texts.Exp_Text).text = $"{Managers.ClientData.SkillByExp[_skillData.SkillType]} / {levelData.Exp}";
        GetImage((int)Images.FillMask).fillAmount = (float)Managers.ClientData.SkillByExp[_skillData.SkillType] / levelData.Exp;

        GetButton((int)Buttons.UpgradeButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.UpgradeButton).onClick.AddListener(UpgradeSkill);
        ShowSkillStat();
    }

    void UpgradeSkill()
    {
        if (_skillData == null) return;

        if (Managers.ClientData.UpgradeSkill(_skillData.SkillType))
            RefreshUI();
    }

    void ShowSkillStat()
    {
        foreach (Transform child in GetObject((int)GameObjects.SkillStatInfoRoot).transform)
            Destroy(child.gameObject);

        if (_skillData.StatInfoFraems == null || _skillData.StatInfoFraems.Count() == 0) return;
        for (int i = 0; i < _skillData.StatInfoFraems.Count(); i++)
            Managers.UI.MakeSubItem<UI_SkillStatInfo>(GetObject((int)GameObjects.SkillStatInfoRoot).transform).ShowSkillStat(_skillData.SkillType, i);
    }
}

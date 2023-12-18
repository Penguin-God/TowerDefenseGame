using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillInfoWindow : UI_Popup
{
    enum Images
    {
        Skill_Image,
        FillMask,
    }

    enum Texts
    {
        SkillName,
        SkillExplaneText,
        Exp_Text,
        LevelText,
        GoldText,
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
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
    }

    SkillUpgradeUseCase _skillUpgradeUseCase;
    SkillInfoPresenter _skillInfoPresenter;
    public void Show(SkillInfoPresenter skillInfoPresenter, SkillUpgradeUseCase skillUpgradeUseCase)
    {
        _skillInfoPresenter = skillInfoPresenter;
        _skillUpgradeUseCase = skillUpgradeUseCase;
        RefreshUI();
    }

    void RefreshUI()
    {
        CheckInit();

        GetTextMeshPro((int)Texts.SkillName).text = _skillInfoPresenter.GetSkillName();
        GetTextMeshPro((int)Texts.SkillExplaneText).text = _skillInfoPresenter.GetSkillDescription();
        GetImage((int)Images.Skill_Image).sprite = _skillInfoPresenter.GetSkillImage();

        GetTextMeshPro((int)Texts.LevelText).text = _skillInfoPresenter.GetSkillLevel().ToString();
        GetTextMeshPro((int)Texts.Exp_Text).text = _skillInfoPresenter.GetExpGaugeText();
        GetImage((int)Images.FillMask).fillAmount = _skillInfoPresenter.GetExpGaugeAmount();

        GetTextMeshPro((int)Texts.GoldText).text = $"X{_skillInfoPresenter.GetGoldForUpgrade()}";
        GetButton((int)Buttons.UpgradeButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.UpgradeButton).onClick.AddListener(UpgradeSkill);
        ShowSkillStat();
    }

    void UpgradeSkill()
    {
        print("@");
        if (_skillUpgradeUseCase.CanUpgrade(_skillInfoPresenter.SkillType))
        {
            print("s");
            _skillUpgradeUseCase.Upgrade(_skillInfoPresenter.SkillType);
            RefreshUI();
        }
    }

    void ShowSkillStat()
    {
        foreach (Transform child in GetObject((int)GameObjects.SkillStatInfoRoot).transform)
            Destroy(child.gameObject);

        //if (_skillData.StatInfoFraems == null || _skillData.StatInfoFraems.Count() == 0) return;
        //for (int i = 0; i < _skillData.StatInfoFraems.Count(); i++)
        //    Managers.UI.MakeSubItem<UI_SkillStatInfo>(GetObject((int)GameObjects.SkillStatInfoRoot).transform).ShowSkillStat(_skillData.SkillType, i);
    }
}

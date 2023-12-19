using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillFrame : UI_Base
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
    }

    SkillUpgradeUseCase _skillUpgradeUseCase;
    SkillInfoPresenter _skillInfoPresenter;
    EquipSkillManager _equipSkillManager;
    public void SetInfo(SkillInfoPresenter skillInfoPresenter, SkillUpgradeUseCase skillUpgradeUseCase, EquipSkillManager equipSkillManager)
    {
        _skillInfoPresenter = skillInfoPresenter;
        _skillUpgradeUseCase = skillUpgradeUseCase;
        _equipSkillManager = equipSkillManager;
        RefreshUI();
    }

    void RefreshUI()
    {
        CheckInit();

        GetText((int)Texts.NameText).text = _skillInfoPresenter.GetSkillName();

        GetButton((int)Buttons.EquipButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.EquipButton).onClick.AddListener(() => _equipSkillManager.ChangedEquipSkill(_skillInfoPresenter.GetSkillClass(), _skillInfoPresenter.SkillType));

        GetImage((int)Images.Skill_ImageButton).sprite = _skillInfoPresenter.GetSkillImage();

        GetButton((int)Buttons.Skill_ImageButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.Skill_ImageButton).onClick.AddListener(() => Managers.UI.ShowPopupUI<UI_SkillInfoWindow>().Show(_skillInfoPresenter, _skillUpgradeUseCase));
    }
}

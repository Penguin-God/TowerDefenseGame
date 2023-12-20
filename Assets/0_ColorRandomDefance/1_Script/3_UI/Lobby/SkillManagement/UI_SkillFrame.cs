using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillFrame : UI_Base
{
    enum Buttons
    {
        EquipButton,
    }

    protected override void Init()
    {
        Bind<Button>(typeof(Buttons));
    }

    SkillUpgradeUseCase _skillUpgradeUseCase;
    SkillInfoPresenter _skillInfoPresenter;
    EquipSkillManager _equipSkillManager;
    UI_SkillFrameView _skillFrameView;
    public void SetInfo(SkillInfoPresenter skillInfoPresenter, SkillUpgradeUseCase skillUpgradeUseCase, EquipSkillManager equipSkillManager)
    {
        _skillInfoPresenter = skillInfoPresenter;
        _skillUpgradeUseCase = skillUpgradeUseCase;
        _equipSkillManager = equipSkillManager;
        _skillFrameView = gameObject.GetOrAddComponent<UI_SkillFrameView>();
        RefreshUI();
    }

    void RefreshUI()
    {
        CheckInit();

        GetButton((int)Buttons.EquipButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.EquipButton).onClick.AddListener(() => _equipSkillManager.ChangedEquipSkill(_skillInfoPresenter.GetSkillClass(), _skillInfoPresenter.SkillType));
        _skillFrameView.RefreshUI(_skillInfoPresenter, _skillUpgradeUseCase);
    }
}

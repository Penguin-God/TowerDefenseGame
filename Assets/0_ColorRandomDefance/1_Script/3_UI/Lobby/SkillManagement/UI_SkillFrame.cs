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
        _initDone = true;
        RefreshUI();
    }

    UserSkillGoodsData _skillData = null;
    SkillUpgradeUseCase _skillUpgradeUseCase;
    PlayerDataManager _playerDataManager;
    public void SetInfo(SkillType skill, SkillUpgradeUseCase skillUpgradeUseCase, PlayerDataManager playerDataManager)
    {
        _skillData = Managers.Data.UserSkill.GetSkillGoodsData(skill);
        _skillUpgradeUseCase = skillUpgradeUseCase;
        _playerDataManager = playerDataManager;
        RefreshUI();
    }

    void RefreshUI()
    {
        if (_initDone == false || _skillData == null) return;

        GetText((int)Texts.NameText).text = _skillData.SkillName;

        GetButton((int)Buttons.EquipButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.EquipButton).onClick.AddListener(() => Managers.ClientData.EquipSkillManager.ChangedEquipSkill(_skillData.SkillClass, _skillData.SkillType));

        GetImage((int)Images.Skill_ImageButton).sprite = SpriteUtility.GetSkillImage(_skillData.SkillType);

        GetButton((int)Buttons.Skill_ImageButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.Skill_ImageButton).onClick.AddListener(() => Managers.UI.ShowPopupUI<UI_SkillInfoWindow>().SetInfo(_skillData, _skillUpgradeUseCase, _playerDataManager));
    }
}

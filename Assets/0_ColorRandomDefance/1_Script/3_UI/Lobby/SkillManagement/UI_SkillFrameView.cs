using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillFrameView : UI_Base
{
    enum Buttons
    {
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
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
    }

    public void RefreshUI(SkillInfoPresenter skillInfoPresenter, SkillUpgradeUseCase skillUpgradeUseCase)
    {
        CheckInit();

        GetTextMeshPro((int)Texts.NameText).text = skillInfoPresenter.GetSkillName();

        GetImage((int)Images.Skill_ImageButton).sprite = skillInfoPresenter.GetSkillImage();

        GetButton((int)Buttons.Skill_ImageButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.Skill_ImageButton).onClick.AddListener(() => Managers.UI.ShowPopupUI<UI_SkillInfoWindow>().Show(skillInfoPresenter, skillUpgradeUseCase));
    }
}

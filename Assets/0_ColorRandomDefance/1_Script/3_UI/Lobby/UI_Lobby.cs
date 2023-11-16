using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : UI_Scene
{
    enum Buttons
    {
        GetSkillButton,
        OpenShopButton,
        OpenSkillButton,
        GameStartButton,
    }

    enum Texts
    {
        TestText,
    }

    protected override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));

        GetButton((int)Buttons.GetSkillButton).onClick.AddListener(GetAllSkill);
        GetButton((int)Buttons.OpenShopButton).onClick.AddListener(ShowShop);
        GetButton((int)Buttons.OpenSkillButton).onClick.AddListener(ShowSkillWindow);

        _container.GetComponent<GameMatchmaker>().SetInfo(GetTextMeshPro((int)Texts.TestText), GetButton((int)Buttons.GameStartButton));
    }

    BattleDIContainer _container;
    public void DependencyInject(BattleDIContainer container) => _container = container;

    public void ShowSkillWindow() => Managers.UI.ShowPopupUI<UI_SkillManagementWindow>().RefreshUI();
    public void ShowShop() => Managers.UI.ShowPopupUI<UI_SkillShop>("UI_LobbyShop").RefreshUI();

    void GetAllSkill()
    {
        foreach (SkillType type in Enum.GetValues(typeof(SkillType)))
            new UserSkillShopUseCase().GetSkillExp(type, 1);
    }
}

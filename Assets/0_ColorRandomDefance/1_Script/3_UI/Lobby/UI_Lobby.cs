using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : UI_Scene
{
    enum Buttons
    {
        OpenShopButton,
        OpenSkillButton,
        GameStartButton,
    }

    enum Texts
    {
        TestText,
        GoldText,
        ScoreText,
        GemText,
    }

    protected override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));

        GetButton((int)Buttons.OpenShopButton).onClick.AddListener(ShowShop);
        GetButton((int)Buttons.OpenSkillButton).onClick.AddListener(ShowSkillWindow);

        _container.GetComponent<GameMatchmaker>().SetInfo(GetTextMeshPro((int)Texts.TestText), GetButton((int)Buttons.GameStartButton), _container.GetService<EquipSkillManager>());

        var playerData = _container.GetService<PlayerDataManager>();
        playerData.OnGoldAmountChanged += amount => GetTextMeshPro((int)Texts.GoldText).text = amount.ToString();
        playerData.OnGemAmountChanged += amount => GetTextMeshPro((int)Texts.GemText).text = amount.ToString();
        playerData.OnChangeScore += amount => GetTextMeshPro((int)Texts.ScoreText).text = amount.ToString();
    }

    BattleDIContainer _container;
    public void DependencyInject(BattleDIContainer container) => _container = container;

    T ShowPopup<T>() where T : UI_Popup
    {
        var ui = Managers.UI.ShowPopupUI<T>();
        _container.Inject(ui);
        return ui;
    }

    void ShowSkillWindow() => ShowPopup<UI_SkillManagementWindow>().RefreshUI();
    void ShowShop() => ShowPopup<UI_LobbyShop>();
}

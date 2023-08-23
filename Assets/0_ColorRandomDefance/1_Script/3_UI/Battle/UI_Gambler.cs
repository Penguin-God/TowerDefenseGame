using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Gambler : UI_Base
{
    enum UI_State
    {
        Defautl,
        Exp,
        Gacha,
    }

    enum GameObjects
    {
        SummonUnitButton,
    }

    protected enum Buttons
    {
        UnitSummonSwichButton,
        BuyExpButton,
    }

    enum Texts
    {
        GambleLevelText,
        ExpStatusText,
    }

    LevelSystem _gambleLevelSystem;
    UnityAction _buyExp;
    public void Inject(LevelSystem levelSystem, UnityAction buyExp)
    {
        _gambleLevelSystem = levelSystem;
        _buyExp = buyExp;
    }

    protected override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        Managers.Camera.OnLookEnemyWorld += InActiveAllButtons;
        Managers.Camera.OnLookMyWolrd += UpdateUIState;

        GetTextMeshPro((int)Texts.GambleLevelText).raycastTarget = false;
        GetTextMeshPro((int)Texts.ExpStatusText).raycastTarget = false;
        _gambleLevelSystem.OnChangeExp += _ => UpdateText();

        GetButton((int)Buttons.UnitSummonSwichButton).onClick.AddListener(SwitchExpDefault);
        GetButton((int)Buttons.BuyExpButton).onClick.AddListener(_buyExp);
        
        UpdateText();
        UpdateUIState();
    }

    UI_State _currentState = UI_State.Defautl;
    void ChangeState(UI_State state)
    {
        _currentState = state;
        UpdateUIState();
    }
    void UpdateUIState()
    {
        switch (_currentState)
        {
            case UI_State.Defautl:
                ToggleDefaultButton(true);
                ToggleExpUI(false);
                break;
            case UI_State.Exp:
                ToggleDefaultButton(false);
                ToggleExpUI(true);
                break;
            case UI_State.Gacha:
                break;
        }
    }

    void SwitchExpDefault()
    {
        if (_currentState == UI_State.Defautl)
            ChangeState(UI_State.Exp);
        else if (_currentState == UI_State.Exp)
            ChangeState(UI_State.Defautl);
    }

    void InActiveAllButtons()
    {
        GetButton((int)Buttons.UnitSummonSwichButton).gameObject.SetActive(false);
        ToggleDefaultButton(false);
        ToggleExpUI(false);
    }

    void ToggleExpUI(bool isActive)
    {
        GetTextMeshPro((int)Texts.GambleLevelText).gameObject.SetActive(isActive);
        GetTextMeshPro((int)Texts.ExpStatusText).gameObject.SetActive(isActive);
        GetButton((int)Buttons.BuyExpButton).gameObject.SetActive(isActive);
    }
    
    void ToggleDefaultButton(bool isActive) => GetObject((int)GameObjects.SummonUnitButton).gameObject.SetActive(isActive);

    void UpdateText()
    {
        GetTextMeshPro((int)Texts.GambleLevelText).text = $"LV : {_gambleLevelSystem.Level}";
        GetTextMeshPro((int)Texts.ExpStatusText).text = $"EXP : {_gambleLevelSystem.Experience} / {_gambleLevelSystem.NeedExperienceForLevelUp}";
    }
}

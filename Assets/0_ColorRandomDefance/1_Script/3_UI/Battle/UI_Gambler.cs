using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        GachaUnitInfoParent,
    }

    protected enum Buttons
    {
        UnitSummonSwichButton,
        BuyExpButton,
        UnitGachaButton,
    }

    enum Texts
    {
        GambleLevelText,
        ExpStatusText,
    }

    
    // LevelSystem _gambleLevelSystem;
    //UnityAction _buyExp;
    //UnityAction _gachaAndLevelUp;
    //Func<int[]> _getRates;
    //public void Inject(LevelSystem levelSystem, UnityAction buyExp, Func<int[]> getRates, UnityAction gachaAndLevelUp)
    //{
    //    _gambleLevelSystem = levelSystem;
    //    _buyExp = buyExp;
    //    _getRates = getRates;
    //    _gachaAndLevelUp = gachaAndLevelUp;
    //}

    GamblerController _gamblerController;
    GamblerLevelSystem GamblerLevelSystem => _gamblerController.LevelSystem;
    int _expPrice;
    int _expAmount;
    public void Inject(GamblerController gamblerController, int expPrice, int expAmount)
    {
        _gamblerController = gamblerController;
        _expPrice = expPrice;
        _expAmount = expAmount;
    }

    protected override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        Managers.Camera.OnLookEnemyWorld += InActiveAllUIs;
        Managers.Camera.OnLookMyWolrd += OnLookMyWorld;

        GetTextMeshPro((int)Texts.GambleLevelText).raycastTarget = false;
        GetTextMeshPro((int)Texts.ExpStatusText).raycastTarget = false;
        //_gambleLevelSystem.OnChangeExp += _ => UpdateText();
        //_gambleLevelSystem.OnLevelUp += _ => UpdateText(); // 레벨 업 로직 딴데 있어서 일단 있어야 함
        //_gambleLevelSystem.OnChangeExp += _ => ToGachaWindow();
        
        GamblerLevelSystem.OnChangeExp += _UpdateText;
        GamblerLevelSystem.OnOverExp += () => ChangeState(UI_State.Gacha);

        GetButton((int)Buttons.UnitSummonSwichButton).onClick.AddListener(SwitchExpDefault);
        GetButton((int)Buttons.BuyExpButton).onClick.AddListener(BuyExp);
        //GetButton((int)Buttons.UnitGachaButton).onClick.AddListener(_gachaAndLevelUp);
        //GetButton((int)Buttons.UnitGachaButton).onClick.AddListener(() => ChangeState(UI_State.Exp));
        GetButton((int)Buttons.UnitGachaButton).onClick.AddListener(GambleAndLevelUp);

        // UpdateText();
        _UpdateText();
        InActiveAllUIs();
        UpdateUIState();
    }

    void GambleAndLevelUp()
    {
        _gamblerController.GambleAndLevelUp();
        _UpdateText();
        ChangeState(UI_State.Exp);
    }

    void BuyExp() => _gamblerController.BuyExp(_expPrice, _expAmount);

    void _UpdateText()
    {
        GetTextMeshPro((int)Texts.GambleLevelText).text = $"LV : {GamblerLevelSystem.Level}";
        GetTextMeshPro((int)Texts.ExpStatusText).text = $"EXP : {GamblerLevelSystem.Experience} / {GamblerLevelSystem.NeedExperienceForLevelUp}";
    }

    //void UpdateText()
    //{
    //    GetTextMeshPro((int)Texts.GambleLevelText).text = $"LV : {_gambleLevelSystem.Level}";
    //    GetTextMeshPro((int)Texts.ExpStatusText).text = $"EXP : {_gambleLevelSystem.Experience} / {_gambleLevelSystem.NeedExperienceForLevelUp}";
    //}

    UI_State _currentState = UI_State.Defautl;
    void ChangeState(UI_State state)
    {
        ExitState(_currentState);
        _currentState = state;
        UpdateUIState();
    }

    //void ToGachaWindow()
    //{
    //    if (_gambleLevelSystem.LevelUpCondition)
    //        ChangeState(UI_State.Gacha);
    //}

    void ExitState(UI_State state)
    {
        switch (state)
        {
            case UI_State.Defautl:
                ToggleDefaultButton(false);
                break;
            case UI_State.Exp:
                ToggleExpUI(false);
                break;
            case UI_State.Gacha:
                ToggleGachaUI(false);
                DestoryItemInfos();
                break;
        }
    }

    void UpdateUIState()
    {
        switch (_currentState)
        {
            case UI_State.Defautl:
                ToggleSwitchButton(true);
                ToggleDefaultButton(true);
                break;
            case UI_State.Exp:
                ToggleSwitchButton(true);
                ToggleExpUI(true);
                break;
            case UI_State.Gacha:
                ToggleSwitchButton(false);
                ToggleGachaUI(true);
                ShowGachaItemInfos();
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

    void InActiveAllUIs()
    {
        ToggleSwitchButton(false);
        ToggleDefaultButton(false);
        ToggleExpUI(false);
        ToggleGachaUI(false);
    }

    void ToggleExpUI(bool isActive)
    {
        GetTextMeshPro((int)Texts.GambleLevelText).gameObject.SetActive(isActive);
        GetTextMeshPro((int)Texts.ExpStatusText).gameObject.SetActive(isActive);
        GetButton((int)Buttons.BuyExpButton).gameObject.SetActive(isActive);
    }
    
    void ToggleDefaultButton(bool isActive) => GetObject((int)GameObjects.SummonUnitButton).gameObject.SetActive(isActive);
    void ToggleSwitchButton(bool isActive) => GetButton((int)Buttons.UnitSummonSwichButton).gameObject.SetActive(isActive);
    void ToggleGachaUI(bool isActive)
    {
        GetObject((int)GameObjects.GachaUnitInfoParent).SetActive(isActive);
        GetButton((int)Buttons.UnitGachaButton).gameObject.SetActive(isActive);
    }

    void ShowGachaItemInfos()
    {
        DestoryItemInfos();

        foreach (var item in _gamblerController.CreateUnitGachaData())
            Managers.UI.MakeSubItem<UI_UnitGachaItemInfo>(GetObject((int)GameObjects.GachaUnitInfoParent).transform)
                .ShowInfo(item.GachaUnitFalgItems.First().UnitClass, item.Rate);
    }

    void DestoryItemInfos()
    {
        foreach (Transform child in GetObject((int)GameObjects.GachaUnitInfoParent).transform)
            Destroy(child.gameObject);
    }

    void OnLookMyWorld() => StartCoroutine(Co_OnLookMyWorld());
    // 같은 오브젝트에 붙어있는 UI_BattleButtons 컴포넌트가 카메라 이동 시 UnitSommonButton의 활성화 상태를 조작하기에 여기서 추가 작업을 해야 됨
    IEnumerator Co_OnLookMyWorld()
    {
        yield return null;
        if(_currentState != UI_State.Defautl)
            ToggleDefaultButton(false);
        UpdateUIState();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Gambler : UI_Base
{
    enum GameObjects
    {
        SummonUnitButton,
    }

    protected enum Buttons
    {
        UnitSummonSwichButton,
        AddExpButton,
    }

    enum Texts
    {
        GambleLevelText,
        ExpStatusText,
    }

    LevelSystem _gamblerLevelSystem;
    int _addExpAmount;
    public void Inject(LevelSystem levelSystem, int addExpAmount)
    {
        _gamblerLevelSystem = levelSystem;
        _addExpAmount = addExpAmount;
    }

    protected override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        Managers.Camera.OnIsLookMyWolrd += OnWorldChange;

        _gamblerLevelSystem.OnChangeExp += _ => UpdateText();
        GetButton((int)Buttons.UnitSummonSwichButton).onClick.AddListener(SwitchButtons);
        GetButton((int)Buttons.AddExpButton).onClick.AddListener(AddExp);
        ApplyButtonEnable();
        UpdateText();
    }

    void OnWorldChange(bool isLooyMy)
    {
        if(_isShowExpButton)
            ToggleExpButton(isLooyMy);
    }

    bool _isShowExpButton = false;
    void SwitchButtons()
    {
        _isShowExpButton = !_isShowExpButton;
        ApplyButtonEnable();
    }

    void ApplyButtonEnable()
    {
        ToggleExpButton(_isShowExpButton);
        ToggleDefaultButton(!_isShowExpButton);
    }

    void ToggleExpButton(bool isActive)
    {
        GetTextMeshPro((int)Texts.GambleLevelText).gameObject.SetActive(isActive);
        GetTextMeshPro((int)Texts.ExpStatusText).gameObject.SetActive(isActive);
        GetButton((int)Buttons.AddExpButton).gameObject.SetActive(isActive);
    }
    
    void ToggleDefaultButton(bool isActive) => GetObject((int)GameObjects.SummonUnitButton).gameObject.SetActive(isActive);

    void AddExp()
    {
        if (Multi_GameManager.Instance.TryUseGold(4))
        {
            _gamblerLevelSystem.AddExperience(_addExpAmount);
            UpdateText();
        }
    }

    void UpdateText()
    {
        GetTextMeshPro((int)Texts.GambleLevelText).text = $"LV : {_gamblerLevelSystem.Level}";
        GetTextMeshPro((int)Texts.ExpStatusText).text = $"EXP : {_gamblerLevelSystem.Experience} / {_gamblerLevelSystem.NeedExperienceForLevelUp}";
    }
}

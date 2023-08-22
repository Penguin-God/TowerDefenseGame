using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleButtonsWhitGambler : UI_BattleButtons
{
    [SerializeField] Button _unitSummonSwichButton;
    [SerializeField] Button _addExpButton;
    [SerializeField] Button _worldButton;
    [SerializeField] TextMeshProUGUI _gambleLevelText;
    [SerializeField] TextMeshProUGUI _expStatusText;

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

        Managers.Camera.OnIsLookMyWolrd += OnWorldChange;
        _unitSummonSwichButton.onClick.AddListener(SwitchButtons);
        _addExpButton.onClick.AddListener(AddExp);
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
        _gambleLevelText.gameObject.SetActive(isActive);
        _expStatusText.gameObject.SetActive(isActive);
        _addExpButton.gameObject.SetActive(isActive);
    }
    
    void ToggleDefaultButton(bool isActive)
    {
        _worldButton.gameObject.SetActive(isActive);
        GetButton((int)Buttons.SummonUnitButton).gameObject.SetActive(isActive);
        GetButton((int)Buttons.StoryWolrd_EnterButton).gameObject.SetActive(isActive);
    }

    void AddExp()
    {
        _gamblerLevelSystem.AddExperience(_addExpAmount);
        UpdateText();
    }

    void UpdateText()
    {
        _gambleLevelText.text = $"LV : {_gamblerLevelSystem.Level}";
        _expStatusText.text = $"EXP : {_gamblerLevelSystem.Experience} / {_gamblerLevelSystem.NeedExperienceForLevelUp}";
    }

    protected override void GachaUnit()
    {
        var _game = Multi_GameManager.Instance;
        double[][] rates = new double[][] { new double[] { 100 }, new double[] { 30, 40, 30 } };
        if (_game.UnitOver == false && _game.TryUseGold(5))
            UnitGacha(rates[_gamblerLevelSystem.Level - 1]);
    }

    void UnitGacha(double[] rates)
    {
        UnitFlags selectUnitFlag = new UnitFlags(UnitFlags.NormalColors.ToList().GetRandom(), (UnitClass)new GachaMachine().SelectIndex(rates));
        Multi_SpawnManagers.NormalUnit.Spawn(selectUnitFlag);
    }
}

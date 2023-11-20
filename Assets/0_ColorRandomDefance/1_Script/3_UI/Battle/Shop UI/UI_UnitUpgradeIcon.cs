using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitUpgradeIcon : UI_Base
{
    UI_UnitIcon _unitIcon;
    TextMeshProUGUI _levelText;
    Button _upgradeButton;
    int _upgradeLevel = 1;
    int MaxLevel;
    protected override void Init()
    {
        _unitIcon = GetComponent<UI_UnitIcon>();
        _levelText = GetComponentInChildren<TextMeshProUGUI>();
        _upgradeButton = GetComponentInChildren<Button>();
    }

    public void FillGoods(UnitColor color, UnitUpgradeData upgradeData, MultiUnitStatController statController, int maxLevel)
    {
        CheckInit();
        _unitIcon.SetBGColor(color);
        _upgradeButton.onClick.AddListener(UpgradeUnit);
        MaxLevel = maxLevel;
        UpdateLevelText();

        void UpgradeUnit()
        {
            if (IsMaxUpgrade()) return;

            if (Multi_GameManager.Instance.TryUseGold(5))
            {
                const float Percentage = 100f;
                switch (upgradeData.UpgradeType)
                {
                    case UnitUpgradeType.Value: statController.AddUnitDamage(upgradeData.TargetColor, upgradeData.Value, UnitStatType.All); break;
                    case UnitUpgradeType.Scale: statController.ScaleUnitDamage(upgradeData.TargetColor, upgradeData.Value / Percentage, UnitStatType.All); break;
                }
                _upgradeLevel++;
                UpdateLevelText();
            }
        }
    }

    void UpdateLevelText()
    {
        if(IsMaxUpgrade())
            _levelText.text = "LV : MAX";
        else
            _levelText.text = $"LV : {_upgradeLevel}";
    }
    bool IsMaxUpgrade() => _upgradeLevel >= MaxLevel;
}

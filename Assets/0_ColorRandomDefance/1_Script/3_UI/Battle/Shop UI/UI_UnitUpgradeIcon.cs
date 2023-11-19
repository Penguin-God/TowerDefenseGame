using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_UnitUpgradeIcon : UI_Base
{
    UI_UnitIcon _unitIcon;
    TextMeshProUGUI _levelText;
    int _upgradeLevel = 1;
    int MaxLevel;
    protected override void Init()
    {
        _unitIcon = GetComponent<UI_UnitIcon>();
        _levelText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void FillGoods(UnitColor color, UnitUpgradeData upgradeData, MultiUnitStatController statController, int maxLevel)
    {
        CheckInit();
        _unitIcon.SetBGColor(color);
        _unitIcon.BindClickEvent(UpgradeUnit);
        MaxLevel = maxLevel;
        UpdateLevelText();

        void UpgradeUnit()
        {
            if (Multi_GameManager.Instance.TryUseGold(5))
            {
                const float Percentage = 100f;
                switch (upgradeData.UpgradeType)
                {
                    case UnitUpgradeType.Value: statController.AddUnitDamage(upgradeData.TargetColor, upgradeData.Value, UnitStatType.All); break;
                    case UnitUpgradeType.Scale: statController.ScaleUnitDamage(upgradeData.TargetColor, upgradeData.Value / Percentage, UnitStatType.All); break;
                }
                UpdateLevelText();
            }
        }
    }

    void UpdateLevelText()
    {
        if( _upgradeLevel >= MaxLevel )
            _levelText.text = "Level : MAX";
        else
            _levelText.text = $"Level : {_upgradeLevel}";
    }
}

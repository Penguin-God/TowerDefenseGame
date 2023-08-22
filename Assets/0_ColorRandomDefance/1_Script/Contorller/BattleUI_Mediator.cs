using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleUI_Type
{
    UnitUpgrdeShop,
    BalckUnitCombineTable,
    WhiteUnitShop,
    UnitMaxCountExpendShop,
}

public class BattleUI_Mediator
{
    readonly UI_Manager _uiManager;
    readonly Dictionary<BattleUI_Type, string> _pathBybattleUIType = new Dictionary<BattleUI_Type, string>();
    public BattleUI_Mediator(UI_Manager uiManager) => _uiManager = uiManager;

    public void RegisterUI(BattleUI_Type type, string path) => _pathBybattleUIType[type] = path;
    public T ShowPopupUI<T>(BattleUI_Type type) where T : UI_Popup => _uiManager.ShowPopupUI<T>(_pathBybattleUIType[type]);
    public void ShowPopupUI(BattleUI_Type type) => _uiManager.ShowPopupUI<UI_Popup>(_pathBybattleUIType[type]);


}

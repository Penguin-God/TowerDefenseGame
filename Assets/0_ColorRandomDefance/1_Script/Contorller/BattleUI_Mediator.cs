using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleUI_Type
{
    UnitUpgrdeShop,
    BalckUnitCombineTable,
    WhiteUnitShop,
    UnitMaxCountExpendShop,
    BattleButtons,
}

public class BattleUI_Mediator
{
    readonly UI_Manager _uiManager;
    readonly Dictionary<BattleUI_Type, string> _pathBybattleUIType = new Dictionary<BattleUI_Type, string>();
    readonly BattleDIContainer _container;
    public BattleUI_Mediator(UI_Manager uiManager) => _uiManager = uiManager;

    public void RegisterUI<T>(BattleUI_Type type) => RegisterUI(type, nameof(T));
    public void RegisterUI(BattleUI_Type type, string path) => _pathBybattleUIType[type] = path;
    public T ShowPopupUI<T>(BattleUI_Type type) where T : UI_Popup => _uiManager.ShowPopupUI<T>(_pathBybattleUIType[type]);
    public void ShowPopupUI(BattleUI_Type type) => _uiManager.ShowPopupUI<UI_Popup>(_pathBybattleUIType[type]);

    public T ShowSceneUI<T>(BattleUI_Type type) where T : UI_Scene
    {
        if(_uiManager.GetSceneUI<T>() != null)
        {
            T ui = _uiManager.ShowSceneUI<T>(_pathBybattleUIType[type]);
            Inject_UI(ui, type);
            return ui;
        }
        return _uiManager.ShowSceneUI<T>(_pathBybattleUIType[type]);
    }

    void Inject_UI(UI_Base ui, BattleUI_Type uiType)
    {
        switch (uiType)
        {
            case BattleUI_Type.UnitUpgrdeShop:
                break;
            case BattleUI_Type.BalckUnitCombineTable:
                break;
            case BattleUI_Type.WhiteUnitShop:
                break;
            case BattleUI_Type.UnitMaxCountExpendShop:
                break;
            case BattleUI_Type.BattleButtons: 
                (ui as UI_BattleButtons).Inject(_container.GetComponent<SwordmanGachaController>(), _container.GetComponent<TextShowAndHideController>()); break;
        }
    }
}

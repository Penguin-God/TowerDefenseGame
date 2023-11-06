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
    Paint,
}

public class BattleUI_Mediator
{
    readonly UI_Manager _uiManager;
    readonly Dictionary<BattleUI_Type, string> _pathBybattleUIType = new Dictionary<BattleUI_Type, string>();
    readonly BattleDIContainer _container;
    public BattleUI_Mediator(UI_Manager uiManager, BattleDIContainer container)
    {
        _uiManager = uiManager;
        _container = container;
    }

    public void RegisterUI<T>(BattleUI_Type type) => RegisterUI(type, typeof(T).Name);
    public void RegisterUI(BattleUI_Type type, string path) => _pathBybattleUIType[type] = path;
    public T ShowPopupUI<T>(BattleUI_Type type) where T : UI_Popup => _uiManager.ShowPopupUI<T>(_pathBybattleUIType[type]);
    public GameObject ShowPopupUI(BattleUI_Type type) => _uiManager.ShowPopupUI<UI_Popup>(_pathBybattleUIType[type]).gameObject;

    // UI 풀링부터 고치고 작업하기
    //public T _ShowPopupUI<T>(BattleUI_Type type) where T : UI_Popup
    //{
    //    bool uiExist = _uiManager.FindPopupUI<T>() != null;
    //    T result = _uiManager.ShowPopupUI<T>(_pathBybattleUIType[type]);
    //    if (uiExist == false)
    //    {
    //        if (result.GetComponent<UI_UnitUpgradeShop>() != null)
    //            result.GetComponent<UI_BattleButtons>().Inject(_container.GetComponent<SwordmanGachaController>(), _container.GetComponent<TextShowAndHideController>());
    //    }

    //    return result;
    //}

    public GameObject ShowUI(BattleUI_Type type)
    {
        switch (type)
        {
            case BattleUI_Type.BattleButtons: return ShowSceneUI<UI_BattleButtons>(type).gameObject;
        }
        return null;
    }

    public T ShowSceneUI<T>(BattleUI_Type type) where T : UI_Scene
    {
        bool uiExist = _uiManager.GetSceneUI<T>() != null;
        T result = _uiManager.ShowSceneUI<T>(_pathBybattleUIType[type]);
        if (uiExist == false)
        {
            if (result.TryGetComponent<UI_BattleButtons>(out var buttons))
                _container.Inject(buttons);
            else if(result.TryGetComponent<UI_Paint>(out var paint))
                _container.Inject(paint);
        }

        return result;
    }
}

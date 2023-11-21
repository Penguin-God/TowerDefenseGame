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
    UnitContolWindow,
}

public class BattleUI_Mediator
{
    readonly Dictionary<BattleUI_Type, string> _pathBybattleUIType = new Dictionary<BattleUI_Type, string>();
    readonly BattleDIContainer _container;
    public BattleUI_Mediator(BattleDIContainer container) => _container = container;

    public void RegisterUI<T>(BattleUI_Type type) => RegisterUI(type, typeof(T).Name);
    public void RegisterUI(BattleUI_Type type, string path) => _pathBybattleUIType[type] = path;
    void RegisterAndShow<T>(BattleUI_Type type) where T : UI_Popup
    {
        RegisterUI<T>(type);
        T ui = ShowPopupUI<T>(type);
        ui.gameObject.SetActive(false);
    }
    public GameObject ShowPopupUI(BattleUI_Type type) => Managers.UI.ShowPopupUI<UI_Popup>(_pathBybattleUIType[type]).gameObject;

    public T ShowPopupUI<T>(BattleUI_Type type) where T : UI_Popup
    {
        T result = Managers.UI.ShowPopupUI<T>(_pathBybattleUIType[type]);
        _container.Inject(result);
        return result;
    }

    public void RegisterDefaultUI()
    {
        RegisterUI(BattleUI_Type.WhiteUnitShop, "InGameShop/WhiteUnitShop");
        RegisterUI(BattleUI_Type.BalckUnitCombineTable, "InGameShop/BlackUnitShop");
        RegisterUI(BattleUI_Type.UnitMaxCountExpendShop, "InGameShop/UnitCountExpendShop_UI");
        RegisterUI(BattleUI_Type.UnitUpgrdeShop, "InGameShop/UI_UnitUpgradeShop"); // 여기 새걸로 바꾸면 됨 UI_UnitUpgradeShop
        RegisterAndShow<UI_UnitContolWindow>(BattleUI_Type.UnitContolWindow);

        RegisterUI(BattleUI_Type.Paint, "Paint");
        RegisterUI<UI_BattleButtons>(BattleUI_Type.BattleButtons);
    }


    public T ShowSceneUI<T>(BattleUI_Type type) where T : UI_Scene
    {
        T result = Managers.UI.ShowSceneUI<T>(_pathBybattleUIType[type]);
        _container.Inject(result);
        return result;
    }
}

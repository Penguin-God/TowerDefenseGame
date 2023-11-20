using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingClickContoller : MonoBehaviour
{
    BattleUI_Mediator _uiMediator;
    public void DependencyInject(BattleUI_Mediator uiMediator) => _uiMediator = uiMediator;

    void Update()
    {
        if (new InputHandler().MouseClickRayCastHit(out var hit))
        {
            var trigger = hit.collider.gameObject.GetComponentInParent<ShopTriggerBuilding>();
            if (trigger != null && trigger.OwnerId == PlayerIdManager.Id)
            {
                Managers.UI.ClosePopupUI();
                ShowUI(trigger.ShowUI_Type);
                // 사운드 필요한가?
            }
        }
    }

    void ShowUI(BattleUI_Type type)
    {
        switch (type)
        {
            case BattleUI_Type.UnitUpgrdeShop: _uiMediator.ShowPopupUI<UI_UnitUpgradeShop>(type); break; // _uiMediator.ShowPopupUI<UI_UnitUpgradeShop>(type); break; 
            case BattleUI_Type.BalckUnitCombineTable: _uiMediator.ShowPopupUI<BalckUnitShop_UI>(type); break;
            case BattleUI_Type.WhiteUnitShop:
            case BattleUI_Type.UnitMaxCountExpendShop: _uiMediator.ShowPopupUI(type); break;
        }
    }
}

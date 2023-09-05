using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingClickContoller : MonoBehaviour
{
    BattleUI_Mediator _uiMediator;
    UI_Manager _uiManager;
    TextShowAndHideController _textController;
    GoodsBuyController _buyController;
    public void Inject(BattleUI_Mediator uiMediator, UI_Manager uiManager, TextShowAndHideController textController, GoodsBuyController buyController)
    {
        _uiMediator = uiMediator;
        _uiManager = uiManager;
        _textController = textController;
        _buyController = buyController;
    }
    void Update()
    {
        if (new InputHandler().MouseClickRayCastHit(out var hit))
        {
            var trigger = hit.collider.gameObject.GetComponentInParent<ShopTriggerBuilding>();
            if (trigger != null && trigger.OwnerId == PlayerIdManager.Id)
            {
                _uiManager.ClosePopupUI();
                ShowUI(trigger.ShowUI_Type);
                // ���� �ʿ��Ѱ�?
            }
        }
    }

    void ShowUI(BattleUI_Type type)
    {
        switch (type)
        {
            case BattleUI_Type.UnitUpgrdeShop:
                var ui = _uiMediator.ShowPopupUI<UI_BattleShop>(type);
                if (ui.IsInject == false) 
                    ui.Inject(_buyController, _textController);
                break;
            case BattleUI_Type.BalckUnitCombineTable:
            case BattleUI_Type.WhiteUnitShop:
            case BattleUI_Type.UnitMaxCountExpendShop: _uiMediator.ShowPopupUI(type); break;
        }
    }
}

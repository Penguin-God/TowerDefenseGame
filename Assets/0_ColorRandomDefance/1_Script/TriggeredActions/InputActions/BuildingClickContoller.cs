using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingClickContoller : MonoBehaviour
{
    BattleUI_Mediator _uiMediator;
    UI_Manager _uiManager;
    public void Injection(BattleUI_Mediator uiMediator, UI_Manager uiManager)
    {
        _uiMediator = uiMediator;
        _uiManager = uiManager;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0�� ���� ���콺 ��ư�� ��Ÿ���ϴ�.
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, LayerMask.GetMask("ShopUITriggerBuilding")))
            {
                var trigger = hit.collider.gameObject.GetComponentInParent<ShopTriggerBuilding>();
                if (trigger != null && trigger.OwnerId == PlayerIdManager.Id)
                {
                    _uiManager.ClosePopupUI();
                    _uiMediator.ShowPopupUI(trigger.ShowUI_Type);
                    // ���� �ʿ��Ѱ�?
                }
            }
        }
    }
}

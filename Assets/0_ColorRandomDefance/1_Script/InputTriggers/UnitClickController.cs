using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitClickController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, LayerMask.GetMask("Unit")))
            {
                var unit = hit.collider.gameObject.GetComponentInParent<Multi_TeamSoldier>();
                if (unit != null && unit.UsingID == PlayerIdManager.Id && Managers.Data.UnitWindowDataByUnitFlags.ContainsKey(unit.UnitFlags))
                {
                    Managers.UI.ClosePopupUI();
                    Managers.UI.ShowPopupUI<UI_UnitManagedWindow>("UnitManagedWindow").Show(unit.UnitFlags);
                }
            }
        }
    }
}

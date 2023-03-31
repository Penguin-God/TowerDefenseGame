using Codice.Client.Commands;
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
                if (unit != null)
                    Managers.UI.ShowPopGroupUI<UI_UnitManagedWindow>(PopupGroupType.UnitWindow, "UnitManagedWindow").Show(unit.UnitFlags);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitClickController : MonoBehaviour
{
    int unitLayerMask;

    void Start()
    {
        unitLayerMask = 1 << LayerMask.NameToLayer("Unit");
    }

    void Update()
    {
        if (new InputHandler().MouseClickRayCastHit(out var hit, unitLayerMask))
        {
            var unit = hit.collider.gameObject.GetComponentInParent<Multi_TeamSoldier>();
            if (unit != null && unit.UsingID == PlayerIdManager.Id && Managers.Data.UnitWindowDataByUnitFlags.ContainsKey(unit.UnitFlags))
            {
                Managers.UI.ClosePopupUI();
                // GetTrakerSroter().SettingUnitTrackers(unit.UnitFlags);
                SrotByTargetColor(unit.UnitFlags);
                Managers.UI.ShowPopupUI<UI_UnitManagedWindow>("UnitManagedWindow").Show(unit.UnitFlags);
            }
        }
    }

    void SrotByTargetColor(UnitFlags flag) => Managers.UI.GetSceneUI<UI_BattleButtons>().GetComponentInChildren<UI_Paint>().SortByColor(flag.ColorNumber);
}

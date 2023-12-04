using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        //if (new InputHandler().MouseClickRayCastHit(out var hit, unitLayerMask))
        //{
        //    var unit = hit.collider.gameObject.GetComponentInParent<Multi_TeamSoldier>();
        //    if (unit != null && unit.UsingID == PlayerIdManager.Id && UnitFlags.NormalFlags.Contains(unit.UnitFlags))
        //    {
        //        Managers.UI.ClosePopupUI();
        //        Managers.UI.GetSceneUI<UI_Paint>().SortByClass(unit.UnitClass);
        //        StartCoroutine(Co_ShowUnitButtons(unit.UnitFlags)); // 정렬 시간 때문에 코루틴으로 한 프레임 대기
        //        //Managers.UI.ShowPopupUI<UI_UnitManagedWindow>("UnitManagedWindow").Show(unit.UnitFlags);
        //    }
        //}
    }

    //IEnumerator Co_ShowUnitButtons(UnitFlags flag)
    //{
    //    yield return null;
    //    Managers.UI.GetSceneUI<UI_Paint>().ClickTracker(flag);
    //}
}

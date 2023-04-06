using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitTooltipController
{
    readonly float OFFSET_X = 120f;
    readonly float DELAY_TIME = 0.5f;
    public void SetMouseOverAction(IEnumerable<UI_UnitTracker> uis)
    {
        uis.ToList().ForEach(x => {
            var mouseOverHandler = x.gameObject.AddComponent<MouseOverHandler>();
            mouseOverHandler.SetDelayTime(DELAY_TIME);
            mouseOverHandler.OnPointerEnterDelayedEvent += () => ShowUnitTooltip(x, OFFSET_X);
            mouseOverHandler.OnPointerExitEvent += CloseWindow;
        });
    }

    bool isShowWindow = false;
    void ShowUnitTooltip(UI_UnitTracker tracker, float offSetX)
    {
        BackGround window = Managers.UI.ShowPopupUI<BackGround>();
        float screenWidthScaleFactor = Screen.width / Managers.UI.UIScreenWidth;
        window.SetPosition(tracker.GetComponent<RectTransform>().position + new Vector3(offSetX * screenWidthScaleFactor, 0, 0));
        window.SetText(tracker.UnitFlags.ToString());
        isShowWindow = true;
    }

    void CloseWindow()
    {
        if (isShowWindow)
        {
            isShowWindow = false;
            Managers.UI.ClosePopupUI();
        }
    }
}

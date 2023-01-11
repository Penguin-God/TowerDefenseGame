using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextPopup
{
    public static void PopupText(string text)
    {
        var ui = Managers.UI.ShowPopupUI<UI_PopupText>();
        ui.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0, 120f, 0);
        ui.Show(text, 3f);
    }
}

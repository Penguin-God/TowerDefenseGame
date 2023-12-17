using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_NotifyWindow : UI_Popup
{
    TextMeshProUGUI _text;

    public void SetMessage(string message)
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _text.text = message;
    }
}

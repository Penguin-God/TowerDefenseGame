using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialText : UI_Popup
{
    public void Setup(string text)
    {
        GetComponent<Text>().text = text;
    }
}

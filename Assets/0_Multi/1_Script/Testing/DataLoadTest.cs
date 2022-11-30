using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataLoadTest : MonoBehaviour
{
    void Awake()
    {
        
    }

    [ContextMenu("Test")]
    void Test()
    {
        Managers.UI.ShowPopupUI<UI_PopupText>().Show("안녕 세상", 200, Color.red);
    }
}

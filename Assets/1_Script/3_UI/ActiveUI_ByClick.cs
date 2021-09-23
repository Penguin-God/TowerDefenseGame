using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveUI_ByClick : MonoBehaviour
{
    [SerializeField] GameObject activeUI = null;
    private void OnMouseDown()
    {
        activeUI.SetActive(true);
    }
}

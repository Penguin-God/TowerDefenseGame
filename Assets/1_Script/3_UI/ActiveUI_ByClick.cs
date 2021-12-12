using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveUI_ByClick : MonoBehaviour
{
    [SerializeField] GameObject activeUI = null;
    private void OnMouseDown()
    {
        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);
        UIManager.instance.WhiteTowerButton.gameObject.SetActive(false);
        activeUI.SetActive(true);
    }
}

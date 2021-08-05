using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddOnClickEvent : MonoBehaviour
{
    [SerializeField] Transform obj_UnitManageUI;
    GameObject currentShowUI = null;
    private void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Button button = transform.GetChild(i).GetComponent<Button>();
            button.onClick.AddListener(() => obj_UnitManageUI.transform.GetChild(i).gameObject.SetActive(true));
            button.onClick.AddListener(() => ShowUnitUI() );
            button.onClick.AddListener(() => currentShowUI = obj_UnitManageUI.GetChild(i).gameObject);
        }
    }

    void ShowUnitUI()
    {
        if (currentShowUI != null) currentShowUI.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddOnClickEvent : MonoBehaviour
{
    [SerializeField] GameObject[] obj_UnitImages;
    [SerializeField] GameObject[] obj_UnitManageUI;
    GameObject currentShowUI = null;
    private void Start()
    {
        for(int i = 0; i < obj_UnitManageUI.Length; i++)
        {
            int index = i;
            obj_UnitImages[index].GetComponent<Button>().onClick.AddListener(() => SetOnClick(index));
        }
    }

    void SetOnClick(int i)
    {
        Debug.Log(i);
        obj_UnitManageUI[i].SetActive(true);
        if (currentShowUI != null) currentShowUI.SetActive(false);
        currentShowUI = obj_UnitManageUI[i].gameObject;
    }
}

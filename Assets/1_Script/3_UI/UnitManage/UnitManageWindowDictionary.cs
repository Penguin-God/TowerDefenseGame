using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitWindowDictionary : SerializableDictionary<string, GameObject>
{

}

public class UnitManageWindowDictionary : MonoBehaviour
{
    public GameObject currentShowWindow = null;
    public UnitWindowDictionary dic_UnitWindow;

    public void ShowUnitManageWindow(string name)
    {
        if (currentShowWindow != null) currentShowWindow.SetActive(false);

        GameObject showObj = dic_UnitWindow[name];
        showObj.SetActive(true);
        currentShowWindow = showObj;
    }

    public void HideCurrentWindow()
    {
        if(currentShowWindow != null)
        {
            currentShowWindow.SetActive(false);
            currentShowWindow = null;
        }
    }
}

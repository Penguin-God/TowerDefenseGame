using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopObject : MonoBehaviour
{
    [SerializeField] protected string path;

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        ShowShop();
    }

    protected virtual void ShowShop()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopObject : MonoBehaviour
{
    [SerializeField] protected string path;

    void OnMouseDown()
    {
        ShowShop();
    }

    protected virtual void ShowShop()
    {

    }
}

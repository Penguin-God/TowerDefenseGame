using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShop : MonoBehaviour
{
    [SerializeField] GameObject shopWindow = null;
    private void OnMouseDown()
    {
        shopWindow.SetActive(true);
    }
}

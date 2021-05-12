using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEvent : MonoBehaviour
{
    
    private void OnMouseDown()
    {
        UIManager.instance.TowerButton.gameObject.SetActive(true);
    }
}

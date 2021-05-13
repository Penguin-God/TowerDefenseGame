using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackTowerEvent : MonoBehaviour
{
    public CreateDefenser createDefenser;
    
    private void OnMouseDown()
    {
        UIManager.instance.BlackTowerButton.gameObject.SetActive(true);
    }

    public void ClickBlackSwordmanButton()
    {
        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);
        createDefenser.CreateSoldier(6, 0);
    }

    public void ClickBlackArcherButton()
    {
        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);
        createDefenser.CreateSoldier(6, 1);
    }

    public void ClickBlackSpearmanButton()
    {
        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);
        createDefenser.CreateSoldier(6, 2);
    }

    public void ClickBlackMageButton()
    {
        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);
        createDefenser.CreateSoldier(6, 3);
    }
}

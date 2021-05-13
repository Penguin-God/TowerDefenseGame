using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteTowerEvent : MonoBehaviour
{
    public CreateDefenser createDefenser;

    private void OnMouseDown()
    {
        UIManager.instance.WhiteTowerButton.gameObject.SetActive(true);
    }

    public void ClickWhiteSwordmanButton()
    {
        if (GameManager.instance.Food >= 1)
        {
            createDefenser.CreateSoldier(7, 0);
            GameManager.instance.Food -= 1;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);
        }
        UIManager.instance.WhiteTowerButton.gameObject.SetActive(false);
    }

    public void ClickWhiteArcherButton()
    {
        if (GameManager.instance.Food >= 2)
        {
            createDefenser.CreateSoldier(7, 1);
            GameManager.instance.Food -= 2;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);
        }
        UIManager.instance.WhiteTowerButton.gameObject.SetActive(false);
    }

    public void ClickWhiteSpearmanButton()
    {
        if (GameManager.instance.Food >= 5)
        {
            createDefenser.CreateSoldier(7, 2);
            GameManager.instance.Food -= 5;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);
        }
        UIManager.instance.WhiteTowerButton.gameObject.SetActive(false);
    }

    public void ClickWhiteMageButton()
    {
        if (GameManager.instance.Food >= 10)
        {
            createDefenser.CreateSoldier(7, 3);
            GameManager.instance.Food -= 10;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);
        }
        UIManager.instance.WhiteTowerButton.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteTowerEvent : MonoBehaviour
{
    public CreateDefenser createDefenser;
    public BlackTowerEvent blackTowerEvent;

    private void OnMouseDown()
    {
        UIManager.instance.WhiteTowerButton.gameObject.SetActive(true);
        blackTowerEvent.Show_BuyBackGround(); // 버그 때문에 박준이 추가한 코드
        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);
        blackTowerEvent.BlackUiAudio.Play();
    }

    public void ClickWhiteSwordmanButton()
    {
        if (GameManager.instance.Food >= 1)
        {
            createDefenser.CreateSoldier(7, 0);
            GameManager.instance.Food -= 1;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);
        }

        blackTowerEvent.BlackUiAudio.Play();
        UIManager.instance.WhiteTowerButton.gameObject.SetActive(false);
        blackTowerEvent.Hide_BuyBackGround();
    }

    public void ClickWhiteArcherButton()
    {
        if (GameManager.instance.Food >= 2)
        {
            createDefenser.CreateSoldier(7, 1);
            GameManager.instance.Food -= 2;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);
        }

        blackTowerEvent.BlackUiAudio.Play();
        UIManager.instance.WhiteTowerButton.gameObject.SetActive(false);
        blackTowerEvent.Hide_BuyBackGround();
    }

    public void ClickWhiteSpearmanButton()
    {
        if (GameManager.instance.Food >= 7)
        {
            createDefenser.CreateSoldier(7, 2);
            GameManager.instance.Food -= 5;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);
        }

        blackTowerEvent.BlackUiAudio.Play();
        UIManager.instance.WhiteTowerButton.gameObject.SetActive(false);
        blackTowerEvent.Hide_BuyBackGround();
    }

    public void ClickWhiteMageButton()
    {
        if (GameManager.instance.Food >= 20)
        {
            createDefenser.CreateSoldier(7, 3);
            GameManager.instance.Food -= 10;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);
        }

        blackTowerEvent.BlackUiAudio.Play();
        UIManager.instance.WhiteTowerButton.gameObject.SetActive(false);
        blackTowerEvent.Hide_BuyBackGround();
    }
}

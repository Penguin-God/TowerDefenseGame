using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteTowerEvent : MonoBehaviour
{
    public CreateDefenser createDefenser;
    public BlackTowerEvent blackTowerEvent;
    public SoldiersTags TagSoldier;

    private void OnMouseDown()
    {
        UIManager.instance.WhiteTowerButton.gameObject.SetActive(true);
        blackTowerEvent.Show_BuyBackGround(); // 버그 때문에 박준이 추가한 코드
        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);
        blackTowerEvent.BlackUiAudio.Play();
    }

    public void ClickWhiteSwordmanButton()
    {
        if (UnitManager.instance.UnitOver)
        {

            return;
        }
        if (GameManager.instance.Food >= 1)
        {
            CombineSoldierPooling.GetObject("WhiteSwordman", 7, 0);
            //createDefenser.CreateSoldier(7, 0);
            GameManager.instance.Food -= 1;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);
        }

        blackTowerEvent.BlackUiAudio.Play();
        UIManager.instance.WhiteTowerButton.gameObject.SetActive(false);
        blackTowerEvent.Hide_BuyBackGround();
    }

    public void ClickWhiteArcherButton()
    {
        if(UnitManager.instance.UnitOver)
        {

            return;
        }
        if (GameManager.instance.Food >= 2)
        {
            CombineSoldierPooling.GetObject("WhiteArcher", 7, 1);
            //createDefenser.CreateSoldier(7, 1);
            GameManager.instance.Food -= 2;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);
        }

        blackTowerEvent.BlackUiAudio.Play();
        UIManager.instance.WhiteTowerButton.gameObject.SetActive(false);
        blackTowerEvent.Hide_BuyBackGround();
    }

    public void ClickWhiteSpearmanButton()
    {
        if(UnitManager.instance.UnitOver)
        {

            return;
        }
        if (GameManager.instance.Food >= 7)
        {
            CombineSoldierPooling.GetObject("WhiteSpearman", 7, 2);
            //createDefenser.CreateSoldier(7, 2);
            GameManager.instance.Food -= 7;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);
        }

        blackTowerEvent.BlackUiAudio.Play();
        UIManager.instance.WhiteTowerButton.gameObject.SetActive(false);
        blackTowerEvent.Hide_BuyBackGround();
    }

    public void ClickWhiteMageButton()
    {
        if(UnitManager.instance.UnitOver)
        {

            return;
        }
        if (GameManager.instance.Food >= 20)
        {
            CombineSoldierPooling.GetObject("WhiteMage", 7, 3);
            //createDefenser.CreateSoldier(7, 3);
            GameManager.instance.Food -= 20;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);
        }

        blackTowerEvent.BlackUiAudio.Play();
        UIManager.instance.WhiteTowerButton.gameObject.SetActive(false);
        blackTowerEvent.Hide_BuyBackGround();
    }
}

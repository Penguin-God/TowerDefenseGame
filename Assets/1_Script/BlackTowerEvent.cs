using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackTowerEvent : MonoBehaviour
{
    public CreateDefenser createDefenser;
    private int Randomnumber;
    

    private void OnMouseDown()
    {
        UIManager.instance.BlackTowerButton.gameObject.SetActive(true);
        UIManager.instance.WhiteTowerButton.gameObject.SetActive(false);
    }

    private void FailTextDown()
    {
        UIManager.instance.FailText.gameObject.SetActive(false);
    }

    private void SuccessTextDown()
    {
        UIManager.instance.SuccessText.gameObject.SetActive(false);
    }

    public void ClickBlackSwordmanButton()
    {
        if (GameManager.instance.Gold >= 10)
        {
            Randomnumber = Random.Range(0, 2); // 50%
            if (Randomnumber == 0)
            {
                createDefenser.CreateSoldier(6, 0);
                UIManager.instance.SuccessText.gameObject.SetActive(true);
                Invoke("SuccessTextDown", 1f);
            }
            else
            {
                UIManager.instance.FailText.gameObject.SetActive(true);
                Invoke("FailTextDown", 1f);
            }
            GameManager.instance.Gold -= 10;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

        } 

        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);
        
    }

    public void ClickBlackArcherButton()
    {
        if (GameManager.instance.Gold >= 15)
        {
            Randomnumber = Random.Range(0, 4); // 25%
            if (Randomnumber == 0)
            {
                createDefenser.CreateSoldier(6, 1);
                UIManager.instance.SuccessText.gameObject.SetActive(true);
                Invoke("SuccessTextDown", 1f);
            }
            else
            {
                UIManager.instance.FailText.gameObject.SetActive(true);
                Invoke("FailTextDown", 1f);
            }
            GameManager.instance.Gold -= 15;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

        }

        
        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);
    }

    public void ClickBlackSpearmanButton()
    {
        if(GameManager.instance.Gold >= 20)
        {
            Randomnumber = Random.Range(0, 10); // 10%
            if (Randomnumber == 0)
            {
                createDefenser.CreateSoldier(6, 2);
                UIManager.instance.SuccessText.gameObject.SetActive(true);
                Invoke("SuccessTextDown", 1f);
            }
            else
            {
                UIManager.instance.FailText.gameObject.SetActive(true);
                Invoke("FailTextDown", 1f);
            }
            GameManager.instance.Gold -= 20;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

        }


        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);
    }

    public void ClickBlackMageButton()
    {
        if(GameManager.instance.Gold >= 25)
        {
            Randomnumber = Random.Range(0, 25); // 4%
            if (Randomnumber == 0)
            {
                createDefenser.CreateSoldier(6, 3);
                UIManager.instance.SuccessText.gameObject.SetActive(true);
                Invoke("SuccessTextDown", 1f);
            }
            else
            {
                UIManager.instance.FailText.gameObject.SetActive(true);
                Invoke("FailTextDown", 1f);
            }
            GameManager.instance.Gold -= 25;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
        }

        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);

    }
}

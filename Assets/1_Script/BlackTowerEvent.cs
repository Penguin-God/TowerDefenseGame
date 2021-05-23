﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackTowerEvent : MonoBehaviour
{
    public CreateDefenser createDefenser;
    private int Randomnumber;
    public AudioSource BlackUiAudio;
    public GameObject BlackCombineButtons;
    public SoldiersTags soldiersTags;

    

    private void OnMouseDown()
    {
        UIManager.instance.BlackTowerButton.gameObject.SetActive(true);
        UIManager.instance.BackGround.gameObject.SetActive(true);
        UIManager.instance.WhiteTowerButton.gameObject.SetActive(false);
        BlackUiAudio.Play();
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
        if (GameManager.instance.Gold >= 30)
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
            GameManager.instance.Gold -= 30;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
            

        }

        BlackUiAudio.Play();
        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);
        UIManager.instance.BackGround.gameObject.SetActive(false);

    }

    public void ClickBlackArcherButton()
    {
        if (GameManager.instance.Gold >= 40)
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

            BlackUiAudio.Play();
            GameManager.instance.Gold -= 40;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

        }

        
        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);
        UIManager.instance.BackGround.gameObject.SetActive(false);
    }

    public void ClickBlackSpearmanButton()
    {
        if(GameManager.instance.Gold >= 50)
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

            BlackUiAudio.Play();
            GameManager.instance.Gold -= 50;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

        }


        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);
        UIManager.instance.BackGround.gameObject.SetActive(false);
    }

    public void ClickBlackMageButton()
    {
        if(GameManager.instance.Gold >= 60)
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
            GameManager.instance.Gold -= 60;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
        }

        BlackUiAudio.Play();
        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);
        UIManager.instance.BackGround.gameObject.SetActive(false);

    }

    public void ClickCombineBlackSoldiersButton()
    {
        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);
        BlackCombineButtons.gameObject.SetActive(true);

        BlackUiAudio.Play();

    }

    public void ClickCombineBlackArcherButton()
    {
        soldiersTags.BlackSwordmanTag();
        if (soldiersTags.BlackSwordman.Length >= 3)
        {
            
            Destroy(soldiersTags.BlackSwordman[0]);
            Destroy(soldiersTags.BlackSwordman[1]);
            Destroy(soldiersTags.BlackSwordman[2]);

            createDefenser.CreateSoldier(6, 1);
        }
        BlackUiAudio.Play();

        BlackCombineButtons.gameObject.SetActive(false);
        UIManager.instance.BackGround.gameObject.SetActive(false);

    }

    public void ClickCombineBlackSpearmanButton()
    {
        soldiersTags.BlackArcherTag();
        if (soldiersTags.BlackArcher.Length >= 3)
        {
            
            Destroy(soldiersTags.BlackArcher[0]);
            Destroy(soldiersTags.BlackArcher[1]);
            Destroy(soldiersTags.BlackArcher[2]);

            createDefenser.CreateSoldier(6, 2);
        }
        BlackUiAudio.Play();

        BlackCombineButtons.gameObject.SetActive(false);
        UIManager.instance.BackGround.gameObject.SetActive(false);
    }

    public void ClickCombineBlackMageButton()
    {
        soldiersTags.BlackSpearmanTag();
        if (soldiersTags.BlackSpearman.Length >= 3)
        {
            
            Destroy(soldiersTags.BlackSpearman[0]);
            Destroy(soldiersTags.BlackSpearman[1]);
            Destroy(soldiersTags.BlackSpearman[2]);

            createDefenser.CreateSoldier(6, 3);
        }
        BlackUiAudio.Play();

        BlackCombineButtons.gameObject.SetActive(false);
        UIManager.instance.BackGround.gameObject.SetActive(false);
    }
}

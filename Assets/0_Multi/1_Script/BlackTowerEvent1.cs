using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BlackTowerEvent1 : MonoBehaviourPun
{
    public Multi_CreateDefenser createDefenser1;
    private int Randomnumber;
    public AudioSource BlackUiAudio;
    public GameObject BlackCombineButtons;
    public SoldiersTags soldiersTags;
    public CombineSoldier combineSoldier;


    public GameObject buyBackGround;

    private void OnMouseDown()
    {
        UIManager.instance.BlackTowerButton.gameObject.SetActive(true);
        //UIManager.instance.BackGround.gameObject.SetActive(true);
        Show_BuyBackGround(); // 버그 때문에 박준이 추가한 코드
        UIManager.instance.WhiteTowerButton.gameObject.SetActive(false);
        BlackUiAudio.Play();
    }                                          
    private void CombineSuccessTextDown()
    {
        UIManager.instance.CombineSuccessText.gameObject.SetActive(false);
    }

    private void CombineFailTextDown()
    {
        UIManager.instance.CombineFailText.gameObject.SetActive(false);
    }

    public void Show_BuyBackGround()
    {
        buyBackGround.SetActive(true);
    }

    public void Hide_BuyBackGround()
    {
        buyBackGround.SetActive(false);
    }

    private void SuccessTextDown()
    {
        UIManager.instance.SuccessText.gameObject.SetActive(false);
    }

    private void FailTextDown()
    {
        UIManager.instance.FailText.gameObject.SetActive(false);
    }

    public void ClickBlackSwordmanButton()
    {
        if (GameManager.instance.Gold >= 5)
        {
            Randomnumber = Random.Range(0, 2); // 50%
            if (Randomnumber == 0)
            {
                createDefenser1.CreateSoldier(6, 0);
                UIManager.instance.SuccessText.gameObject.SetActive(true);
                Invoke("SuccessTextDown", 1f);
            }
            else
            {
                UIManager.instance.FailText.gameObject.SetActive(true);
                Invoke("FailTextDown", 1f);
            }
            GameManager.instance.Gold -= 5;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
            

        }

        BlackUiAudio.Play();
        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);
        Hide_BuyBackGround();

    }

    public void ClickBlackArcherButton()
    {
        if (GameManager.instance.Gold >= 10)
        {
            Randomnumber = Random.Range(0, 3); // 25%
            if (Randomnumber == 0)
            {
                createDefenser1.CreateSoldier(6, 1);
                UIManager.instance.SuccessText.gameObject.SetActive(true);
                Invoke("SuccessTextDown", 1f);
            }
            else
            {
                UIManager.instance.FailText.gameObject.SetActive(true);
                Invoke("FailTextDown", 1f);
            }

            BlackUiAudio.Play();
            GameManager.instance.Gold -= 10;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

        }

        
        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);
        Hide_BuyBackGround();
    }

    public void ClickBlackSpearmanButton()
    {
        if(GameManager.instance.Gold >= 15)
        {
            Randomnumber = Random.Range(0, 9); // 10%
            if (Randomnumber == 0)
            {
                createDefenser1.CreateSoldier(6, 2);
                UIManager.instance.SuccessText.gameObject.SetActive(true);
                Invoke("SuccessTextDown", 1f);
            }
            else
            {
                UIManager.instance.FailText.gameObject.SetActive(true);
                Invoke("FailTextDown", 1f);
            }

            BlackUiAudio.Play();
            GameManager.instance.Gold -= 15;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

        }


        UIManager.instance.BlackTowerButton.gameObject.SetActive(false);
        Hide_BuyBackGround();
    }

    public void ClickBlackMageButton()
    {
        if(GameManager.instance.Gold >= 30)
        {
            Randomnumber = Random.Range(0, 25); // 4%
            if (Randomnumber == 0)
            {
                createDefenser1.CreateSoldier(6, 3);
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
        Hide_BuyBackGround();

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
            
            Destroy(soldiersTags.BlackSwordman[0].transform.parent.gameObject);
            Destroy(soldiersTags.BlackSwordman[1].transform.parent.gameObject);
            Destroy(soldiersTags.BlackSwordman[2].transform.parent.gameObject);

            createDefenser1.CreateSoldier(6, 1);
            UIManager.instance.UpdateCombineSuccessText("검은 아처 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }


        BlackUiAudio.Play();

        BlackCombineButtons.gameObject.SetActive(false);
        Hide_BuyBackGround();

    }

    public void ClickCombineBlackSpearmanButton()
    {
        soldiersTags.BlackArcherTag();
        if (soldiersTags.BlackArcher.Length >= 3)
        {
            
            Destroy(soldiersTags.BlackArcher[0].transform.parent.gameObject);
            Destroy(soldiersTags.BlackArcher[1].transform.parent.gameObject);
            Destroy(soldiersTags.BlackArcher[2].transform.parent.gameObject);

            createDefenser1.CreateSoldier(6, 2);

            UIManager.instance.UpdateCombineSuccessText("검은 창병 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }
        BlackUiAudio.Play();

        BlackCombineButtons.gameObject.SetActive(false);
        Hide_BuyBackGround();
    }

    public void ClickCombineBlackMageButton()
    {
        soldiersTags.BlackSpearmanTag();
        if (soldiersTags.BlackSpearman.Length >= 3)
        {
            
            Destroy(soldiersTags.BlackSpearman[0].transform.parent.gameObject);
            Destroy(soldiersTags.BlackSpearman[1].transform.parent.gameObject);
            Destroy(soldiersTags.BlackSpearman[2].transform.parent.gameObject);

            createDefenser1.CreateSoldier(6, 3);

            UIManager.instance.UpdateCombineSuccessText("검은 마법사 조합");
            UIManager.instance.CombineSuccessText.gameObject.SetActive(true);
            Invoke("CombineSuccessTextDown", 1f);

        }
        else
        {
            UIManager.instance.CombineFailText.gameObject.SetActive(true);
            Invoke("CombineFailTextDown", 1f);
        }


        BlackUiAudio.Play();

        BlackCombineButtons.gameObject.SetActive(false);
        Hide_BuyBackGround();
    }
}

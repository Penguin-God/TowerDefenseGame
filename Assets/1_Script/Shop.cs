using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    private int Productnumber;

    private int SetRandomNumber()
    {
        Productnumber = Random.Range(0, 2);
        return Productnumber;
    } 

    public Text LeftText;
    public Text CenterText;
    public Text RightText;

    public GameObject EventShop;



    public CreateDefenser createDefenser;
    public void OnEventShop()
    {
        EventShop.SetActive(true);

        SetRandomNumber();

        // 랜덤하게 상품 변경 
        UpdateLeftText();
        UpdateCenterText();
        UpdateRightText();

    }

    public void UpdateLeftText() // 이미지로 바꾸면 좋을 듯
    {
        if(Productnumber == 0)
        {
            LeftText.text = "하얀 기사";
        }

        if(Productnumber == 1)
        {
            LeftText.text = "하얀 아처";
        }

        if (Productnumber == 2)
        {
            LeftText.text = "하얀 창병";
        }
    }

    public void UpdateCenterText() // 이미지로 바꾸면 좋을 듯
    {
        if (Productnumber == 0)
        {
            CenterText.text = "고기 1개";
        }

        if (Productnumber == 1)
        {
            CenterText.text = "고기 2개";
        }

        if (Productnumber == 2)
        {
            CenterText.text = "고기 3개";
        }
    }

    public void UpdateRightText() // 이미지로 바꾸면 좋을 듯
    {
        if (Productnumber == 0)
        {

            RightText.text = "데미지 증가";
        }
        if (Productnumber == 1)
        { 
           RightText.text = "보스 데미지 증가";
        }

        if (Productnumber == 2)
        {
            RightText.text = "다른 효과";
        }
        
    }



    public void ClickLeftGoods()
    {
        if (Productnumber == 0 && GameManager.instance.Gold >= 5)
        {
            createDefenser.CreateSoldier(7, 0);

            GameManager.instance.Gold -= 5;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            EventShop.SetActive(false);
        }

        if (Productnumber == 1 && GameManager.instance.Gold >= 10)
        {
            createDefenser.CreateSoldier(7, 1);

            GameManager.instance.Gold -= 10;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            EventShop.SetActive(false);
        }

        if (Productnumber == 2 && GameManager.instance.Gold >= 15)
        {
            createDefenser.CreateSoldier(7, 2);

            GameManager.instance.Gold -= 15;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            EventShop.SetActive(false);
        }
    }

    public void ClickCenterGoods()
    {
        if (Productnumber == 0 && GameManager.instance.Gold >= 5)
        {
            GameManager.instance.Food += 1;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);

            GameManager.instance.Gold -= 5;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            EventShop.SetActive(false);
        }

        if (Productnumber == 1 && GameManager.instance.Gold >= 10)
        {
            GameManager.instance.Food += 2;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);

            GameManager.instance.Gold -= 10;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            EventShop.SetActive(false);
        }

        if (Productnumber == 2 && GameManager.instance.Gold >= 15)
        {
            GameManager.instance.Food += 3;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);

            GameManager.instance.Gold -= 15;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            EventShop.SetActive(false);
        }
    }

    public void ClickRightGoods()
    {
        if (Productnumber == 0)
        {
            EventShop.SetActive(false);
        }

        if (Productnumber == 1)
        {
            EventShop.SetActive(false);
        }

        if (Productnumber == 2)
        {
            EventShop.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    // Action을 담는 리스트를 다르게 만들어 상점 기능 구현 ex) 유닛 소환은 void로 int 인자를 받고, 이벤트는 다른 타입의 인자를 받음
    private int Productnumber;

    private int SetRandomNumber()
    {
        Productnumber = Random.Range(0, 2);
        return Productnumber;
    }

    public Text LeftText;
    public Text CenterText;
    public Text RightText;

    public GameObject envetShop;
    public GameObject leftGoldGoods;
    public GameObject centerGoldGoods;
    public GameObject rigthGoldGoods;
    public GameObject foodGoods;
    // 변수명 반대로 하기
    public GameObject[] leftGoldStocks;
    public GameObject[] centerGoldStocks;
    public GameObject[] rigthGoldStocks;
    public GameObject[] foodStocks;

    private GameObject current_LeftGoldGoods;
    private GameObject current_CenterGoldGoods;
    private GameObject current_RigthGoldGoods;
    private GameObject current_FoodGoldGoods;

    private void Awake() // 배열 선언
    {
        leftGoldStocks = new GameObject[leftGoldGoods.transform.childCount];
        for(int i = 0; i < leftGoldStocks.Length; i++)
        {
            leftGoldStocks[i] = leftGoldGoods.transform.GetChild(i).gameObject;
        }

        centerGoldStocks = new GameObject[centerGoldGoods.transform.childCount];
        for (int i = 0; i < centerGoldStocks.Length; i++)
        {
            centerGoldStocks[i] = centerGoldGoods.transform.GetChild(i).gameObject;
        }

        rigthGoldStocks = new GameObject[rigthGoldGoods.transform.childCount];
        for (int i = 0; i < rigthGoldStocks.Length; i++)
        {
            rigthGoldStocks[i] = rigthGoldGoods.transform.GetChild(i).gameObject;
        }

        foodStocks = new GameObject[foodGoods.transform.childCount];
        for (int i = 0; i < foodStocks.Length; i++)
        {
            foodStocks[i] = foodGoods.transform.GetChild(i).gameObject;
        }

        Set_RandomShop();
    }

    public CreateDefenser createDefenser;


    //private void OnMouseDown()
    //{
    //    OnEnvetShop();
    //}

    public void OnenvetShop()
    {
        //envetShop.SetActive(true);

        //SetRandomNumber();

        // 랜덤하게 상품 변경 
        //UpdateLeftText();
        //UpdateCenterText();
        //UpdateRightText();
    }

    void Set_RandomShop()
    {
        Show_RandomGoods(leftGoldStocks);
        Show_RandomGoods(centerGoldStocks);
        Show_RandomGoods(rigthGoldStocks);
        Show_RandomGoods(foodStocks);
    }

    void Show_RandomGoods(GameObject[] goods)
    {
        int goodsIndex = Random.Range(0, goods.Length);
        goods[goodsIndex].SetActive(true);
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

    public void BuyGoods(int price)
    {
        if (GameManager.instance.Gold < price) return;

        GameManager.instance.Gold -= price;
    }

    public void ClickLeftGoods()
    {
        if (Productnumber == 0 && GameManager.instance.Gold >= 5)
        {
            createDefenser.CreateSoldier(7, 0);

            GameManager.instance.Gold -= 5;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            envetShop.SetActive(false);
        }

        if (Productnumber == 1 && GameManager.instance.Gold >= 10)
        {
            createDefenser.CreateSoldier(7, 1);

            GameManager.instance.Gold -= 10;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            envetShop.SetActive(false);
        }

        if (Productnumber == 2 && GameManager.instance.Gold >= 15)
        {
            createDefenser.CreateSoldier(7, 2);

            GameManager.instance.Gold -= 15;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            envetShop.SetActive(false);
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

            envetShop.SetActive(false);
        }

        if (Productnumber == 1 && GameManager.instance.Gold >= 10)
        {
            GameManager.instance.Food += 2;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);

            GameManager.instance.Gold -= 10;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            envetShop.SetActive(false);
        }

        if (Productnumber == 2 && GameManager.instance.Gold >= 15)
        {
            GameManager.instance.Food += 3;
            UIManager.instance.UpdateFoodText(GameManager.instance.Food);

            GameManager.instance.Gold -= 15;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            envetShop.SetActive(false);
        }
    }

    public void ClickRightGoods()
    {
        if (Productnumber == 0)
        {
            envetShop.SetActive(false);
        }

        if (Productnumber == 1)
        {
            envetShop.SetActive(false);
        }

        if (Productnumber == 2)
        {
            envetShop.SetActive(false);
        }
    }

    public bool showShop;

    public void ExitShop()
    {
        envetShop.SetActive(false);
        showShop = false;
    }

    public void ShowShop()
    {
        envetShop.SetActive(true);
        showShop = true;
    }

    //public void EnterShopWlord()
    //{
    //    if (!enterShop)
    //    {
    //        Camera.main.gameObject.transform.position = new Vector3(-500, 100, -30);
    //        enterShop = true;
    //    }
    //    else
    //    {
    //        Camera.main.gameObject.transform.position = new Vector3(0, 100, -30);
    //        enterShop = false;
    //    }
    //}



}

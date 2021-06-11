using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour
{
    public GameObject envetShop;
    public GameObject leftGoldGoods;
    public GameObject centerGoldGoods;
    //public GameObject rigthGoldGoods;
    public GameObject foodGoods;
    // 변수명 반대로 하기
    public GameObject[] leftGoldStocks;
    public GameObject[] centerGoldStocks; 
    //public GameObject[] rigthGoldStocks; 
    public GameObject[] foodStocks;

    private GameObject current_LeftGoldGoods = null;
    private GameObject current_CenterGoldGoods = null;
    //private GameObject current_RigthGoldGoods = null;
    private GameObject current_FoodGoldGoods = null;
    public CreateDefenser createDefenser;

    private void Awake() // 배열 선언
    {
        leftGoldStocks = new GameObject[leftGoldGoods.transform.childCount];
        for (int i = 0; i < leftGoldStocks.Length; i++)
        {
            leftGoldStocks[i] = leftGoldGoods.transform.GetChild(i).gameObject;
        }

        centerGoldStocks = new GameObject[centerGoldGoods.transform.childCount];
        for (int i = 0; i < centerGoldStocks.Length; i++)
        {
            centerGoldStocks[i] = centerGoldGoods.transform.GetChild(i).gameObject;
        }

        foodStocks = new GameObject[foodGoods.transform.childCount];
        for (int i = 0; i < foodStocks.Length; i++)
        {
            foodStocks[i] = foodGoods.transform.GetChild(i).gameObject;
        }
    }
    void MinusGold(int price)
    {
        GameManager.instance.Gold -= price;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
    }

    GameObject SetButton(Button clickButton, GameObject buyGoodsObject)
    {
        buyGoodsObject.SetActive(true);
        clickButton.onClick.RemoveAllListeners();
        return EventSystem.current.currentSelectedGameObject; // 현재 클릭한 오브젝트
    }

    // 고기 판매
    public GameObject buyFoodObject;
    public Button foodBuyButton;
    public void Set_BuyFoodButton()
    {
        GameObject clickGoods = SetButton(foodBuyButton, buyFoodObject);
        foodBuyButton.onClick.AddListener(() => BuyFood(clickGoods));
    }
    public void BuyFood(GameObject foodGoodsObject)
    {
        GoodsData buyGoods = foodGoodsObject.GetComponent<GoodsData>();
        if (GameManager.instance.Gold < buyGoods.price) return;
        MinusGold(buyGoods.price);

        GameManager.instance.Food += buyGoods.buyFoodCount;
        UIManager.instance.UpdateFoodText(GameManager.instance.Food);

        ExitShop();
    }


    // 골드 판매
    public GameObject buyGoldObject;
    public Button goldBuyButton;
    public void Set_BuyGoldButton()
    { 
        GameObject clickGoods = SetButton(goldBuyButton, buyGoldObject);
        goldBuyButton.onClick.AddListener(() => BuyGold(clickGoods));
    }
    void BuyGold(GameObject goldGoodsObject)
    {
        GoodsData buyGoods = goldGoodsObject.GetComponent<GoodsData>();
        if (GameManager.instance.Food < buyGoods.price) return;

        GameManager.instance.Food -= buyGoods.price;
        UIManager.instance.UpdateFoodText(GameManager.instance.Food);

        GameManager.instance.Gold += buyGoods.buyGoldAmount;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

        ExitShop();
    }


    // 유닛 판매
    public GameObject buyUnitObject;
    public Button unitBuyButton;
    public void Set_BuyUnitButton()
    {
        GameObject clickGoods = SetButton(unitBuyButton, buyUnitObject);
        unitBuyButton.onClick.AddListener(() => BuyUnit(clickGoods));
    }
    void BuyUnit(GameObject foodGoodsObject)
    {
        GoodsData buyGoods = foodGoodsObject.GetComponent<GoodsData>();
        if (GameManager.instance.Gold < buyGoods.price) return;
        MinusGold(buyGoods.price);

        createDefenser.CreateSoldier(buyGoods.unitColorNumber, buyGoods.unitClassNumber);

        ExitShop();
    }

    private void OnEnable()
    {
        Set_RandomShop();
    }

    



    public bool showShop;

    public void ExitShop()
    {
        envetShop.SetActive(false);
        showShop = false;

        current_LeftGoldGoods.SetActive(false);
        current_CenterGoldGoods.SetActive(false);
        //current_RigthGoldGoods.SetActive(false);
        current_FoodGoldGoods.SetActive(false);

        current_LeftGoldGoods = null;
        current_CenterGoldGoods = null;
        //current_RigthGoldGoods = null;
        current_FoodGoldGoods = null;

        buyFoodObject.SetActive(false);
    }

    public void CancleBuy()
    {
        buyGoldObject.SetActive(false);
        buyUnitObject.SetActive(false);
        buyFoodObject.SetActive(false);

        unitBuyButton.onClick.RemoveAllListeners();
        goldBuyButton.onClick.RemoveAllListeners();
        foodBuyButton.onClick.RemoveAllListeners();
    }

    public void ShowShop()
    {
        envetShop.SetActive(true);
        showShop = true;
    }

    public void OnenvetShop()
    {
        envetShop.SetActive(true);
        Set_RandomShop();
    }

    void Set_RandomShop() // 랜덤하게 상품 변경 
    {
        current_LeftGoldGoods = Show_RandomGoods(leftGoldGoods);
        current_CenterGoldGoods = Show_RandomGoods(centerGoldGoods);
        current_FoodGoldGoods = Show_RandomGoods(foodGoods);
    }

    GameObject Show_RandomGoods(GameObject goods)
    {
        // 휘귀도 선택 부분은 가중치 둬야함
        int rarityIndex = UnityEngine.Random.Range(0, goods.transform.childCount);
        Transform goodsRarity = goods.transform.GetChild(rarityIndex);


        int goodsIndex = UnityEngine.Random.Range(0, goodsRarity.transform.childCount);
        GameObject showGoods = goodsRarity.transform.GetChild(goodsIndex).gameObject;
        //Debug.Log(showGoods);
        showGoods.SetActive(true);
        return showGoods;
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


    //private void OnMouseDown()
    //{
    //    OnEnvetShop();
    //}


}

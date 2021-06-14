﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour
{
    public Text guideText;

    public GameObject leftGoldGoods;
    public GameObject centerGoldGoods;
    //public GameObject rigthGoldGoods;
    public GameObject foodGoods;

    public GameObject[] leftGoldStocks;
    public GameObject[] centerGoldStocks; 
    //public GameObject[] rigthGoldStocks; 
    public GameObject[] foodStocks;

    private GameObject current_LeftGoldGoods = null;
    private GameObject current_CenterGoldGoods = null;
    //private GameObject current_RigthGoldGoods = null;
    private GameObject current_FoodGoldGoods = null;
    public CreateDefenser createDefenser;

    private void Awake() 
    {
        // 배열 선언
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

        // 딕셔너리 세팅
        bossShopWeighDictionary = new Dictionary<int, int[]>();
        towerShopWeighDictionary = new Dictionary<int, int[]>();
        Set_BossShopWeigh();
        Set_TowerShopWeigh();

        // 인스턴스 안되고 실행되는 버그 때문에 게임 시작시 Awake 실행 후 원위치
        gameObject.SetActive(false);
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(0, 0, 0);
    }
    void MinusGold(int price)
    {
        GameManager.instance.Gold -= price;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
    }

    void MinusFood(int price)
    {
        GameManager.instance.Food -= price;
        UIManager.instance.UpdateFoodText(GameManager.instance.Food);
    }

    void AddGold(int buyAmount)
    {
        GameManager.instance.Gold += buyAmount;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
    }

    void AddFood(int buyAmount)
    {
        GameManager.instance.Food += buyAmount;
        UIManager.instance.UpdateFoodText(GameManager.instance.Food);
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
        if (GameManager.instance.Gold < buyGoods.price) 
        {
            CancleBuy();
            LacksGold();
            return;
        }

        MinusGold(buyGoods.price);

        AddFood(buyGoods.buyFoodCount);

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
        if (GameManager.instance.Food < buyGoods.price)
        {
            CancleBuy();
            LacksGold();
            return;
        }

        MinusFood(buyGoods.price);

        AddGold(buyGoods.buyGoldAmount);

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
        if (GameManager.instance.Gold < buyGoods.price) 
        {
            CancleBuy();
            LacksGold();
            return;
        }

        MinusGold(buyGoods.price);

        createDefenser.CreateSoldier(buyGoods.unitColorNumber, buyGoods.unitClassNumber);

        ExitShop();
    }


    // 유닛 강화 판매
    public GameObject buyUnitReinforce_Object;
    public Button buyUnitReinforce_Button;

    public void Set_BuyUnitReinforceButton()
    {
        GameObject clickGoods = SetButton(buyUnitReinforce_Button, buyUnitReinforce_Object);
        buyUnitReinforce_Button.onClick.AddListener(() => BuyUnitReinforce(clickGoods));
    }

    void BuyUnitReinforce(GameObject unitReinForce_GoodsObject)
    {
        GoodsData buyGoodsData = unitReinForce_GoodsObject.GetComponent<GoodsData>();
        if (GameManager.instance.Gold < buyGoodsData.price) 
        {
            CancleBuy();
            LacksGold();
            return;
        }
        
        MinusGold(buyGoodsData.price);

        int eventNumber = buyGoodsData.reinforceEventNumber;
        int eventUnitNumber = buyGoodsData.eventUnitNumber;
        EventManager.instance.Action_SelectReinForceEvent(eventNumber, eventUnitNumber);

        ExitShop();
    }


    // 이벤트 판매
    public GameObject buyEvent_Object;
    public Button buyEvent_Button;

    public void Set_BuyEventButton()
    {
        GameObject clickGoods = SetButton(buyEvent_Button, buyEvent_Object);
        buyEvent_Button.onClick.AddListener(() => BuyEvent(clickGoods));
    }

    void BuyEvent(GameObject unitReinForce_GoodsObject)
    {
        GoodsData buyGoodsData = unitReinForce_GoodsObject.GetComponent<GoodsData>();
        if (GameManager.instance.Gold < buyGoodsData.price)
        {
            CancleBuy();
            LacksGold();
            return;
        }
        MinusGold(buyGoodsData.price);

        EventManager.instance.eventArray[buyGoodsData.eventNumber]();

        ExitShop();
    }

    // 법사 스킬강화 판매
    public GameObject buyMageUltimate_Object;
    public Button buyMageUltimate_Button;

    public void Set_BuyMageUltimateButton()
    {
        GameObject clickGoods = SetButton(buyMageUltimate_Button, buyMageUltimate_Object);
        buyMageUltimate_Button.onClick.AddListener(() => BuyMageUltimate(clickGoods));
    }

    void BuyMageUltimate(GameObject unitReinForce_GoodsObject)
    {
        GoodsData buyGoodsData = unitReinForce_GoodsObject.GetComponent<GoodsData>();
        if (GameManager.instance.Gold < buyGoodsData.price)
        {
            CancleBuy();
            LacksGold();
            return;
        }
        MinusGold(buyGoodsData.price);

        GameObject mage = UnitManager.instance.unitArrays[buyGoodsData.ultimateMageNumber].unitArray[3];
        mage.GetComponentInChildren<Unit_Mage>().isUltimate = true;
        SetCurrentMageUltimate(buyGoodsData.ultimateMageNumber);

        ExitShop();
    }

    void SetCurrentMageUltimate(int mageColorNumber)
    {
        switch (mageColorNumber)
        {
            case 0:
                GameObject[] redMages = GameObject.FindGameObjectsWithTag("RedMage");
                SetMageUltimate(redMages);
                break;
            case 1:
                GameObject[] blueMages = GameObject.FindGameObjectsWithTag("BlueMage");
                SetMageUltimate(blueMages);
                break;
            case 2:
                GameObject[] yellowMages = GameObject.FindGameObjectsWithTag("YellowMage");
                SetMageUltimate(yellowMages);
                break;
            case 3:
                GameObject[] greenMages = GameObject.FindGameObjectsWithTag("GreenMage");
                SetMageUltimate(greenMages);
                break;
            case 4:
                GameObject[] orangeMages = GameObject.FindGameObjectsWithTag("OrangeMage");
                SetMageUltimate(orangeMages);
                break;
            case 5:
                GameObject[] violetMages = GameObject.FindGameObjectsWithTag("VioletMage");
                SetMageUltimate(violetMages);
                break;
            case 6:
                GameObject[] blackMages = GameObject.FindGameObjectsWithTag("BlackMage");
                SetMageUltimate(blackMages);
                break;
        }
    }

    void SetMageUltimate(GameObject[] mages)
    {
        for(int i = 0; i < mages.Length; i++)
        {
            Unit_Mage mage = mages[i].GetComponentInParent<Unit_Mage>();
            mage.isUltimate = true;
        }
    }
    //private void OnEnable()
    //{
    //    OnShop(3, bossShopWeighDictionary);
    //}

    public bool showShop;

    public void OnShop(int level, Dictionary<int, int[]> goodsWeighDictionary)
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
        showShop = true;
        Show_RandomShop(level, goodsWeighDictionary);
    }

    public void SetGuideText(string message)
    {
        guideText.text = message;
    }

    public void ExitShop()
    {
        gameObject.SetActive(false);
        showShop = false;
        SetGuideText("");

        current_LeftGoldGoods.SetActive(false);
        current_CenterGoldGoods.SetActive(false);
        current_FoodGoldGoods.SetActive(false);

        current_LeftGoldGoods = null;
        current_CenterGoldGoods = null;
        current_FoodGoldGoods = null;

        CancleBuy();
        Time.timeScale = 1;
    }

    public void CancleBuy()
    {
        buyGoldObject.SetActive(false);
        buyUnitObject.SetActive(false);
        buyFoodObject.SetActive(false);
        buyUnitReinforce_Object.SetActive(false);
        buyEvent_Object.SetActive(false);
        buyMageUltimate_Object.SetActive(false);

        unitBuyButton.onClick.RemoveAllListeners();
        goldBuyButton.onClick.RemoveAllListeners();
        foodBuyButton.onClick.RemoveAllListeners();
        buyUnitReinforce_Button.onClick.RemoveAllListeners();
        buyEvent_Button.onClick.RemoveAllListeners();
        buyMageUltimate_Button.onClick.RemoveAllListeners();
    }

    public Text lacksGuideText;
    void LacksGold()
    {
        StopCoroutine("HideGoldText_Coroutine");
        StartCoroutine("HideGoldText_Coroutine");
    }

    IEnumerator HideGoldText_Coroutine()
    {
        lacksGuideText.gameObject.SetActive(true);

        lacksGuideText.color = new Color32(255, 44, 35, 255);
        Color textColor;
        textColor = lacksGuideText.color;

        // Time.timeScale의 영향을 받지 않게 하기 위한 코드 (코루틴은 안멈추는데 WaitForSecond가 멈춤)
        float pastTime = 0;
        float delayTime = 0.8f;
        while (delayTime > pastTime)
        {
            pastTime += Time.unscaledDeltaTime;
            yield return null;
        }

        while (textColor.a > 0.1f)
        {
            textColor.a -= 0.02f;
            lacksGuideText.color = textColor;
            yield return null;
        }

        lacksGuideText.gameObject.SetActive(false);
    }


    public void ShowShop()
    {
        gameObject.SetActive(true);
        showShop = true;
    }

    // 상품 세팅
    void Show_RandomShop(int level, Dictionary<int, int[]> goodsWeighDictionary) // 랜덤하게 상품 변경 
    {
        current_LeftGoldGoods = Set_RandomGoods(leftGoldGoods, level, goodsWeighDictionary);
        current_CenterGoldGoods = Set_RandomGoods(centerGoldGoods, level, goodsWeighDictionary);
        current_FoodGoldGoods = Set_RandomGoods(foodGoods, level, goodsWeighDictionary);
    }

    public Dictionary<int, int[]> bossShopWeighDictionary;
    public Dictionary<int, int[]> towerShopWeighDictionary;
    int totalWeigh = 100;
    // 확률 가중치
    void Set_BossShopWeigh()
    {
        bossShopWeighDictionary.Add(1, new int[] { 70, 30, 0 });
        bossShopWeighDictionary.Add(2, new int[] { 30, 60, 10 });
        bossShopWeighDictionary.Add(3, new int[] { 15, 45, 40 });
        bossShopWeighDictionary.Add(4, new int[] { 0, 30, 70 });
    }
    void Set_TowerShopWeigh()
    {
        towerShopWeighDictionary.Add(1, new int[] { 85, 15, 0 });
        towerShopWeighDictionary.Add(2, new int[] { 70, 30, 0 });
        towerShopWeighDictionary.Add(3, new int[] { 55, 40, 5 });
        towerShopWeighDictionary.Add(4, new int[] { 35, 55, 10 });
        towerShopWeighDictionary.Add(5, new int[] { 10, 60, 30 });
        towerShopWeighDictionary.Add(6, new int[] { 0, 30, 70 });
    }

    GameObject Set_RandomGoods(GameObject goods, int level, Dictionary<int, int[]> goodsWeighDictionary)
    {
        Transform goodsRarity = null;
        int randomNumber = UnityEngine.Random.Range(0, totalWeigh);
        for (int i = 0; i < goods.transform.childCount; i++)
        {
            if (goodsWeighDictionary[level][i] >= randomNumber)
            {
                goodsRarity = goods.transform.GetChild(i);
                break;
            }
            else randomNumber -= goodsWeighDictionary[level][i];
        }

        // 휘귀도 선택 후 상품 랜덤 선택
        int goodsIndex = UnityEngine.Random.Range(0, goodsRarity.transform.childCount);
        GameObject showGoods = goodsRarity.transform.GetChild(goodsIndex).gameObject;
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

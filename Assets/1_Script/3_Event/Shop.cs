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

        itemWeighDictionary = new Dictionary<int, int[]>();
        SetItemWeigh();
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
        if (GameManager.instance.Gold < buyGoods.price) 
        {
            CancleBuy();
            LacksGold();
            return;
        }

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
        if (GameManager.instance.Food < buyGoods.price)
        {
            CancleBuy();
            LacksGold();
            return;
        }

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

        int eventNumber = buyGoodsData.eventNumber;
        int eventUnitNumber = buyGoodsData.eventUnitNumber;
        EventManager.instance.Action_SelectEvent(eventNumber, eventUnitNumber);

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

        CurrentEnemyDie(10); // 이건 무조건 바꿔야함 액션 리스트 형식으로

        ExitShop();
    }

    void CurrentEnemyDie(int dieEnemyCount)
    {
        for(int i = 0; i < dieEnemyCount; i++)
        {
            if (enemySpawn.currentEnemyList.Count == 0) break;

            int dieEnemyNumber = Random.Range(0, enemySpawn.currentEnemyList.Count);
            NomalEnemy enemy = enemySpawn.currentEnemyList[dieEnemyNumber].GetComponent<NomalEnemy>();
            if (enemy != null) enemy.Dead();
        }
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
        current_FoodGoldGoods.SetActive(false);

        current_LeftGoldGoods = null;
        current_CenterGoldGoods = null;
        current_FoodGoldGoods = null;

        CancleBuy();
    }

    public void CancleBuy()
    {
        buyGoldObject.SetActive(false);
        buyUnitObject.SetActive(false);
        buyFoodObject.SetActive(false);
        buyUnitReinforce_Object.SetActive(false);
        buyEvent_Object.SetActive(false);

        unitBuyButton.onClick.RemoveAllListeners();
        goldBuyButton.onClick.RemoveAllListeners();
        foodBuyButton.onClick.RemoveAllListeners();
        buyUnitReinforce_Button.onClick.RemoveAllListeners();
        buyEvent_Button.onClick.RemoveAllListeners();
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

        lacksGuideText.color = new Color32(255,44, 35, 255);
        Color textColor;
        textColor = lacksGuideText.color;

        yield return new WaitForSeconds(0.8f);
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
        envetShop.SetActive(true);
        showShop = true;
    }

    public void OnEnvetShop()
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

    Dictionary<int, int[]> itemWeighDictionary;
    int totalWeigh = 100;
    void SetItemWeigh()
    {
        itemWeighDictionary.Add(1, new int[] { 70, 30, 0 });
        itemWeighDictionary.Add(2, new int[] { 40, 50, 10 });
        itemWeighDictionary.Add(3, new int[] { 15, 45, 40 });
        itemWeighDictionary.Add(4, new int[] { 0, 30, 70 });
    }

    public EnemySpawn enemySpawn;
    GameObject Show_RandomGoods(GameObject goods)
    {
        // 휘귀도 선택 부분은 가중치 둬야함
        Transform goodsRarity = null;
        int randomNumber = UnityEngine.Random.Range(0, totalWeigh);
        for (int i = 0; i < goods.transform.childCount; i++)
        {
            if (itemWeighDictionary[enemySpawn.bossLevel][i] >= randomNumber)
            {
                goodsRarity = goods.transform.GetChild(i);
                break;
            }
            else randomNumber -= itemWeighDictionary[enemySpawn.bossLevel][i];
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

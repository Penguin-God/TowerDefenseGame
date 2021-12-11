using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public enum TriggerType
{
    None,
    Boss,
    EnemyTower,
}

public class Shop : MonoBehaviour
{
    public Text shopGuideText;

    public GameObject leftGoldGoods;
    public GameObject centerGoldGoods;
    public GameObject foodGoods;

    public GameObject[] leftGoldStocks;
    public GameObject[] centerGoldStocks; 
    public GameObject[] foodStocks;

    private GameObject current_LeftGoldGoods = null;
    private GameObject current_CenterGoldGoods = null;
    private GameObject current_FoodGoldGoods = null;
    public CreateDefenser createDefenser;


    // 나의 의도 : Panel을 하나로 묶어서 쉽고 편하게살자
    [SerializeField] GameObject panelObject = null;
    [SerializeField] Button panel_YesButton = null;
    [SerializeField] Text panelGuideText = null;

    public void SetPanel(string guideText, Action onClickYes)
    {
        SetPanelButton(ref panel_YesButton, onClickYes);

        panelObject.SetActive(true);
        panelGuideText.text = guideText;
    }

    void SetPanelButton(ref Button panelButton, Action action)
    {
        panelButton.onClick.RemoveAllListeners();
        panelButton.onClick.AddListener(() => action());
    }

    public void CanclePanelAction()
    {
        panelObject.SetActive(false);
        panel_YesButton.onClick.RemoveAllListeners();
        panelGuideText.text = "";
    }

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

    [ContextMenu("SetData")]
    void SetData()
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

    // 이 부분도 뜯어고칠 여지가 많음 굳이 여기서 판매, 파괴를 구현할 이유가 없음
    public void BuyItem(GameObject item)
    {
        SellEventShopItem buyItem = item.GetComponent<SellEventShopItem>();
        if (buyItem != null && buyItem.BuyAble)
        {
            buyItem.Sell_Item();
            GoodsPurchase(item);
        }
        else LacksGold();
    }

    void GoodsPurchase(GameObject goodsObject)
    {
        SoundManager.instance.PlayEffectSound_ByName("PurchaseItem");
        Destroy(goodsObject, 0.1f);

        Transform goodsStock = goodsObject.transform.parent;
        // 조건에 childCount가 1개인 이유는 0.1f 파괴 대기 중이라 아직 파괴가 안되서 1가 남아있음
        if (goodsStock.childCount == 1) Destroy(goodsStock.gameObject); // 물품을 다 샀으면 등급 파괴
        ExitShop();
    }

    [SerializeField] Button[] dontClickButtons;

    void SetButtonRayCast(bool isRaycast) // 상점 이용 시 특정 버튼 끄고 키기
    {
        for(int i = 0; i < dontClickButtons.Length; i++)
        {
            dontClickButtons[i].enabled = isRaycast;
        }
    }


    private void OnEnable()
    {
        EnterEventShop(currentLevel, currentShopType);
    }

    public void OnShop(int level, TriggerType type)
    {
        Set_ShopData(level, type);
        gameObject.SetActive(true);
    }

    public int currentLevel = 0;
    public TriggerType currentShopType = TriggerType.None;

    void Set_ShopData(int level, TriggerType type)
    {
        currentShopType = type;
        currentLevel = level;
    }


    void EnterEventShop(int level, TriggerType type)
    {
        obj_ShopReset.SetActive(true);
        Time.timeScale = 0;
        SetButtonRayCast(false);

        string guideText = (type == TriggerType.Boss) ? "보스를 처치하였습니다" : "적군의 성을 파괴하였습니다";
        SetGuideText(guideText);

        Show_RandomShop(level, type);
    }

    public void SetGuideText(string message)
    {
        shopGuideText.text = message;
    }

    public void ExitShop()
    {
        gameObject.SetActive(false);
        SetGuideText("");

        Disabled_CurrentShowGoods();
        CanclePanelAction();

        lacksGuideText.gameObject.SetActive(false);
        Time.timeScale = GameManager.instance.gameTimeSpeed;
        SetButtonRayCast(true);
    }

    void Disabled_CurrentShowGoods()
    {
        Disabled_Goods(ref current_LeftGoldGoods);
        Disabled_Goods(ref current_CenterGoldGoods);
        Disabled_Goods(ref current_FoodGoldGoods);
    }

    void Disabled_Goods(ref GameObject goods)
    {
        if (goods != null) goods.SetActive(false);
        goods = null;
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
        SoundManager.instance.PlayEffectSound_ByName("LackPurchaseGold");

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
    }

    // 상품 세팅
    void Show_RandomShop(int level, TriggerType type) // 랜덤하게 상품 변경 
    {
        Dictionary<int, int[]> goodsWeighDictionary = (type == TriggerType.Boss) ? bossShopWeighDictionary : towerShopWeighDictionary;

        current_LeftGoldGoods = Set_RandomGoods(leftGoldGoods, level, goodsWeighDictionary);
        current_CenterGoldGoods = Set_RandomGoods(centerGoldGoods, level, goodsWeighDictionary);
        current_FoodGoldGoods = Set_RandomGoods(foodGoods, level, goodsWeighDictionary);
    }

    GameObject Set_RandomGoods(GameObject goods, int level, Dictionary<int, int[]> goodsWeighDictionary)
    {
        Transform goodsRarity = null;
        int totalWeigh = 100;
        int randomNumber = UnityEngine.Random.Range(0, totalWeigh);

        for (int i = 0; i < goods.transform.childCount; i++) // 레벨 가중치에 따라 상품 등급 정함
        {
            if (goodsWeighDictionary[level][i] >= randomNumber)
            {
                goodsRarity = goods.transform.GetChild(i);
                break;
            }
            else randomNumber -= goodsWeighDictionary[level][i];
        }
        if (goodsRarity == null) goodsRarity = goods.transform.GetChild(0); // 등급파괴되서 null이면 첫번째 등급으로

        // 휘귀도 선택 후 상품 랜덤 선택
        int goodsIndex = UnityEngine.Random.Range(0, goodsRarity.transform.childCount);
        GameObject showGoods = goodsRarity.GetChild(goodsIndex).gameObject;
        showGoods.SetActive(true);
        return showGoods;
    }

    // 확률 가중치 딕셔너리
    public Dictionary<int, int[]> bossShopWeighDictionary;
    public Dictionary<int, int[]> towerShopWeighDictionary;
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


    public void SetShopExitPanel(string text)
    {
        text = GetEscapeText(text);
        SetPanel(text, ExitShop);
    }

    public void SetShopResetPanel(string text)
    {
        text = GetEscapeText(text);
        SetPanel(text, ResetShop);
    }

    string GetEscapeText(string text)
    {
        return text.Replace("\\n", "\n");
    }

    // 상점 재설정
    [SerializeField] GameObject obj_ShopReset = null;
    public void ResetShop()
    {
        if (GameManager.instance.Gold >= 10)
        {
            SoundManager.instance.PlayEffectSound_ByName("Click_XButton");
            GameManager.instance.Gold -= 10;
            UIManager.instance.UpdateGoldText(GameManager.instance.Gold);

            Disabled_CurrentShowGoods();
            CanclePanelAction();
            Show_RandomShop(currentLevel, currentShopType);

            obj_ShopReset.SetActive(false);
        }
        else LacksGold();

        panelObject.SetActive(false);
    }
}

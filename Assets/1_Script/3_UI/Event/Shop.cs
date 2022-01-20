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

public enum GoodsPosition
{
    Left,
    Center,
    Right
}

public class Shop : MonoBehaviour
{
    [SerializeField] Text shopGuideText;

    [SerializeField] GameObject leftGoldGoods;
    [SerializeField] GameObject centerGoldGoods;
    [SerializeField] GameObject foodGoods;

    Dictionary<GoodsPosition, GameObject> GoodsPositionDic = new Dictionary<GoodsPosition, GameObject>();
    [SerializeField] GameObject current_LeftGoldGoods = null;
    [SerializeField] GameObject current_CenterGoldGoods = null;
    [SerializeField] GameObject current_FoodGoldGoods = null;

    private void Update()
    {
        current_LeftGoldGoods = GoodsPositionDic[GoodsPosition.Left];
        current_CenterGoldGoods = GoodsPositionDic[GoodsPosition.Center];
        current_FoodGoldGoods = GoodsPositionDic[GoodsPosition.Right];
    }

    private void Awake()
    {
        // 물품 선언
        //leftGoldStocks = SettingStocks(leftGoldGoods.transform);
        //centerGoldStocks = SettingStocks(centerGoldGoods.transform);
        //foodStocks = SettingStocks(foodGoods.transform);

        // 딕셔너리 세팅
        SettingGoodsPosition();
        Set_BossShopWeigh();
        Set_TowerShopWeigh();

        // 인스턴스 안되고 실행되는 버그 때문에 게임 시작시 Awake 실행 후 원위치
        gameObject.SetActive(false);
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(0, 0, 0);
    }

    void SettingGoodsPosition()
    {
        GoodsPositionDic.Add(GoodsPosition.Left, null);
        GoodsPositionDic.Add(GoodsPosition.Center, null);
        GoodsPositionDic.Add(GoodsPosition.Right, null);
    }

    public void ChangeGoods(GoodsPosition _position, GameObject _goods)
    {
        if (GoodsPositionDic.ContainsKey(_position)) GoodsPositionDic[_position] = _goods;
    }
    //GameObject[] SettingStocks(Transform _stocksPrent)
    //{
    //    GameObject[] _stocks = new GameObject[_stocksPrent.transform.childCount];
    //    for (int i = 0; i < _stocks.Length; i++) _stocks[i] = _stocksPrent.transform.GetChild(i).gameObject;
    //    return _stocks;
    //}


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
            dontClickButtons[i].enabled = isRaycast;
    }


    // 외부에서 상점 열때 사용
    public void OnShop(int level, TriggerType type)
    {
        Set_ShopData(level, type);
        gameObject.SetActive(true);
    }

    [SerializeField] int currentLevel = 1;
    [SerializeField] TriggerType currentShopType = TriggerType.Boss;
    void Set_ShopData(int level, TriggerType type)
    {
        currentShopType = type;
        currentLevel = level;
    }

    private void OnEnable()
    {
        EnterEventShop(currentLevel, currentShopType);
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

    public void SetGuideText(string message) => shopGuideText.text = message;



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
        GoodsPositionDic[GoodsPosition.Left].SetActive(false);
        GoodsPositionDic[GoodsPosition.Center].SetActive(false);
        GoodsPositionDic[GoodsPosition.Right].SetActive(false);

        GoodsPositionDic[GoodsPosition.Left] = null;
        GoodsPositionDic[GoodsPosition.Center] = null;
        GoodsPositionDic[GoodsPosition.Right] = null;
    }

    void Disabled_Goods(ref GameObject goods)
    {
        if (goods != null) goods.SetActive(false);
        goods = null;
    }

    // 상품 세팅
    void Show_RandomShop(int level, TriggerType type) // 랜덤하게 상품 변경 
    {
        Dictionary<int, int[]> goodsWeighDictionary = (type == TriggerType.Boss) ? bossShopWeighDictionary : towerShopWeighDictionary;

        Set_RandomGoods(leftGoldGoods, level, goodsWeighDictionary);
        Set_RandomGoods(centerGoldGoods, level, goodsWeighDictionary);
        Set_RandomGoods(foodGoods, level, goodsWeighDictionary);

        //SetGoodsEvent(ref current_LeftGoldGoods, current_LeftGoldGoods);
        //SetGoodsEvent(ref current_CenterGoldGoods, current_CenterGoldGoods);
        //SetGoodsEvent(ref current_FoodGoldGoods, current_FoodGoldGoods);
    }

    void Set_RandomGoods(GameObject goods, int level, Dictionary<int, int[]> goodsWeighDictionary)
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
        if (showGoods.GetComponent<IGoodsSeleter>() != null) showGoods = showGoods.GetComponent<IGoodsSeleter>().GetGoods();
        showGoods.SetActive(true);
    }

    public Action<GameObject> SetGoodsEventAction => SetGoodsEvent;

    // 인자값은 SellEventShopItem에서 넣어줌
    void SetGoodsEvent(GameObject _goods)
    {
        _goods.GetComponent<SellEventShopItem>().AddListener((string _text, bool _byeAble, Action _OnSell) => 
        {
            if (_OnSell != null)
            {
                Action _BuyAction = null;

                _BuyAction += () =>
                {
                    if (_byeAble)
                    {
                        _OnSell();
                        GoodsPurchase(_goods);
                    }
                    else LacksGold();
                };

                SetPanel(_text, _BuyAction);
            }
        });
    }

    

    // 확률 가중치 딕셔너리
    public Dictionary<int, int[]> bossShopWeighDictionary;
    public Dictionary<int, int[]> towerShopWeighDictionary;
    void Set_BossShopWeigh()
    {
        bossShopWeighDictionary = new Dictionary<int, int[]>();
        bossShopWeighDictionary.Add(1, new int[] { 70, 30, 0 });
        bossShopWeighDictionary.Add(2, new int[] { 30, 60, 10 });
        bossShopWeighDictionary.Add(3, new int[] { 15, 45, 40 });
        bossShopWeighDictionary.Add(4, new int[] { 0, 30, 70 });
    }
    void Set_TowerShopWeigh()
    {
        towerShopWeighDictionary = new Dictionary<int, int[]>();
        towerShopWeighDictionary.Add(1, new int[] { 85, 15, 0 });
        towerShopWeighDictionary.Add(2, new int[] { 70, 30, 0 });
        towerShopWeighDictionary.Add(3, new int[] { 55, 40, 5 });
        towerShopWeighDictionary.Add(4, new int[] { 35, 55, 10 });
        towerShopWeighDictionary.Add(5, new int[] { 10, 60, 30 });
        towerShopWeighDictionary.Add(6, new int[] { 0, 30, 70 });
    }


    // Button Click Evnet 구독에 쓰이는 함수들
    public void SetShopExitPanel(string text)
    {
        text = GetEscapeText(text);
        SetPanel(text, ExitShop);
    }
    string GetEscapeText(string text)
    {
        return text.Replace("\\n", "\n");
    }

    public void SetShopResetPanel(string text)
    {
        text = GetEscapeText(text);
        SetPanel(text, ResetShop);
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

        panelObject.SetActive(false);
    }

    // 골드 부족 안내
    [SerializeField] Text lacksGuideText;
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
}

using UnityEngine;
using UnityEngine.UI;
using System;

public enum PriceType
{
    Gold,
    Food,
}

public class SellEventShopItem : MonoBehaviour
{
    public PriceType priceType;
    public int price;
    public string itemName;
    [Tooltip("선언한 텍스트 뒤에 ' 구입하시겠습니까?'라는 문구가 붙음 ")]
    public string goodsInformation;

    public bool BuyAble // 골드 부족을 상점에서 뛰어서 조건 검사는 shop에서
    {
        get
        {
            switch (priceType)
            {
                case PriceType.Gold: return GameManager.instance.Gold > price;
                case PriceType.Food: return GameManager.instance.Food > price;
            }
            return false;
        }
    }

    [SerializeField] Shop shop;
    private void Start()
    {
        shop = GetComponentInParent<Shop>();
        buyPanelObject = shop.buyPanel;
        buyButton = shop.buyButton;
        guideText = shop.buyGuideText;
        itemName = gameObject.name;

        GetComponent<Button>().onClick.AddListener(() => SetByPanel()); // 자기 자신 클릭 시
    }

    [SerializeField] GameObject buyPanelObject;
    [SerializeField] Button buyButton;
    void SetByPanel()
    {
        SoundManager.instance.PlayEffectSound_ByName("ShopItemClick");
        buyButton.onClick.RemoveAllListeners();
        Set_BuyGuideText();
        buyPanelObject.SetActive(true);
        buyButton.onClick.AddListener(() => shop.BuyItem(gameObject));
    }


    public void Sell_Item()
    {
        if (GetComponent<ISellEventShopItem>() != null)
        {
            SpendMoney(priceType);
            GetComponent<ISellEventShopItem>().Sell_Item();
        }
    }

    
    [SerializeField] Text guideText;
    void Set_BuyGuideText()
    {
        guideText.text = goodsInformation + " 구입하시겠습니까?";
    }

    void SpendMoney(PriceType priceType)
    {
        switch (priceType)
        {
            case PriceType.Gold: SubTractGold(); break;
            case PriceType.Food: SubTractFood(); break;
        }
    }

    void SubTractGold()
    {
        GameManager.instance.Gold -= price;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
    }

    void SubTractFood()
    {
        GameManager.instance.Food -= price;
        UIManager.instance.UpdateFoodText(GameManager.instance.Food);
    }


    /* public int unitColorNumber;
    public int unitClassNumber;

    public int buyFoodCount;

    public int buyGoldAmount;

    [Tooltip("0 : 대미지 증가, 1 : 보스 대미지 증가, 2 : 스킬 사용 빈도 증가, 3 : 패시브 강화")]
    public int reinforceEventNumber;
    public int eventUnitNumber;

    public int eventNumber;

    public int ultimateMageNumber; */
}

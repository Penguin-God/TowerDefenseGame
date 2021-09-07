using UnityEngine;
using UnityEngine.UI;

public enum PriceType
{
    Gold,
    Food,
}

public class SellEventShopItem : MonoBehaviour
{
    private void Awake()
    {
        EventShopItemDataBase dataBase = GetComponentInParent<EventShopItemDataBase>();
        EventShopItemData data = dataBase.itemDatas.Find(itemData => itemData.name == itemName);
        if(data != null)
        {
            price = data.price;
            priceType = GetPriceType(data.type);
            goodsInformation = data.info;
        }
    }

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

    PriceType GetPriceType(string type)
    {
        switch (type)
        {
            case "Gold": return PriceType.Gold;
            case "Food": return PriceType.Food;
        }

        return PriceType.Gold;
    }

    [SerializeField] Shop shop;
    private void Start()
    {
        shop = GetComponentInParent<Shop>();
        buyPanelObject = shop.buyPanel;
        buyButton = shop.buyButton;
        guideText = shop.buyGuideText;

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
}

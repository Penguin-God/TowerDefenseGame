using UnityEngine;
using UnityEngine.UI;
public enum GoodsType
{
    Gold,
    Food,
}

public class SellEventShopItem : MonoBehaviour
{
    public GoodsType goodsType;
    public int price;
    public string itemName;
    public string goodsInformation;

    public bool BuyAble // 골드 부족을 상점에서 뛰어서 조건 검사는 shop에서
    {
        get
        {
            switch (goodsType)
            {
                case GoodsType.Gold: return price > GameManager.instance.Gold;
                case GoodsType.Food: return price > GameManager.instance.Food;
            }
            return false;
        }
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => SetByPanel());
    }

    void SetByPanel()
    {
        Set_BuyGuideText();
        buyPanelObject.SetActive(true);
        shop.onBuy = Sell_Item;
    }
    [SerializeField] Shop shop;

    public void Sell_Item()
    {
        SpendMoney(goodsType);
        GetComponent<ISellEventShopItem>().Sell_Item();
    }

    [SerializeField] GameObject buyPanelObject;
    [SerializeField] Text guideText;
    void Set_BuyGuideText()
    {
        guideText.text = goodsInformation + " 구입하시겠습니까?";
    }

    void SpendMoney(GoodsType goodsType)
    {
        switch (goodsType)
        {
            case GoodsType.Gold: SubTractGold(); break;
            case GoodsType.Food: SubTractFood(); break;
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



    [Tooltip("선언한 텍스트 뒤에 ' 구입하시겠습니까?'라는 문구가 붙음 ")]
    //public string goodsInformation;

    public int unitColorNumber;
    public int unitClassNumber;

    public int buyFoodCount;

    public int buyGoldAmount;

    [Tooltip("0 : 대미지 증가, 1 : 보스 대미지 증가, 2 : 스킬 사용 빈도 증가, 3 : 패시브 강화")]
    public int reinforceEventNumber;
    public int eventUnitNumber;

    public int eventNumber;

    public int ultimateMageNumber;
}

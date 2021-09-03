using UnityEngine;

public enum GoodsType
{
    Gold,
    Food,
}

public class GoodsData : MonoBehaviour
{
    public GoodsType goodsType;
    public int price;
    public string itemName;
    public string goodsInformation;

    public bool BuyAble
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


    public void Sell_Item()
    {
        SpendMoney(goodsType);
        GetComponent<ISellEventShopItem>().Sell_Item();
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

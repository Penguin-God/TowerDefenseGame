using System.Collections;
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
    public GoodsPosition Position;
    public PriceType priceType;
    public int price;
    public string itemName;
    [Tooltip("선언한 텍스트 뒤에 ' 구입하시겠습니까?'라는 문구가 붙음 ")]
    public string goodsInformation;

    private void Awake()
    {
        shop = GetComponentInParent<Shop>();
        myButton = GetComponent<Button>();

        EventShopItemDataBase dataBase = GetComponentInParent<EventShopItemDataBase>();
        EventShopItemData data = dataBase.itemDatas.Find(itemData => itemData.name == itemName);
        if(data != null) SetData(data.price, GetPriceType(data.type), data.info);
    }

    public void SetData(int _price, PriceType _type, string _info)
    {
        price = _price;
        priceType = _type;
        goodsInformation = _info;
        goodsInformation += " 구입하시겠습니까?";
    }

    private void OnEnable()
    {
        shop.ChangeGoods(Position, gameObject);
        shop.SetGoodsEventAction(gameObject);
    }

    protected Shop shop = null;
    protected Button myButton = null;
    // shop에서 사용, shop에서 팔 상품을 고르고 이 함수를 사용해서 클릭시 행동을 정의
    public void AddListener(Action<string ,bool, Action> OnClick)
    {
        if(OnClick != null)
        {
            myButton.onClick.RemoveAllListeners();
            myButton.onClick.AddListener(() => OnClick(goodsInformation, BuyAble, Get_SellAction()));
        }
    }

    public bool BuyAble
    {
        get
        {
            switch (priceType)
            {
                case PriceType.Gold: return GameManager.instance.Gold >= price;
                case PriceType.Food: return GameManager.instance.Food >= price;
                default: return false;
            }
        }
    }

    PriceType GetPriceType(string type)
    {
        switch (type)
        {
            case "Gold": return PriceType.Gold;
            case "Food": return PriceType.Food;
            default: return PriceType.Gold;
        }
    }

    // 아이템 판매
    public Action Get_SellAction()
    {
        Action _action = null;
        if (GetComponent<ISellEventShopItem>() != null)
        {
            _action += () => SpendMoney(priceType);
            _action += GetComponent<ISellEventShopItem>().Sell_Item;
        }
        return _action;
    }

    // 재화 사용
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

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
    protected Shop shop = null;
    Button myButton = null;

    private void Awake()
    {
        shop = GetComponentInParent<Shop>();
        myButton = GetComponent<Button>();

        EventShopItemDataBase dataBase = GetComponentInParent<EventShopItemDataBase>();
        EventShopItemData data = dataBase.itemDatas.Find(itemData => itemData.name == itemName);
        if(data != null)
        {
            price = data.price;
            priceType = GetPriceType(data.type);
            goodsInformation = data.info;
            goodsInformation += " 구입하시겠습니까?";
        }
    }

    // shop에서 사용
    public void AddListener(Action<bool, Action> OnClick)
    {
        if(OnClick != null)
        {
            myButton.onClick.RemoveAllListeners();
            myButton.onClick.AddListener(() => OnClick(BuyAble, Get_SellAction()));
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

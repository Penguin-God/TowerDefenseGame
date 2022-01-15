using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyShopGoods : MonoBehaviour
{

    public string itemName;
    public PriceType priceType;

    public int Price { get { return prices[goodsLevel]; } }

    int[] prices = null;
    int goodsLevel = 0;

    public bool BuyAble // 골드 부족을 상점에서 뛰어서 조건 검사는 shop에서
    {
        get
        {
            switch (priceType)
            {
                case PriceType.Gold: return GameManager.instance.Gold > Price;
                case PriceType.Food: return GameManager.instance.Food > Price;
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

    public void BuyGoods()
    {
        if (GetComponent<ISellEventShopItem>() != null)
        {
            SpendMoney(priceType);
            GetComponent<ISellEventShopItem>().Sell_Item();
        }
    }

    public void SpendMoney(PriceType priceType)
    {
        if (!BuyAble)
        {
            Debug.Log("못사요 못사");
            return;
        }
        switch (priceType)
        {
            case PriceType.Gold: SubTractGold(); break;
            case PriceType.Food: SubTractFood(); break;
        }
    }

    public void SpendMoney(string priceType)
    {
        if (!BuyAble)
        {
            Debug.Log("못사요 못사");
            return;
        }
        switch (priceType)
        {
            case "Gold":SubTractGold(); break;
            case "Food": SubTractFood(); break;
        }
    }

    void SubTractGold()
    {
        GameManager.instance.Gold -= Price;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
    }

    void SubTractFood()
    {
        GameManager.instance.Food -= Price;
        UIManager.instance.UpdateFoodText(GameManager.instance.Food);
    }


    private void OnMouseDown()
    {
        ShowShopWindow();
    }

    [SerializeField] GameObject obj_ShopWindow;
    void ShowShopWindow()
    {
        obj_ShopWindow.SetActive(true);
    }
}

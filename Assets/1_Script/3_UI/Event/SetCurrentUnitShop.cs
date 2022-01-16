using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCurrentUnitShop : GoodsSeleter
{
    [SerializeField] GameObject SeletedGoods = null;

    //public enum GoodsType { mageUltimate };
    //public GoodsType goodsType;

    public override void AddListener(System.Action<string, bool, System.Action> OnClick)
    {
        for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(false);
        SeletedGoods = SetMageUltimateGoods();
        SellEventShopItem _data = SeletedGoods.GetComponent<SellEventShopItem>();
        GetComponent<SellEventShopItem>().SetData(_data.price, _data.priceType, _data.goodsInformation);
        Debug.Log(1241287781387);
        SeletedGoods.GetComponent<Button>().onClick.RemoveAllListeners();
        SeletedGoods.GetComponent<Button>().onClick.AddListener(() => OnClick(goodsInformation, BuyAble, SeletedGoods.GetComponent<ISellEventShopItem>().Sell_Item));
    }

    private void Test()
    {
        for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(false);
        SeletedGoods = SetMageUltimateGoods();
        SellEventShopItem _data = SeletedGoods.GetComponent<SellEventShopItem>();
        GetComponent<SellEventShopItem>().SetData(_data.price, _data.priceType, _data.goodsInformation);
    }

    GameObject SetMageUltimateGoods()
    {
        List<int> mageUltimateGoodsList = null; //EventManager.instance.Return_CurrentUnitColorList(3);

        // 현재 법사 없으면 그냥 랜덤
        if (mageUltimateGoodsList == null || mageUltimateGoodsList.Count == 0) return SetRandomGoods();

        int listIndex = Random.Range(0, mageUltimateGoodsList.Count);
        int GoodsIndex = mageUltimateGoodsList[listIndex];
        GameObject _obj = transform.GetChild(GoodsIndex).gameObject;
        _obj.SetActive(true);
        return _obj;
    }

    GameObject SetRandomGoods()
    {
        int random = Random.Range(0, transform.childCount);
        GameObject _obj = transform.GetChild(random).gameObject;
        _obj.SetActive(true);
        return _obj;
    }
}

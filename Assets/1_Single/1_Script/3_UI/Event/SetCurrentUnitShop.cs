using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCurrentUnitShop : MonoBehaviour, IGoodsSeleter
{
    //[SerializeField] GameObject SeletedGoods = null;
    //public override void AddListener(System.Action<string, bool, System.Action> OnClick)
    //{
    //    SeletedGoods = SetMageUltimateGoods();
    //    SellEventShopItem _data = SeletedGoods.GetComponent<SellEventShopItem>();
    //    Button _button = SeletedGoods.GetComponent<Button>();

    //    base.SetData(_data.price, _data.priceType, _data.goodsInformation);
    //    Debug.Log(1241287781387);
    //    _button.onClick.RemoveAllListeners();
    //    _button.onClick.AddListener(() => OnClick(goodsInformation, BuyAble, SeletedGoods.GetComponent<ISellEventShopItem>().Sell_Item));
    //}

    GameObject SetMageUltimateGoods()
    {
        TeamSoldier[] mages = UnitManager.instance.GetCurrnetUnits(UnitClass.mage);

        // 현재 법사 없으면 그냥 랜덤
        if (mages == null || mages.Length == 0) return SetRandomGoods();

        int listIndex = Random.Range(0, mages.Length);
        int GoodsIndex = (int)mages[listIndex].unitClass;

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

    private void OnDisable()
    {
        for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(false);
    }

    public GameObject GetGoods()
    {
        return SetMageUltimateGoods();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class UI_GoodsChangeController : UI_Base
{
    enum Buttons
    {
        ChangeGoodsButton,
    }

    UI_BattleShopGoods _goods;
    Func<GoodsLocation, BattleShopGoodsData, BattleShopGoodsData> _getNewGoods;
    protected override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        _goods = GetComponent<UI_BattleShopGoods>();

        GetButton((int)Buttons.ChangeGoodsButton).onClick.AddListener(ChangeGoods);
    }

    public void DependencyInject(Func<GoodsLocation, BattleShopGoodsData, BattleShopGoodsData> getNewGoods) => _getNewGoods = getNewGoods;
    public void ActiveButton()
    {
        if(_initDone)
            GetButton((int)Buttons.ChangeGoodsButton).gameObject.SetActive(true);
    }
    void ChangeGoods()
    {
        _goods.DisplayGoods(_getNewGoods.Invoke(_goods.GoodsLocation, _goods.CurrentDisplayGoodsData));
        GetButton((int)Buttons.ChangeGoodsButton).gameObject.SetActive(false);
    }
}

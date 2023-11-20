using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum GoodsLocation
{
    Left,
    Middle,
    Right,
}

public class UI_BattleShop : UI_Popup
{
    enum GameObjects
    {
        GoodsParent,
    }

    enum Buttons
    {
        ResetButton,
    }

    UnitUpgradeShopData _unitUpgradeShopData;
    protected override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        GetButton((int)Buttons.ResetButton).onClick.AddListener(BuyShopReset);

        _unitUpgradeShopData = Multi_GameManager.Instance.BattleData.UnitUpgradeShopData;
        InitGoods();
    }

    GoodsBuyController _buyController;
    BuyAction _buyAction;
    public void DependencyInject(GoodsBuyController buyController, BuyAction buyAction)
    {
        _buyController = buyController;
        _buyAction = buyAction;
    }

    GoodsManager<BattleShopGoodsData> _goodsManager;
    Dictionary<GoodsLocation, UI_BattleShopGoods> _goodsByLocation = new Dictionary<GoodsLocation, UI_BattleShopGoods>();
    void InitGoods()
    {
        _goodsManager = new GoodsManager<BattleShopGoodsData>(GenerateUnitUpgradeDatas());

        foreach (var goods in GetObject((int)GameObjects.GoodsParent).GetComponentsInChildren<UI_BattleShopGoods>())
        {
            goods.Inject(_buyController, _buyAction);
            goods.OnBuyGoods += DisplayGoods;
            goods.DisplayGoods(_goodsManager.GetRandomGoods());
            _goodsByLocation.Add(goods.GoodsLocation, goods);
        }
    }

    IEnumerable<BattleShopGoodsData> GenerateUnitUpgradeDatas()
    => Multi_GameManager.Instance.BattleData.ShopPriceDataByUnitUpgradeData.Select(x => CreateShopGoodsData(x.Key, x.Value));

    BattleShopGoodsData CreateShopGoodsData(UnitUpgradeData unitUpgradeData, CurrencyData priceData)
    {
        var datas = new float[] { (float)unitUpgradeData.UpgradeType, (float)unitUpgradeData.TargetColor, unitUpgradeData.Value };
        return new BattleShopGoodsData().Clone(
            name: UnitUpgradeGoodsPresenter.BuildGoodsText(unitUpgradeData),
            priceData,
            info: "를",
            new GoodsData().Clone(BattleShopGoodsType.UnitUpgrade, datas)
            );
    }

    void DisplayGoods(GoodsLocation location)
    {
        StartCoroutine(Co_DisplayGoods(_goodsByLocation[location], _goodsManager.ChangeGoods(_goodsByLocation[location].CurrentDisplayGoodsData)));
    }

    IEnumerator Co_DisplayGoods(UI_BattleShopGoods goods, BattleShopGoodsData newGoods)
    {
        goods.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        goods.DisplayGoods(newGoods);
        goods.gameObject.SetActive(true);
    }

    string ShopChangeText => $"{_unitUpgradeShopData.ResetPrice}골드를 지불하여 상점을 초기화하시겠습니까?";
    void BuyShopReset() => _buyController.TryBuy(ShopChangeText, new CurrencyData(GameCurrencyType.Gold, _unitUpgradeShopData.ResetPrice), ChangeAllGoods);
    void ChangeAllGoods()
    {
        var newGoodsList = _goodsManager.ChangeAllGoods().ToArray();
        var goodsList = GetComponentsInChildren<UI_BattleShopGoods>();
        for (int i = 0; i < newGoodsList.Length; i++)
            goodsList[i].DisplayGoods(newGoodsList[i]);
    }
}

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

    public bool IsInject { get; private set; } = false;
    GoodsBuyController _buyController;
    public void Inject(GoodsBuyController buyController)
    {
        _buyController = buyController;
        IsInject = true;
    }

    void InitGoods()
    {
        GoodsManager = new GoodsManager<BattleShopGoodsData>(new UnitUpgradeGoodsSelector().GetAllGoods().Select(x => CreateShopGoodsData(x)));
        foreach (var goods in GetObject((int)GameObjects.GoodsParent).GetComponentsInChildren<UI_BattleShopGoods>())
        {
            goods.Inject(_buyController);
            goods.OnBuyGoods += DisplayGoods;
            goods.DisplayGoods(GoodsManager.GetRandomGoods());
            _goodsByLocation.Add(goods.GoodsLocation, goods);
        }
    }
    Dictionary<GoodsLocation, UI_BattleShopGoods> _goodsByLocation = new Dictionary<GoodsLocation, UI_BattleShopGoods>();
    GoodsManager<BattleShopGoodsData> GoodsManager = new GoodsManager<BattleShopGoodsData>(new BattleShopGoodsData[] { });
    void DisplayGoods(GoodsLocation location)
    {
        StartCoroutine(Co_DisplayGoods(_goodsByLocation[location], GoodsManager.ChangeGoods(_goodsByLocation[location].CurrentDisplayGoodsData)));
    }

    IEnumerator Co_DisplayGoods(UI_BattleShopGoods goods, BattleShopGoodsData newGoods)
    {
        goods.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        goods.DisplayGoods(newGoods);
        goods.gameObject.SetActive(true);
    }

    readonly UnitUpgradeGoodsPresenter _goodsPresenter = new UnitUpgradeGoodsPresenter();
    BattleShopGoodsData CreateShopGoodsData(UnitUpgradeGoodsData unitUpgradeData)
    {
        var unitUpgradeGoodsData = CreateGoodsData(unitUpgradeData);
        var datas = new float[] { (float)unitUpgradeGoodsData.UpgradeType, (float)unitUpgradeGoodsData.TargetColor, unitUpgradeGoodsData.Value };
        return new BattleShopGoodsData().Clone(_goodsPresenter.BuildGoodsText(unitUpgradeGoodsData), unitUpgradeGoodsData.PriceData, "를", new GoodsData().Clone(BattleShopGoodsType.UnitUpgrade, datas));
    }

    UnitUpgradeData CreateGoodsData(UnitUpgradeGoodsData data)
    {
        if (data.UpgradeType == UnitUpgradeType.Value)
            return new UnitUpgradeData(data.UpgradeType, data.TargetColor, _unitUpgradeShopData.AddValue, _unitUpgradeShopData.AddValuePriceData);
        else
            return new UnitUpgradeData(data.UpgradeType, data.TargetColor, _unitUpgradeShopData.UpScale, _unitUpgradeShopData.UpScalePriceData);
    }

    string ShopChangeText => $"{_unitUpgradeShopData.ResetPrice}골드를 지불하여 상점을 초기화하시겠습니까?";
    void BuyShopReset() => _buyController.ShowBuyWindow(ShopChangeText, new CurrencyData(GameCurrencyType.Gold, _unitUpgradeShopData.ResetPrice), ChangeAllGoods);
    void ChangeAllGoods()
    {
        var newGoodsList = GoodsManager.ChangeAllGoods().ToArray();
        var goodsList = GetComponentsInChildren<UI_BattleShopGoods>();
        for (int i = 0; i < newGoodsList.Length; i++)
            goodsList[i].DisplayGoods(newGoodsList[i]);
    }
}

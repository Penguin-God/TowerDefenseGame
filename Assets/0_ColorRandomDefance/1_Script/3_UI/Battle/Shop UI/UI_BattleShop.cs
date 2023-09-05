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
    enum Buttons
    {
        ResetButton,
    }

    UnitUpgradeShopData _unitUpgradeShopData;
    GoodsManager<UnitUpgradeGoodsData> _goodsManager;
    protected override void Init()
    {
        base.Init();
        _unitUpgradeShopData = Multi_GameManager.Instance.BattleData.UnitUpgradeShopData;
        _goodsManager = new GoodsManager<UnitUpgradeGoodsData>(new UnitUpgradeGoodsSelector().GetAllGoods());
        InitGoods();
        // __InitGoods();

        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.ResetButton).onClick.AddListener(ResetShop);
    }

    public bool IsInject { get; private set; } = false;
    public void Inject(TextShowAndHideController textController)
    {
        _textController = textController;
        IsInject = true;
    }

    GoodsBuyController _buyController;
    public void Inject(GoodsBuyController buyController, TextShowAndHideController textController)
    {
        _buyController = buyController;
        _textController = textController;
        IsInject = true;
    }

    void InitGoods()
    {
        foreach (var goods in GetComponentsInChildren<UI_UnitUpgradeGoods>())
        {
            goods._Init(new UnitUpgradeShopController(_textController));
            goods.OnBuyGoods += DisplayGoods;
            goods.Setup(CreateGoodsData(_goodsManager.GetRandomGoods()));
        }
    }

    void __InitGoods()
    {
        GoodsManager = new GoodsManager<BattleShopGoodsData>(new UnitUpgradeGoodsSelector().GetAllGoods().Select(x => CreateShopGoodsData(x)));
        foreach (var goods in GetComponentsInChildren<UI_BattleShopGoods>())
        {
            goods.Inject(_buyController);
            goods.OnBuyGoods += DisplayGoods;
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

    void DisplayGoods(UI_UnitUpgradeGoods goods)
    {
        StartCoroutine(Co_DisplayGoods(goods, _goodsManager.ChangeGoods(new UnitUpgradeGoodsData(goods.UpgradeData.UpgradeType, goods.UpgradeData.TargetColor))));
    }

    IEnumerator Co_DisplayGoods(UI_UnitUpgradeGoods goods, UnitUpgradeGoodsData newGoods)
    {
        goods.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        goods.Setup(CreateGoodsData(newGoods));
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

    void ResetShop()
    {
        Managers.UI.ShowPopupUI<UI_ComfirmPopup>("UI_ComfirmPopup2").SetInfo($"{_unitUpgradeShopData.ResetPrice}골드를 지불하여 상점을 초기화하시겠습니까?", BuyShopReset);
        Managers.Sound.PlayEffect(EffectSoundType.ShopGoodsClick);
    }
    protected TextShowAndHideController _textController;
    void BuyShopReset()
    {
        if (Multi_GameManager.Instance.TryUseGold(_unitUpgradeShopData.ResetPrice))
        {
            var newGoodsList = _goodsManager.ChangeAllGoods().ToArray();
            var goodsList = GetComponentsInChildren<UI_UnitUpgradeGoods>();
            for (int i = 0; i < newGoodsList.Length; i++)
                goodsList[i].Setup(CreateGoodsData(newGoodsList[i]));
            Managers.Sound.PlayEffect(EffectSoundType.GoodsBuySound);
        }
        else
        {
            _textController.ShowTextForTime($"골드가 부족해 구매할 수 없습니다.", Color.red);
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public enum GoodsLocation
{
    Left,
    Middle,
    Right,
}

public class UI_UnitUpgradeShop : UI_Popup
{
    enum Buttons
    {
        ResetButton,
    }

    UnitUpgradeShopData _unitUpgradeShopData;
    Dictionary<GoodsLocation, UnitUpgradeGoodsData> _locationByGoods = new Dictionary<GoodsLocation, UnitUpgradeGoodsData>();
    readonly UnitUpgradeGoodsSelector _goodsSelector = new UnitUpgradeGoodsSelector();
    protected TextShowAndHideController _textController;
    protected override void Init()
    {
        base.Init();
        _unitUpgradeShopData = Multi_GameManager.Instance.BattleData.UnitUpgradeShopData;

        foreach (var goods in GetComponentsInChildren<UI_UnitUpgradeGoods>())
        {
            goods._Init(new UnitUpgradeShopController(_textController));
            goods.OnBuyGoods += DisplayGoods;
        }
        SetLocationByGoodsDatas(new HashSet<UnitUpgradeGoodsData>());
        
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.ResetButton).onClick.AddListener(ResetShop);
    }

    public bool IsInject { get; private set; } = false;
    public void Inject(TextShowAndHideController textController)
    {
        _textController = textController;
        IsInject = true;
    }

    void SetLocationByGoodsDatas(HashSet<UnitUpgradeGoodsData> excludingGoddsSet)
    {
        var locations = Enum.GetValues(typeof(GoodsLocation)).Cast<GoodsLocation>();
        var goodsSet = _goodsSelector.SelectGoodsSetExcluding(excludingGoddsSet);
        _locationByGoods = locations.Zip(goodsSet, (location, goods) => new { location, goods }).ToDictionary(pair => pair.location, pair => pair.goods);

        foreach (var item in _locationByGoods)
            GetComponentsInChildren<UI_UnitUpgradeGoods>().Where(x => x.GoodsLocation == item.Key).First().Setup(CreateGoodsData(item.Value));
    }

    UnitUpgradeData CreateGoodsData(UnitUpgradeGoodsData data)
    {
        if (data.UpgradeType == UnitUpgradeType.Value)
            return new UnitUpgradeData(data.UpgradeType, data.TargetColor, _unitUpgradeShopData.AddValue, _unitUpgradeShopData.AddValuePriceData);
        else
            return new UnitUpgradeData(data.UpgradeType, data.TargetColor, _unitUpgradeShopData.UpScale, _unitUpgradeShopData.UpScalePriceData);
    }

    void DisplayGoods(UI_UnitUpgradeGoods goods)
    {
        var newGoods = _goodsSelector.SelectGoodsExcluding(_locationByGoods.Where(x => x.Key != goods.GoodsLocation).Select(x => x.Value));
        StartCoroutine(Co_DisplayGoods(goods, newGoods));
    }

    IEnumerator Co_DisplayGoods(UI_UnitUpgradeGoods goods, UnitUpgradeGoodsData newGoods)
    {
        goods.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        ChangeGoods(goods, newGoods);
        goods.gameObject.SetActive(true);
    }

    void ChangeGoods(UI_UnitUpgradeGoods goods, UnitUpgradeGoodsData newGoods)
    {
        _locationByGoods[goods.GoodsLocation] = newGoods;
        goods.Setup(CreateGoodsData(newGoods));
    }

    void ResetShop()
    {
        Managers.UI.ShowPopupUI<UI_ComfirmPopup>("UI_ComfirmPopup2").SetInfo($"{_unitUpgradeShopData.ResetPrice}골드를 지불하여 상점을 초기화하시겠습니까?", BuyShopReset);
        Managers.Sound.PlayEffect(EffectSoundType.ShopGoodsClick);
    }
    void BuyShopReset()
    {
        if (Multi_GameManager.Instance.TryUseGold(_unitUpgradeShopData.ResetPrice))
        {
            SetLocationByGoodsDatas(new HashSet<UnitUpgradeGoodsData>(_locationByGoods.Values));
            Managers.Sound.PlayEffect(EffectSoundType.GoodsBuySound);
        }
        else
        {
            _textController.ShowTextForTime($"골드가 부족해 구매할 수 없습니다.", Color.red);
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
        }
    }
}

public class __UI_UnitUpgradeShop : UI_Popup
{
    enum Buttons
    {
        ResetButton,
    }

    UnitUpgradeShopData _unitUpgradeShopData;
    Dictionary<GoodsLocation, UnitUpgradeGoodsData> _locationByGoods = new Dictionary<GoodsLocation, UnitUpgradeGoodsData>();
    readonly UnitUpgradeGoodsSelector _goodsSelector = new UnitUpgradeGoodsSelector();
    protected TextShowAndHideController _textController;
    protected override void Init()
    {
        base.Init();
        _unitUpgradeShopData = Multi_GameManager.Instance.BattleData.UnitUpgradeShopData;

        foreach (var goods in GetComponentsInChildren<UI_UnitUpgradeGoods>())
        {
            goods._Init(new UnitUpgradeShopController(_textController));
            goods.OnBuyGoods += DisplayGoods;
        }
        SetLocationByGoodsDatas(new HashSet<UnitUpgradeGoodsData>());

        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.ResetButton).onClick.AddListener(ResetShop);
    }

    public bool IsInject { get; private set; } = false;
    public void Inject(TextShowAndHideController textController)
    {
        _textController = textController;
        IsInject = true;
    }

    void SetLocationByGoodsDatas(HashSet<UnitUpgradeGoodsData> excludingGoddsSet)
    {
        var locations = Enum.GetValues(typeof(GoodsLocation)).Cast<GoodsLocation>();
        var goodsSet = _goodsSelector.SelectGoodsSetExcluding(excludingGoddsSet);
        _locationByGoods = locations.Zip(goodsSet, (location, goods) => new { location, goods }).ToDictionary(pair => pair.location, pair => pair.goods);

        foreach (var item in _locationByGoods)
            GetComponentsInChildren<UI_UnitUpgradeGoods>().Where(x => x.GoodsLocation == item.Key).First().Setup(CreateGoodsData(item.Value));
    }

    UnitUpgradeData CreateGoodsData(UnitUpgradeGoodsData data)
    {
        if (data.UpgradeType == UnitUpgradeType.Value)
            return new UnitUpgradeData(data.UpgradeType, data.TargetColor, _unitUpgradeShopData.AddValue, _unitUpgradeShopData.AddValuePriceData);
        else
            return new UnitUpgradeData(data.UpgradeType, data.TargetColor, _unitUpgradeShopData.UpScale, _unitUpgradeShopData.UpScalePriceData);
    }

    void DisplayGoods(UI_UnitUpgradeGoods goods)
    {
        var newGoods = _goodsSelector.SelectGoodsExcluding(_locationByGoods.Where(x => x.Key != goods.GoodsLocation).Select(x => x.Value));
        StartCoroutine(Co_DisplayGoods(goods, newGoods));
    }

    IEnumerator Co_DisplayGoods(UI_UnitUpgradeGoods goods, UnitUpgradeGoodsData newGoods)
    {
        goods.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        ChangeGoods(goods, newGoods);
        goods.gameObject.SetActive(true);
    }

    void ChangeGoods(UI_UnitUpgradeGoods goods, UnitUpgradeGoodsData newGoods)
    {
        _locationByGoods[goods.GoodsLocation] = newGoods;
        goods.Setup(CreateGoodsData(newGoods));
    }

    void ResetShop()
    {
        Managers.UI.ShowPopupUI<UI_ComfirmPopup>("UI_ComfirmPopup2").SetInfo($"{_unitUpgradeShopData.ResetPrice}골드를 지불하여 상점을 초기화하시겠습니까?", BuyShopReset);
        Managers.Sound.PlayEffect(EffectSoundType.ShopGoodsClick);
    }
    void BuyShopReset()
    {
        if (Multi_GameManager.Instance.TryUseGold(_unitUpgradeShopData.ResetPrice))
        {
            SetLocationByGoodsDatas(new HashSet<UnitUpgradeGoodsData>(_locationByGoods.Values));
            Managers.Sound.PlayEffect(EffectSoundType.GoodsBuySound);
        }
        else
        {
            _textController.ShowTextForTime($"골드가 부족해 구매할 수 없습니다.", Color.red);
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
        }
    }
}

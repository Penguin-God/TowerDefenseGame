using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class UnitUpgradeShopController
{
    public event Action<UnitUpgradeGoodsData> OnBuyGoods;
    UnitUpgradeShopData _unitUpgradeShopData;

    public UnitUpgradeShopController(UnitUpgradeShopData unitUpgradeShopData) => _unitUpgradeShopData = unitUpgradeShopData;

    public void Buy(UnitUpgradeGoodsData upgradeData)
    {
        var priceData = Multi_GameManager.Instance.BattleData.ShopPriceDataByUnitUpgradeData[upgradeData];
        if (Multi_GameManager.Instance.TryUseCurrency(priceData.CurrencyType, priceData.Amount))
        {
            Managers.Sound.PlayEffect(EffectSoundType.GoodsBuySound);
            UpgradeUnit(upgradeData);
            Multi_GameManager.Instance.IncrementUnitUpgradeValue(upgradeData);
            OnBuyGoods?.Invoke(upgradeData);
        }
        else
        {
            Managers.UI.ShowDefualtUI<UI_PopupText>()
                .Show($"{new GameCurrencyPresenter().BuildCurrencyTypeText(priceData.CurrencyType)}가 부족해 구매할 수 없습니다.", 2f, Color.red);
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
        }
    }

    void UpgradeUnit(UnitUpgradeGoodsData goods)
    {
        switch (goods.UpgradeType)
        {
            case UnitUpgradeType.Value: 
                MultiServiceMidiator.UnitUpgrade.AddUnitDamageValue(goods.TargetColor, _unitUpgradeShopData.AddValue, UnitStatType.All); break;
            case UnitUpgradeType.Scale: 
                MultiServiceMidiator.UnitUpgrade.ScaleUnitDamageValue(goods.TargetColor, _unitUpgradeShopData.UpScaleApplyValue / 100f, UnitStatType.All); break;
        }
    }
}

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
    Dictionary<GoodsLocation, UI_Goods> _locationByGoods_UI = new Dictionary<GoodsLocation, UI_Goods>();
    Dictionary<GoodsLocation, UnitUpgradeGoodsData> _locationByGoods = new Dictionary<GoodsLocation, UnitUpgradeGoodsData>();
    readonly UnitUpgradeGoodsSelector _goodsSelector = new UnitUpgradeGoodsSelector();
    UnitUpgradeShopController _buyController;
    protected override void Init()
    {
        base.Init();
        _unitUpgradeShopData = Multi_GameManager.Instance.BattleData.UnitUpgradeShopData;
        _buyController = new UnitUpgradeShopController(_unitUpgradeShopData);

        InitShopGoodsList();
        _buyController.OnBuyGoods += OnBuyGoods;
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.ResetButton).onClick.AddListener(ResetShop);
    }

    void InitShopGoodsList()
    {
        GetComponentsInChildren<UI_Goods>().ToList().ForEach(x => x._Init());
        SetGoods(new HashSet<UnitUpgradeGoodsData>());
    }

    void SetGoods(HashSet<UnitUpgradeGoodsData> excludingGoddsSet)
    {
        var locations = Enum.GetValues(typeof(GoodsLocation)).Cast<GoodsLocation>();
        var goodsSet = _goodsSelector.SelectGoodsSetExcluding(excludingGoddsSet);
        _locationByGoods = locations.Zip(goodsSet, (location, goods) => new { location, goods }).ToDictionary(pair => pair.location, pair => pair.goods);

        var goodsUIs = GetComponentsInChildren<UI_Goods>();
        _locationByGoods_UI = locations.Zip(goodsUIs, (location, goodsUI) => new { location, goodsUI }).ToDictionary(pair => pair.location, pair => pair.goodsUI);
        foreach (var item in _locationByGoods)
            _locationByGoods_UI[item.Key].Setup(item.Value, _buyController, _unitUpgradeShopData);
    }

    void OnBuyGoods(UnitUpgradeGoodsData goods)
    {
        var buyLocation = _locationByGoods.First(x => x.Value.Equals(goods)).Key;
        var newGoods = _goodsSelector.SelectGoodsExcluding(_locationByGoods.Where(x => x.Key != buyLocation).Select(x => x.Value));
        StartCoroutine(Co_OnBuyGoods(buyLocation, newGoods));
    }

    IEnumerator Co_OnBuyGoods(GoodsLocation buyLocation, UnitUpgradeGoodsData newGoods)
    {
        _locationByGoods_UI[buyLocation].gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        ChangeGoods(buyLocation, newGoods);
        _locationByGoods_UI[buyLocation].gameObject.SetActive(true);
    }

    void ChangeGoods(GoodsLocation buyLocation, UnitUpgradeGoodsData newGoods)
    {
        _locationByGoods[buyLocation] = newGoods;
        _locationByGoods_UI[buyLocation].Setup(newGoods, _buyController, _unitUpgradeShopData);
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
            SetGoods(new HashSet<UnitUpgradeGoodsData>(_locationByGoods.Values));
            Managers.Sound.PlayEffect(EffectSoundType.GoodsBuySound);
        }
        else
        {
            Managers.UI.ShowDefualtUI<UI_PopupText>().Show($"골드가 부족해 구매할 수 없습니다.", 2f, Color.red);
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class BuyController
{
    public event Action<UnitUpgradeData> OnBuyGoods;

    public void Buy(UnitUpgradeGoodsData goodsData)
    {
        if (Multi_GameManager.Instance.TryUseCurrency(goodsData.Currency, goodsData.Price))
        {
            Managers.Sound.PlayEffect(EffectSoundType.GoodsBuySound);
            UpgradeUnit(goodsData.UpgradeGoods);
            OnBuyGoods?.Invoke(goodsData.UpgradeGoods);
        }
        else
        {
            Managers.UI.ShowDefualtUI<UI_PopupText>().Show($"{GetCurrcneyText(goodsData.Currency)}가 부족해 구매할 수 없습니다.", 2f, Color.red);
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
        }
    }

    void UpgradeUnit(UnitUpgradeData goods)
    {
        switch (goods.UpgradeType)
        {
            case UnitUpgradeType.Value: 
                MultiServiceMidiator.UnitUpgrade.AddUnitDamageValue(goods.TargetColor, UnitUpgradeGoodsData.ADD_DAMAGE, UnitStatType.All); break;
            case UnitUpgradeType.Scale: 
                MultiServiceMidiator.UnitUpgrade.ScaleUnitDamageValue(goods.TargetColor, UnitUpgradeGoodsData.SCALE_DAMAGE_RATE, UnitStatType.All); break;
        }
    }
    string GetCurrcneyText(GameCurrencyType type) => type == GameCurrencyType.Gold ? "골드" : "고기";
}

public struct UnitUpgradeGoodsData
{
    public static int ADD_DAMAGE => 50;
    public static float SCALE_DAMAGE_RATE => 0.1f;
    public static int VALUE_PRICE => 10;
    public static int SCALE_PRICE => 1;

    readonly public UnitUpgradeData UpgradeGoods;
    public UnitUpgradeGoodsData(UnitUpgradeData upgradeGoods) => UpgradeGoods = upgradeGoods;

    public int Price => UpgradeGoods.UpgradeType == UnitUpgradeType.Value ? VALUE_PRICE : SCALE_PRICE;
    public GameCurrencyType Currency => UpgradeGoods.UpgradeType == UnitUpgradeType.Value ? GameCurrencyType.Gold : GameCurrencyType.Food;
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

    Dictionary<GoodsLocation, UI_Goods> _locationByGoods_UI = new Dictionary<GoodsLocation, UI_Goods>();
    Dictionary<GoodsLocation, UnitUpgradeData> _locationByGoods = new Dictionary<GoodsLocation, UnitUpgradeData>();
    readonly UnitUpgradeDataSelector _goodsSelector = new UnitUpgradeDataSelector();
    readonly BuyController _buyController = new BuyController();
    protected override void Init()
    {
        base.Init();
        InitShopGoodsList();
        _buyController.OnBuyGoods += OnBuyGoods;
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.ResetButton).onClick.AddListener(ResetShop);
        gameObject.SetActive(false);
    }

    void InitShopGoodsList()
    {
        GetComponentsInChildren<UI_Goods>().ToList().ForEach(x => x._Init());
        SetGoods(new HashSet<UnitUpgradeData>());
    }

    void SetGoods(HashSet<UnitUpgradeData> excludingGoddsSet)
    {
        var locations = Enum.GetValues(typeof(GoodsLocation)).Cast<GoodsLocation>();
        var goodsSet = _goodsSelector.SelectGoodsSetExcluding(excludingGoddsSet);
        _locationByGoods = locations.Zip(goodsSet, (location, goods) => new { location, goods }).ToDictionary(pair => pair.location, pair => pair.goods);

        var goodsUIs = GetComponentsInChildren<UI_Goods>();
        _locationByGoods_UI = locations.Zip(goodsUIs, (location, goodsUI) => new { location, goodsUI }).ToDictionary(pair => pair.location, pair => pair.goodsUI);
        foreach (var item in _locationByGoods)
            _locationByGoods_UI[item.Key].Setup(item.Value, _buyController);
    }

    void OnBuyGoods(UnitUpgradeData goods)
    {
        var changeLocation = _locationByGoods.First(x => x.Value.Equals(goods)).Key;
        var newGoods = _goodsSelector.SelectGoodsExcluding(_locationByGoods.Where(x => x.Key != changeLocation).Select(x => x.Value));
        _locationByGoods[changeLocation] = newGoods;
        _locationByGoods_UI[changeLocation].Setup(newGoods, _buyController);
    }

    const int RESET_PRICE = 5;
    void ResetShop()
    {
        Managers.UI.ShowPopupUI<UI_ComfirmPopup>("UI_ComfirmPopup2").SetInfo($"{RESET_PRICE}골드를 지불하여 상점을 초기화하시겠습니까?", BuyShopReset);
        Managers.Sound.PlayEffect(EffectSoundType.ShopGoodsClick);
    }
    void BuyShopReset()
    {
        if (Multi_GameManager.Instance.TryUseGold(RESET_PRICE))
        {
            SetGoods(new HashSet<UnitUpgradeData>(_locationByGoods.Values));
            Managers.Sound.PlayEffect(EffectSoundType.GoodsBuySound);
        }
        else
        {
            Managers.UI.ShowDefualtUI<UI_PopupText>().Show($"골드가 부족해 구매할 수 없습니다.", 2f, Color.red);
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
        }
    }
}

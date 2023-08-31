using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public struct GoodsData
{
    [SerializeField] BattleShopGoodsType _goodsType;
    [SerializeField] float[] _datas;

    public BattleShopGoodsType GoodsType => _goodsType;
    public float[] Datas => _datas;
}

public class UI_BattleShopGoods : UI_Base
{
    enum Texts
    {
        ProductNameText,
        PriceText,
    }

    enum Images
    {
        CurrencyImage,
    }

    enum Buttons
    {
        PanelButton,
    }

    protected override void Init()
    {
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        // Bind<Button>(typeof(Buttons));
        _panelButton = GetComponent<Button>();
    }

    GoodsBuyController _goodsBuyController;
    [SerializeField] GoodsLocation _goodsLocation;
    public Action<GoodsLocation> OnBuyGoods;
    public Action<BattleShopGoodsData> _OnBuyGoods;
    public void Inject(GoodsBuyController goodsBuyController)
    {
        _goodsBuyController = goodsBuyController;
    }

    Button _panelButton;
    internal void DisplayGoods(BattleShopGoodsData goodsData)
    {
        CheckInit();

        GetText((int)Texts.ProductNameText).text = goodsData.Name;
        GetText((int)Texts.PriceText).text = goodsData.PriceData.Amount.ToString();
        // GetImage((int)Images.CurrencyImage).sprite = CurrencyToSprite(priceData.CurrencyType);

        _panelButton.onClick.RemoveAllListeners();
        _panelButton.onClick.AddListener(() => ShowBuyWindow(goodsData));
    }

    void ShowBuyWindow(BattleShopGoodsData goodsData)
    {
        string qustionText = "사쉴?";
        Managers.UI.ShowPopupUI<UI_ComfirmPopup>("UI_ComfirmPopup2").SetInfo(qustionText, () => Buy(goodsData));
    }

    void Buy(BattleShopGoodsData goodsData)
    {
        if (_goodsBuyController.TryBuy(goodsData))
        {
            OnBuyGoods?.Invoke(_goodsLocation);
            _OnBuyGoods?.Invoke(goodsData);
        }
    }
}

public class GoodsBuyController
{
    readonly Multi_GameManager _gameManager;
    readonly BuyActionFactory _buyActionFactory;
    readonly TextShowAndHideController _textController;

    public GoodsBuyController(Multi_GameManager gameManager, BuyActionFactory buyActionFactory, TextShowAndHideController textController)
    {
        _gameManager = gameManager;
        _buyActionFactory = buyActionFactory;
        _textController = textController;
    }

    public bool TryBuy(BattleShopGoodsData data)
    {
        if (_gameManager.TryUseCurrency(data.PriceData))
        {
            Managers.Sound.PlayEffect(EffectSoundType.GoodsBuySound);
            _buyActionFactory.CreateBuyAction(data.SellData).Invoke();
            return true;
        }
        else
        {
            _textController.ShowTextForTime($"{new GameCurrencyPresenter().BuildCurrencyTypeText(data.PriceData.CurrencyType)}이 부족해 구매할 수 없습니다.", Color.red);
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
            return false;
        }
    }
}

public class BuyActionFactory
{
    readonly Multi_NormalUnitSpawner _unitSpawner;
    readonly UnitUpgradeController _unitUpgradeController;
    public BuyActionFactory(Multi_NormalUnitSpawner unitSpawner, UnitUpgradeController unitUpgradeController)
    {
        _unitSpawner = unitSpawner;
        _unitUpgradeController = unitUpgradeController;
    }

    public UnityAction CreateBuyAction(GoodsData sellData)
    {
        var convertor = new DataConvertUtili();
        switch (sellData.GoodsType)
        {
            case BattleShopGoodsType.Unit: return () => SpawnUnit(convertor.ToUnitFlag(sellData.Datas));
            case BattleShopGoodsType.UnitUpgrade: return () => UpgradeUnit(convertor.ToUnitUpgradeData(sellData.Datas));
            default: Debug.LogError($"정의되지 않은 굿즈 타입 {sellData.GoodsType}"); return null;
        }
    }

    void SpawnUnit(UnitFlags flag) => _unitSpawner.Spawn(flag);

    void UpgradeUnit(UnitUpgradeData goods)
    {
        switch (goods.UpgradeType)
        {
            case UnitUpgradeType.Value:
                _unitUpgradeController.AddUnitDamageValue(goods.TargetColor, goods.Value, UnitStatType.All); break;
            case UnitUpgradeType.Scale:
                _unitUpgradeController.ScaleUnitDamageValue(goods.TargetColor, goods.Value / 100f, UnitStatType.All);
                Multi_GameManager.Instance.IncrementUnitUpgradeValue(goods);
                break;
        }
    }
}

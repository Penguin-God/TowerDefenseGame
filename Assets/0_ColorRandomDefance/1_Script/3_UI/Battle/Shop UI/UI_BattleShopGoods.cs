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
        ColorPanel,
    }

    enum Buttons
    {
        PanelButton,
        ChangeGoodsButton,
    }

    protected override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.ChangeGoodsButton).onClick.AddListener(() => _changeGoods.Invoke(_goodsLocation, _currentDisplayGoodsData));
    }

    GoodsBuyController _goodsBuyController;
    [SerializeField] GoodsLocation _goodsLocation;
    public GoodsLocation GoodsLocation => _goodsLocation;
    public event Action<GoodsLocation> OnBuyGoods;
    Action<GoodsLocation, BattleShopGoodsData> _changeGoods;
    BattleShopGoodsData _currentDisplayGoodsData;
    public void Inject(GoodsBuyController goodsBuyController, Action<GoodsLocation, BattleShopGoodsData> changeGoods)
    {
        _goodsBuyController = goodsBuyController;
        _changeGoods = changeGoods;
    }

    internal void DecorateGoods(BattleShopGoodsData goodsData)
    {
        CheckInit();
        _currentDisplayGoodsData = goodsData;

        GetTextMeshPro((int)Texts.ProductNameText).text = goodsData.Name;
        GetTextMeshPro((int)Texts.PriceText).text = goodsData.PriceData.Amount.ToString();
        
        GetImage((int)Images.CurrencyImage).sprite = new SpriteUtility().GetBattleCurrencyImage(goodsData.PriceData.CurrencyType);
        SetPanelColor(goodsData.SellData);

        GetButton((int)Buttons.PanelButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.PanelButton).onClick.AddListener(() => ShowBuyWindow(goodsData));
    }

    void SetPanelColor(GoodsData goodsData)
    {
        UnitColor unitColor = UnitColor.Black;
        var convertor = new DataConvertUtili();
        switch (goodsData.GoodsType)
        {
            case BattleShopGoodsType.Unit:
                unitColor = convertor.ToUnitFlag(goodsData.Datas).UnitColor; break;
            case BattleShopGoodsType.UnitUpgrade:
                unitColor = convertor.ToUnitUpgradeData(goodsData.Datas).TargetColor; break;
        }
        GetImage((int)Images.ColorPanel).color = new SpriteUtility().GetUnitColor(unitColor);
    }

    void ShowBuyWindow(BattleShopGoodsData goodsData)
    {
        string qustionText = $"{goodsData.Name}{goodsData.InfoText} 구매하시겠습니까?";
        Managers.UI.ShowPopupUI<UI_ComfirmPopup>("UI_ComfirmPopup2").SetInfo(qustionText, () => Buy(goodsData));
    }

    void Buy(BattleShopGoodsData goodsData)
    {
        if (_goodsBuyController.TryBuy(goodsData))
        {
            OnBuyGoods?.Invoke(_goodsLocation);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using Codice.Client.Commands;

public class GoodsManager
{
    const int maxGrade = 3;
    Dictionary<GoodsLocation, List<UI_RandomShopGoodsData>[]> _goodsData;
    Dictionary<GoodsLocation, UI_RandomShopGoodsData> _locationByData = new Dictionary<GoodsLocation, UI_RandomShopGoodsData>();
    public IReadOnlyDictionary<GoodsLocation, UI_RandomShopGoodsData> LocationByData => _locationByData;

    public event Action<UI_RandomShopGoodsData> OnDropGoods = null;

    public bool HasGoods() => _locationByData.Count() > 0;
    public GoodsManager()
    {
        _goodsData = GeneratedGoodsData();
    }

    Dictionary<GoodsLocation, List<UI_RandomShopGoodsData>[]> GeneratedGoodsData()
    {
        var goodsData = new Dictionary<GoodsLocation, List<UI_RandomShopGoodsData>[]>();
        foreach (var data in Managers.Data.RandomShopDatas)
        {
            if (goodsData.ContainsKey(data.GoodsLocation) == false)
            {
                goodsData.Add(data.GoodsLocation, new List<UI_RandomShopGoodsData>[maxGrade]);
                for (int i = 0; i < goodsData[data.GoodsLocation].Length; i++)
                    goodsData[data.GoodsLocation][i] = new List<UI_RandomShopGoodsData>();
            }
                
            goodsData[data.GoodsLocation][data.Grade].Add(data);
        }

        return goodsData;
    }

    public void DropGoods(UI_RandomShopGoodsData data)
    {
        if (_locationByData.ContainsKey(data.GoodsLocation) == false) return;

        _locationByData.Remove(data.GoodsLocation);
        OnDropGoods?.Invoke(data);
    }

    public void BindGoods()
    {
        _locationByData.Clear();
        UI_RandomShopGoodsData[] datas = new RandomShopGenerater().GetRandomGoodsArr(_goodsData);
        for (int i = 0; i < datas.Length; i++)
            _locationByData.Add(datas[i].GoodsLocation, datas[i]);
    }

    class RandomShopGenerater
    {
        GoodsLocation[] GoodsLocations = { GoodsLocation.Left, GoodsLocation.Middle, GoodsLocation.Right};
        const int maxGrade = 3;

        public UI_RandomShopGoodsData[] GetRandomGoodsArr(Dictionary<GoodsLocation, List<UI_RandomShopGoodsData>[]> currentAllData)
        {
            List<UI_RandomShopGoodsData> result = new List<UI_RandomShopGoodsData>();

            foreach (var location in GoodsLocations)
            {
                List<UI_RandomShopGoodsData> datas = currentAllData[location][GetGrade(location, new int[] { 33, 33, 34 })];
                if (datas.Count > 0)
                    result.Add(datas[Random.Range(0, datas.Count)]);
            }

            return result.ToArray();

            int GetGrade(GoodsLocation location, int[] weigths)
            {
                var gradeByWeith = GeneratedGradeByWeigth(location, weigths);
                int randomNumber = Random.Range(0, gradeByWeith.Values.Sum());

                foreach (var item in gradeByWeith)
                {
                    if (item.Value >= randomNumber) return item.Key;
                    else randomNumber -= item.Value;
                }

                //Debug.LogError("확률 잘못 설정함");
                return 0;
            }

            Dictionary<int, int> GeneratedGradeByWeigth(GoodsLocation location, int[] weigths)
            {
                var resultDict = new Dictionary<int, int>();
                for (int i = 0; i < maxGrade; i++)
                {
                    if (ContainsGoods(location, i))
                        resultDict.Add(i, weigths[i]);
                }
                return resultDict;
            }

            bool ContainsGoods(GoodsLocation location, int grade) => currentAllData[location][grade].Count > 0;
        }
    }
}

public class BuyController
{
    public event Action<UnitUpgradeGoods> OnBuyGoods;

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

    void UpgradeUnit(UnitUpgradeGoods goods)
    {
        switch (goods.UpgradeType)
        {
            case UnitUpgradeType.Value: MultiServiceMidiator.Game.AddUnitDamageValue(goods.TargetColor, 50, UnitStatType.All); break;
            case UnitUpgradeType.Scale: MultiServiceMidiator.Game.ScaleUnitDamageValue(goods.TargetColor, 50, UnitStatType.All); break;
        }
    }
    string GetCurrcneyText(GameCurrencyType type) => type == GameCurrencyType.Gold ? "골드" : "고기";
}

public struct UnitUpgradeGoodsData
{
    readonly public UnitUpgradeGoods UpgradeGoods;
    public UnitUpgradeGoodsData(UnitUpgradeGoods upgradeGoods) => UpgradeGoods = upgradeGoods;

    public int Price => UpgradeGoods.UpgradeType == UnitUpgradeType.Value ? 10 : 1;
    public GameCurrencyType Currency => UpgradeGoods.UpgradeType == UnitUpgradeType.Value ? GameCurrencyType.Gold : GameCurrencyType.Food;
}

public class RandomShop_UI : UI_Popup
{
    enum Buttons
    {
        ResetButton,
    }

    Dictionary<GoodsLocation, UI_Goods> _locationByGoods_UI = new Dictionary<GoodsLocation, UI_Goods>();
    Dictionary<GoodsLocation, UnitUpgradeGoods> _locationByGoods = new Dictionary<GoodsLocation, UnitUpgradeGoods>();
    readonly UnitUpgradeGoodsSelector _goodsSelector = new UnitUpgradeGoodsSelector();

    GoodsManager goodsManager;
    readonly BuyController _buyController = new BuyController();
    [SerializeField] UI_RandomShopPanel panel;
    protected override void Init()
    {
        base.Init();
        //goodsManager = new GoodsManager();

        foreach (var item in GetComponentsInChildren<UI_Goods>())
        {
            item._Init();
            _locationByGoods_UI.Add(item.Loaction, item);
        }

        _buyController.OnBuyGoods += OnBuyGoods;

        //goodsManager.OnDropGoods += HideGoods;
        //goodsManager.OnDropGoods += UpdateShop;

        //Bind<Button>(typeof(Buttons));
        //GetButton((int)Buttons.ResetButton).onClick.AddListener(ShopReset);
        BindGoods();
        gameObject.SetActive(false);
    }

    void BindGoods()
    {
        //goodsManager.BindGoods();
        //foreach (var item in goodsManager.LocationByData.Values)
        //    _locationByGoods[item.GoodsLocation].Setup(item, OnClickGoods);

        var locations = Enum.GetValues(typeof(GoodsLocation)).Cast<GoodsLocation>().Skip(1);
        var goodsSet = _goodsSelector.SelectGoodsSet();
        _locationByGoods = locations.Zip(goodsSet, (location, goods) => new { location, goods }).ToDictionary(pair => pair.location, pair => pair.goods);
        foreach (var item in _locationByGoods)
            _locationByGoods_UI[item.Key].Setup(item.Value, _buyController);
    }

    void OnBuyGoods(UnitUpgradeGoods goods)
    {
        var changeLocation = _locationByGoods.First(x => x.Value.Equals(goods)).Key;
        var newGoods = _goodsSelector.SelectGoodsExcluding(_locationByGoods.Where(x => x.Key != changeLocation).Select(x => x.Value));
        _locationByGoods[changeLocation] = newGoods;
        _locationByGoods_UI[changeLocation].Setup(newGoods, _buyController);
    }

    void HideGoods(UI_RandomShopGoodsData data) => _locationByGoods_UI[data.GoodsLocation].gameObject.SetActive(false);
    void UpdateShop(UI_RandomShopGoodsData data)
    {
        if (goodsManager.HasGoods() == false)
            BindGoods();
    }

    // 리셋 버튼에서 사용하는 함수
    void ShopReset()
    {
        UI_RandomShopGoodsData data =
            new UI_RandomShopGoodsData("상점 리롤", GoodsLocation.None, -1, GameCurrencyType.Gold, 10, 
            "10골드를 지불하여 상점을 돌리시겠습니까?", SellType.None, null);
        panel.Setup(data, goodsManager, BindGoods);
        Managers.Sound.PlayEffect(EffectSoundType.ShopGoodsClick);
    }
}

class GoodsSellUseCase
{
    public bool TrySell(UI_RandomShopGoodsData data, GoodsManager goodsManager, Action SellAct = null)
    {
        if (Multi_GameManager.Instance.TryUseCurrency(data.CurrencyType, data.Price))
        {
            goodsManager.DropGoods(data);

            GiveGoods(data, SellAct);
            Managers.Sound.PlayEffect(EffectSoundType.GoodsBuySound);
            return true;
        }
        return false;
    }

    void GiveGoods(UI_RandomShopGoodsData data, Action SellAct = null)
    {
        if (SellAct == null)
            new SellMethodFactory().GetSellMeghod(data.SellType)?.Invoke(data.SellDatas);
        else
            SellAct?.Invoke();
    }
}

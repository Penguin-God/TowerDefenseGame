using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class GoodsManager
{
    const int maxGrade = 3;
    Dictionary<GoodsLocation, List<UI_RandomShopGoodsData>[]> _goodsData;
    Dictionary<GoodsLocation, UI_RandomShopGoodsData> _locationByData = new Dictionary<GoodsLocation, UI_RandomShopGoodsData>();
    Dictionary<GoodsLocation, Goods_UI> _locationByGoods = new Dictionary<GoodsLocation, Goods_UI>();

    public IReadOnlyDictionary<GoodsLocation, UI_RandomShopGoodsData> LocationByData => _locationByData;

    public bool HasGoods(UI_RandomShopGoodsData data) => _goodsData[data.GoodsLocation][data.Grade].Count() > 0;
    public GoodsManager(Transform root)
    {

        foreach (var item in root.GetComponentsInChildren<Goods_UI>())
        {
            item._Init();
            _locationByGoods.Add(item.Loaction, item);
        }
        _goodsData = GeneratedGoodsData();
    }

    Dictionary<GoodsLocation, List<UI_RandomShopGoodsData>[]> GeneratedGoodsData()
    {
        var goodsData = new Dictionary<GoodsLocation, List<UI_RandomShopGoodsData>[]>();
        foreach (var data in Multi_Managers.Data.RandomShopDatas)
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

    public void RemoveGoods(UI_RandomShopGoodsData data)
    {
        _locationByData.Remove(data.GoodsLocation);
        _goodsData[data.GoodsLocation][data.Grade].Remove(data);
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
        const int goodsTypeCount = 3;
        const int maxGrade = 3;

        public UI_RandomShopGoodsData[] GetRandomGoodsArr(Dictionary<GoodsLocation, List<UI_RandomShopGoodsData>[]> currentAllData)
        {
            List<UI_RandomShopGoodsData> result = new List<UI_RandomShopGoodsData>();

            for (int i = 0; i < goodsTypeCount; i++)
            {
                List<UI_RandomShopGoodsData> datas = currentAllData[(GoodsLocation)i][GetGrade((GoodsLocation)i, new int[] { 33, 33, 34 })];
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

                Debug.LogError("확률 잘못 설정함");
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



public class RandomShop_UI : Multi_UI_Popup
{
    enum Buttons
    {
        ResetButton,
    }

    Dictionary<GoodsLocation, Goods_UI> _locationByGoods = new Dictionary<GoodsLocation, Goods_UI>();

    GoodsManager goodsManager;
    [SerializeField] RandomShopPanel_UI panel;
    protected override void Init()
    {
        base.Init();
        goodsManager = new GoodsManager(transform);

        foreach (var item in GetComponentsInChildren<Goods_UI>())
        {
            item._Init();
            _locationByGoods.Add(item.Loaction, item);
        }

        Bind<Button>(typeof(Buttons));
        BindGoods();
        gameObject.SetActive(false);
    }

    void BindGoods()
    {
        goodsManager.BindGoods();
        foreach (var item in goodsManager.LocationByData.Values)
            _locationByGoods[item.GoodsLocation].Setup(item, OnClickGoods);
    }

    void OnClickGoods(Goods_UI goods) 
        => panel.Setup(goodsManager.LocationByData[goods.Loaction], goodsManager, () => goods.gameObject.SetActive(false));


    // TODO : 상점 데이터 enum으로 바꾸고 리롤도 리팩터링하기
    void SetupResetButton()
    {
        //Goods_UI goods = GetButton((int)Buttons.ResetButton).gameObject.AddComponent<Goods_UI>();
        //goods.Setup(new UI_RandomShopGoodsData(), );
    }

    // 리셋 버튼에서 사용하는 함수
    public void ShopReset()
    {
        panel.Setup(BindGoods, 10, "Gold", "10골드를 지불하여 상점을 돌리시겠습니까?");
        Multi_Managers.Sound.PlayEffect(EffectSoundType.ShopGoodsClick);
    }
}

class GoodsSellUseCase
{
    public bool TrySell(UI_RandomShopGoodsData data, GoodsManager goodsManager, Action SellAct = null)
    {
        if (Multi_GameManager.instance.TryUseCurrency(data.CurrencyType, data.Price))
        {
            goodsManager.RemoveGoods(data);
            if (goodsManager.HasGoods(data) == false) goodsManager.BindGoods();

            GiveGoods(data, SellAct);
            Multi_Managers.Sound.PlayEffect(EffectSoundType.GoodsBuySound);
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

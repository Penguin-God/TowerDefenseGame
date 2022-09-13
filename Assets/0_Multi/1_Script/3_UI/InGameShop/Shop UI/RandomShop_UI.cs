﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using Random = UnityEngine.Random;

class GoodsManager
{
    const int goodsTypeCount = 3;
    const int maxGrade = 3;
    Dictionary<GoodsLocation, List<UI_RandomShopGoodsData>[]> _goodsData;
    public GoodsManager()
    {
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
    
    public UI_RandomShopGoodsData[] GetRandomGoods()
    {
        UI_RandomShopGoodsData[] result = new UI_RandomShopGoodsData[goodsTypeCount];

        for (int i = 0; i < result.Length; i++)
        {
            List<UI_RandomShopGoodsData> datas = _goodsData[(GoodsLocation)i][GetGrade((GoodsLocation)i, new int[] { 33, 33, 34 })];
            result[i] = datas[Random.Range(0, datas.Count)];
        }

        return result;
    }

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
        var result = new Dictionary<int, int>();
        for (int i = 0; i < maxGrade; i++)
        {
            if (ContainsGoods(location, i))
                result.Add(i, weigths[i]);
        }
        return result;
    }

    bool ContainsGoods(GoodsLocation location, int grade) => _goodsData[location][grade].Count > 0;
}

public class RandomShop_UI : Multi_UI_Popup
{
    enum Buttons
    {
        ResetButton,
    }

    GoodsManager goodsManager;
    List<Goods_UI> currentGoodsList = new List<Goods_UI>();
    [SerializeField] Transform goodsParent;
    [SerializeField] RandomShopPanel_UI panel;
    protected override void Init()
    {
        base.Init();
        goodsManager = new GoodsManager();
        for (int i = 0; i < goodsParent.childCount; i++)
            goodsParent.GetChild(i).GetComponent<Goods_UI>()._Init();
        Bind<Button>(typeof(Buttons));
        BindGoods();
        panel.OnSell += UpdateGoodsList;
        gameObject.SetActive(false);
    }

    void BindGoods()
    {
        currentGoodsList.Clear();
        UI_RandomShopGoodsData[] datas = goodsManager.GetRandomGoods();
        for (int i = 0; i < goodsParent.childCount; i++)
        {
            Goods_UI goods = goodsParent.GetChild(i).GetComponent<Goods_UI>();
            goods.Setup(datas[i], panel);
            currentGoodsList.Add(goods);
        }
    }

    void UpdateGoodsList(Goods_UI goods)
    {
        currentGoodsList.Remove(goods);
        if (currentGoodsList.Count == 0)
            BindGoods();
    }


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

    class SellUseCase
    {
        public bool TrySell(UI_RandomShopGoodsData data, Action SellAct = null)
        {
            if (Multi_GameManager.instance.TryUseCurrency(data.CurrencyType, data.Price))
            {
                if(SellAct == null)
                    new SellMethodFactory().GetSellMeghod(data.SellType)?.Invoke(data.SellDatas);
                else
                    SellAct?.Invoke();
                Multi_Managers.Sound.PlayEffect(EffectSoundType.GoodsBuySound);
                return true;
            }
            return false;
        }
    }
}

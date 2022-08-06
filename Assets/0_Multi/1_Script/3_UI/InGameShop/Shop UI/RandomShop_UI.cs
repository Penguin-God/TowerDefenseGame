using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class GoodsManager
{
    const int goodsTypeCount = 3;
    const int maxGrade = 3;
    List<UI_RandomShopGoodsData>[,] _goodsDatas;
    public GoodsManager(string dataPath)
    {
        string csv = Multi_Managers.Resources.Load<TextAsset>(dataPath).text;

        Setup();
        foreach (var data in CsvUtility.GetEnumerableFromCsv<UI_RandomShopGoodsData>(csv))
            _goodsDatas[data.GoodsType, data.Grade].Add(data);
    }

    void Setup()
    {
        _goodsDatas = null;
        _goodsDatas = new List<UI_RandomShopGoodsData>[goodsTypeCount, maxGrade];

        for (int i = 0; i < _goodsDatas.GetLength(0); i++)
        {
            for (int j = 0; j < _goodsDatas.GetLength(1); j++)
                _goodsDatas[i, j] = new List<UI_RandomShopGoodsData>();
        }
    }

    
    public UI_RandomShopGoodsData[] GetRandomGoods()
    {
        UI_RandomShopGoodsData[] result = new UI_RandomShopGoodsData[goodsTypeCount];

        for (int i = 0; i < result.Length; i++)
        {
            List<UI_RandomShopGoodsData> datas = _goodsDatas[i, GetGrade(new int[] { 33, 33, 34 })];
            result[i] = datas[Random.Range(0, datas.Count)];
        }

        return result;
    }

    int GetGrade(int[] weigths)
    {
        int totalWeigh = 100;
        int randomNumber = Random.Range(0, totalWeigh);

        for (int i = 0; i < maxGrade; i++) // 레벨 가중치에 따라 상품 등급 정함
        {
            if (weigths[i] >= randomNumber) return i;
            else randomNumber -= weigths[i];
        }

        Debug.LogError("확률 잘못 설정함");
        return 0;
    }
}

public class RandomShop_UI : Multi_UI_Popup
{
    GoodsManager goodsManager;
    List<Goods_UI> currentGoodsList = new List<Goods_UI>();
    [SerializeField] string dataPath;
    [SerializeField] Transform goodsParent;
    [SerializeField] RandomShopPanel_UI panel;
    protected override void Init()
    {
        base.Init();
        goodsManager = new GoodsManager(dataPath);
        for (int i = 0; i < goodsParent.childCount; i++)
            goodsParent.GetChild(i).GetComponent<Goods_UI>()._Init();

        BindGoods();
        panel.OnSell += UpdateGoodsList;
        gameObject.SetActive(false);
    }

    void BindGoods()
    {
        UI_RandomShopGoodsData[] datas = goodsManager.GetRandomGoods();
        currentGoodsList.Clear();
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

    // 리셋 버튼에서 사용하는 함수
    public void ShopReset()
    {
        panel.Setup(BindGoods, 10, "Gold", "10골드를 지불하여 상점을 돌리시겠습니까?");
    }
}

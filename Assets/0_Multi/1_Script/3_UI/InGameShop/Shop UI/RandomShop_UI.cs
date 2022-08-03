using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] string dataPath;
    [SerializeField] GoodsManager goodsManager;
    [SerializeField] Transform goodsParent;
    protected override void Init()
    {
        base.Init();
        goodsManager = new GoodsManager(dataPath);
        gameObject.SetActive(false);
    }

    [ContextMenu("Show")]
    public void Show()
    {
        UI_RandomShopGoodsData[] datas = goodsManager.GetRandomGoods();
        for (int i = 0; i < goodsParent.childCount; i++)
            goodsParent.GetChild(i).GetComponent<Goods_UI>().Setup(datas[i]);
    }

    [ContextMenu("test")]
    void Test()
    {
        for (int i = 0; i < 100; i++)
        {
            foreach (var item in goodsManager.GetRandomGoods())
            {
                print($"Type : {item.GoodsType}, Grade : {item.Grade}, Name : {item.Name}");
            }
        }
    }
}

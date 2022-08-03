using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
}

public class RandomShop_UI : Multi_UI_Popup
{
    [SerializeField] string dataPath;
    GoodsManager goodsManager;
    protected override void Init()
    {
        base.Init();
        goodsManager = new GoodsManager(dataPath);
    }
}

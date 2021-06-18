using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCurrentUnitShop : MonoBehaviour
{
    public enum GoodsType { mageUltimate };
    public GoodsType goodsType;

    private void OnEnable()
    {
        SetMageUltimateGoods();
    }

    void SetMageUltimateGoods()
    {
        List<int> mageUltimateGoodsList = EventManager.instance.Return_CurrentUnitColorList(3);

        if(mageUltimateGoodsList.Count == 0)
        {
            SetRandomGoods();
            return;
        }

        int listIndex = Random.Range(0, mageUltimateGoodsList.Count);
        int GoodsIndex = mageUltimateGoodsList[listIndex];
        transform.GetChild(GoodsIndex).gameObject.SetActive(true);
    }

    void SetRandomGoods()
    {
        int random = Random.Range(0, transform.childCount);
        transform.GetChild(random).gameObject.SetActive(true);
    }
}

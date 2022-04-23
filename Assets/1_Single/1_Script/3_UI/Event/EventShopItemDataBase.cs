using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventShopItemData
{
    public EventShopItemData(string _name, int _price, string _type, string _info)
    {
        name = _name; price = _price; type = _type; info = _info; 
    }

    public string name;
    public int price;
    public string type;
    public string info;
}

public class EventShopItemDataBase : MonoBehaviour
{
    [SerializeField] TextAsset goodsTextData;
    public List<EventShopItemData> itemDatas;

    [ContextMenu("SetGoodsList")]
    public void SetGoodsDataList()
    {
        string[] line = goodsTextData.text.Split('\n');
        for(int i = 1; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            EventShopItemData itemData = new EventShopItemData(row[0], int.Parse(row[1]), row[2], row[3]); 
            itemDatas.Add(itemData);
        }
    }

    public SellEventShopItem[] items;

    [ContextMenu("SetGoodsName")]
    public void SetGoodsName()
    {
        for(int i = 0; i < items.Length; i++)
        {
            items[i].itemName = itemDatas[i].name;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GoodsManager<T>
{
    readonly HashSet<T> _goods;
    public GoodsManager(HashSet<T> initialGoods) => _goods = initialGoods;

    public T GetRandomGoods()
    {
        T selectedGoods = _goods.ToList().GetRandom();
        _goods.Remove(selectedGoods);
        return selectedGoods;
    }

    public void AddBackGoods(T goods)
    {
        _goods.Add(goods);
    }
}


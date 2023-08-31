using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GoodsManager<T>
{
    readonly HashSet<T> _savedGoods;
    readonly HashSet<T> _useingGoods;
    public GoodsManager(HashSet<T> initialGoods) => _savedGoods = initialGoods;

    public T GetRandomGoods()
    {
        T selectedGoods = _savedGoods.ToList().GetRandom();
        _savedGoods.Remove(selectedGoods);
        return selectedGoods;
    }

    public void AddBackAllGoods()
    {
        foreach (var usedGood in _useingGoods.ToList()) // ToList는 foreach중 컬랙션 변경되면 에러나서 해둠
            AddBackGoods(usedGood);
    }
    void AddBackGoods(T goods)
    {
        _savedGoods.Add(goods);
        _useingGoods.Remove(goods);
    }

    public T ChangeGoods(T currentGoods)
    {
        if (_useingGoods.Contains(currentGoods) == false) 
            throw new ArgumentException("배치되지도 않은 상품을 바꾸려 함");

        AddBackGoods(currentGoods);
        return GetRandomGoods();
    }

    public IEnumerable<T> ChangeAllGoods()
    {
        int originalCount = _useingGoods.Count;
        AddBackAllGoods();

        List<T> newGoods = new List<T>();
        for (int i = 0; i < originalCount; i++)
            newGoods.Add(GetRandomGoods());

        return newGoods;
    }
}


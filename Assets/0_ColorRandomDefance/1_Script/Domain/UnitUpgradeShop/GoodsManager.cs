using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GoodsManager<T>
{
    readonly HashSet<T> _savedGoods;
    readonly HashSet<T> _usingGoods = new HashSet<T>();
    public GoodsManager(IEnumerable<T> initialGoods) => _savedGoods = new HashSet<T>(initialGoods);

    public T GetRandomGoods()
    {
        if(_savedGoods.Count == 0)
            throw new ArgumentException("남아있는 상품이 없는데 가져가려 함");

        T selectedGoods = _savedGoods.ToList().GetRandom();
        _savedGoods.Remove(selectedGoods);
        _usingGoods.Add(selectedGoods);
        return selectedGoods;
    }

    public void AddBackAllGoods()
    {
        foreach (var usedGood in _usingGoods.ToList()) // ToList는 foreach중 컬랙션 변경되면 에러나서 해둠
            AddBackGoods(usedGood);
    }
    void AddBackGoods(T goods)
    {
        _savedGoods.Add(goods);
        _usingGoods.Remove(goods);
    }

    public T ChangeGoods(T currentGoods)
    {
        if (_usingGoods.Contains(currentGoods) == false) 
            throw new ArgumentException("배치되지도 않은 상품을 바꾸려 함");

        var result = GetRandomGoods();
        AddBackGoods(currentGoods);
        return result;
    }

    public IEnumerable<T> ChangeAllGoods()
    {
        int useGoodsCount = _usingGoods.Count;
        IEnumerable<T> savedUseList = new HashSet<T>(_usingGoods);

        List<T> newGoods = new List<T>();
        for (int i = 0; i < useGoodsCount; i++)
            newGoods.Add(GetRandomGoods());

        foreach (var goods in savedUseList)
            AddBackGoods(goods);

        return newGoods;
    }
}


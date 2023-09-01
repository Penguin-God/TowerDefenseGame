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
            throw new ArgumentException("�����ִ� ��ǰ�� ���µ� �������� ��");

        T selectedGoods = _savedGoods.ToList().GetRandom();
        _savedGoods.Remove(selectedGoods);
        _usingGoods.Add(selectedGoods);
        return selectedGoods;
    }

    public void AddBackAllGoods()
    {
        foreach (var usedGood in _usingGoods.ToList()) // ToList�� foreach�� �÷��� ����Ǹ� �������� �ص�
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
            throw new ArgumentException("��ġ������ ���� ��ǰ�� �ٲٷ� ��");

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


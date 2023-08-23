using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GachaMachine
{
    public int SelectIndex(IReadOnlyList<int> rates) => SelectIndex(rates.Select(x => (double)x).ToArray());
    public int SelectIndex(double[] rates)
    {
        if(CheckRateSumIsOenHendred(rates)) throw new ArgumentException("»Æ∑¸¿« «’¿Ã 100¿Ã æ∆¥‘");

        double randomValue = UnityEngine.Random.Range(0f, 100f);
        for (int i = 0; i < rates.Length; i++)
        {
            if (rates[i] >= randomValue)
                return i;
            randomValue -= rates[i];
        }
        return -1;
    }

    bool CheckRateSumIsOenHendred(double[] rates) => Math.Abs(100 - rates.Sum()) > 0.0000001;
}

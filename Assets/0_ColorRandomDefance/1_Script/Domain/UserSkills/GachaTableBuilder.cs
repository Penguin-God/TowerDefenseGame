using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GachaTableBuilder
{
    public IEnumerable<UnitGachaData> CreateGachaTable(int[] rates)
    {
        var result = new List<UnitGachaData>();
        var flagTable = GetFlagTable();

        for (int i = 0; i < rates.Length; i++)
        {
            if (rates[i] != 0)
                result.Add(new UnitGachaData(rates[i], flagTable[i]));
        }
        return result;
    }

    IReadOnlyList<IEnumerable<UnitFlags>> GetFlagTable()
    {
        var result = new List<IEnumerable<UnitFlags>>();
        Queue<UnitFlags> queue = new Queue<UnitFlags>(UnitFlags.NormalFlags.OrderBy(x => x.ClassNumber).ThenBy(x => x.ColorNumber));
        int unitSplitSize = 3;
        int listCount = queue.Count / unitSplitSize - 1; // 마지막 요소(초록, 주황, 보라 법사)는 제외하기 위해 -1 씀

        for (int i = 0; i < listCount; i++)
        {
            var chunk = new List<UnitFlags>();
            for (int j = 0; j < unitSplitSize && queue.Count > 0; j++)
                chunk.Add(queue.Dequeue());
            result.Add(chunk);
        }

        return result;
    }
}

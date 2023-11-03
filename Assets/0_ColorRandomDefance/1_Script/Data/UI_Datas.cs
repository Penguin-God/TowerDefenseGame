using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class UI_UnitWindowData
{
    [SerializeField] UnitFlags _unitFlags;
    [SerializeField] List<UnitFlags> _combineUnitFalgs;
    [SerializeField] string _description;
    [SerializeField] string _combinationRecipe;

    public UnitFlags UnitFlags => _unitFlags;
    public IReadOnlyList<UnitFlags> CombineUnitFlags => _combineUnitFalgs;
    public string Description => _description.Replace("\\n", "\n");
    public string CombinationRecipe => _combinationRecipe.Replace("\\n", "\n");
}

[Serializable]
public class UI_UnitWindowDatas : ICsvLoader<UnitFlags, UI_UnitWindowData>
{
    public Dictionary<UnitFlags, UI_UnitWindowData> MakeDict(string csv)
    {
        List<UI_UnitWindowData> datas = CsvUtility.CsvToArray<UI_UnitWindowData>(csv).ToList();
        return datas.ToDictionary(x => x.UnitFlags, x => x);
    }
}

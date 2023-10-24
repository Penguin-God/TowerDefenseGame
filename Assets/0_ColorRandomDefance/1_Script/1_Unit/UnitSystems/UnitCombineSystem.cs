using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class UnitCombineSystem
{
    readonly IReadOnlyDictionary<UnitFlags, CombineCondition> _combineConditions;
    public UnitCombineSystem(IReadOnlyDictionary<UnitFlags, CombineCondition> combineConditions) => _combineConditions = combineConditions;

    public IEnumerable<UnitFlags> GetCombinableUnitFalgs(IEnumerable<UnitFlags> currentUnits) => GetCombinableUnitFalgs((flag) => GetUnitCount(flag, currentUnits));

    public IEnumerable<UnitFlags> GetCombinableUnitFalgs(Func<UnitFlags, int> getCount)
        => _combineConditions
            .Keys
            .Where(x => CheckCombineable(x, getCount));

    public bool CheckCombineable(UnitFlags targetFlag, Func<UnitFlags, int> getCount)
        => _combineConditions[targetFlag]
            .NeedCountByFlag
            .All(x => getCount(x.Key) >= x.Value);

    public bool CheckCombineable(UnitFlags targetFlag, IEnumerable<UnitFlags> flags) => CheckCombineable(targetFlag, (flag) => GetUnitCount(flag, flags));

    int GetUnitCount(UnitFlags flag, IEnumerable<UnitFlags> currentUnits) => currentUnits.Where(x => x == flag).Count();

    // 필요한 flag들 중복 허용해서 전부 합친 후 열거형으로 반환
    public IEnumerable<UnitFlags> GetNeedFlags(UnitFlags unitFlag)
        => _combineConditions[unitFlag]
                .NeedCountByFlag
                .SelectMany(item => Enumerable.Repeat(item.Key, item.Value));
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IUnitOperationHandler
{
    public void Do(UnitFlags flag);
    public IEnumerable<UnitFlags> GetOperableUnits(IEnumerable<UnitFlags> units);
}

public class UnitCombineHandler : IUnitOperationHandler
{
    UnitCombineMultiController _combineController;
    public UnitCombineHandler(UnitCombineMultiController combineController) => _combineController = combineController;
    public void Do(UnitFlags flag) => _combineController.TryCombine(flag);
    public IEnumerable<UnitFlags> GetOperableUnits(IEnumerable<UnitFlags> units) => _combineController.CombineSystem.GetCombinableUnitFalgs(units);
}

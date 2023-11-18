using System.Collections;
using System.Collections.Generic;
using System.Linq;

public interface IUnitOperationHandler
{
    public void Do(UnitFlags flag);
    public IEnumerable<UnitFlags> GetOperableUnits(IEnumerable<UnitFlags> units);
}

public class UnitCombineHandler : IUnitOperationHandler
{
    readonly UnitCombineMultiController _combineController;
    public UnitCombineHandler(UnitCombineMultiController combineController) => _combineController = combineController;
    public void Do(UnitFlags flag) => _combineController.TryCombine(flag);
    public IEnumerable<UnitFlags> GetOperableUnits(IEnumerable<UnitFlags> units) => _combineController.CombineSystem.GetCombinableUnitFalgs(units);
}

public class UnitTowerMoveHandler : IUnitOperationHandler
{
    readonly UnitMoveHandler _unitMoveHandler;
    public UnitTowerMoveHandler(UnitManagerController unitManager) => _unitMoveHandler = new UnitMoveHandler(unitManager);
    public void Do(UnitFlags flag) => _unitMoveHandler.ChangeUnitWorld(flag, true);
    public IEnumerable<UnitFlags> GetOperableUnits(IEnumerable<UnitFlags> units) => _unitMoveHandler.GetMovealbeUnits(true);
}

public class UnitWolrdMoveHandler : IUnitOperationHandler
{
    readonly UnitMoveHandler _unitMoveHandler;
    public UnitWolrdMoveHandler(UnitManagerController unitManager) => _unitMoveHandler = new UnitMoveHandler(unitManager);
    public void Do(UnitFlags flag) => _unitMoveHandler.ChangeUnitWorld(flag, false);
    public IEnumerable<UnitFlags> GetOperableUnits(IEnumerable<UnitFlags> units) => _unitMoveHandler.GetMovealbeUnits(false);
}

public class UnitMoveHandler
{
    UnitManagerController _unitManager;
    public UnitMoveHandler(UnitManagerController unitManager) => _unitManager = unitManager;

    public void ChangeUnitWorld(UnitFlags flag, bool isDefense) => GetUnits(isDefense).Where(x => x.UnitFlags == flag).FirstOrDefault()?.ChangeUnitWorld();
    public IEnumerable<UnitFlags> GetMovealbeUnits(bool isDefense) => GetUnits(isDefense).Select(x => x.UnitFlags).Distinct();
    IEnumerable<Multi_TeamSoldier> GetUnits(bool isDefense) => _unitManager.GetUnits(PlayerIdManager.Id).Where(x => x.IsInDefenseWorld == isDefense);
}

public class UnitSellHandler : IUnitOperationHandler
{
    readonly UnitManagerController _unitManager;
    public UnitSellHandler(UnitManagerController unitManager) => _unitManager = unitManager;

    public void Do(UnitFlags flag)
    {
        const int HighUnitMultiple = 2;
        if (_unitManager.TryFindUnit(PlayerIdManager.Id, unit => unit.UnitFlags == flag, out var findUnit))
        {
            int amount = Multi_GameManager.Instance.BattleData.UnitSellRewardDatas[(int)findUnit.UnitClass].Amount;
            if (UnitFlags.GetUnitType(findUnit.UnitColor) == UnitType.High) amount *= HighUnitMultiple;
            Multi_GameManager.Instance.AddGold(amount);
            findUnit.Dead();
        }
    }

    public IEnumerable<UnitFlags> GetOperableUnits(IEnumerable<UnitFlags> units) => units.Distinct();
}

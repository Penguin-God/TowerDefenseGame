using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tutorial_AI : MonoBehaviour
{
    int _gold;
    readonly byte AI_ID = 1;
    List<Multi_TeamSoldier> _units = new List<Multi_TeamSoldier>();

    void Awake()
    {
        _gold = 15;
        StageManager.Instance.OnUpdateStage += (stage) => OnChangeStage();
    }

    void OnChangeStage()
    {
        _gold += 10;
        DrawUnits();
    }

    void DrawUnits() => StartCoroutine(Co_DrawUnits());

    void SpawnUnit(UnitFlags flag)
    {
        var unit = Multi_SpawnManagers.NormalUnit.RPCSpawn(flag, AI_ID);
        _units.Add(unit);
        unit.OnDead += _ => _units.Remove(unit);
    }

    IEnumerator Co_DrawUnits()
    {
        while (_gold >= 5)
        {
            _gold -= 5;
            SpawnUnit(new UnitFlags(Random.Range(0, 3), 0));
            yield return new WaitForSeconds(0.2f);
            TryCombine();
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerable<UnitFlags> UnitFlags => _units.Select(x => x.UnitFlags);
    void TryCombine()
    {
        var combineSystem = new UnitCombineSystem(Managers.Data.CombineConditionByUnitFalg);
        while (combineSystem.GetCombinableUnitFalgs(UnitFlags).Count() != 0)
        {
            var targetFlag = combineSystem.GetCombinableUnitFalgs(UnitFlags).First();
            foreach (var flag in combineSystem.GetNeedFlags(targetFlag))
                _units.Where(x => x.UnitFlags == flag).First().Dead();
            SpawnUnit(targetFlag);
        }
    }
}

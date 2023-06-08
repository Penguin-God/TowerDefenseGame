using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tutorial_AI : MonoBehaviour
{
    int _gold;
    readonly byte AI_ID = 1;
    List<UnitFlags> _unitFlags = new List<UnitFlags>();

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

    IEnumerator Co_DrawUnits()
    {
        while (_gold >= 5)
        {
            _gold -= 5;
            var unit = Multi_SpawnManagers.NormalUnit.RPCSpawn(new UnitFlags(Random.Range(0, 3), 0), AI_ID);
            _unitFlags.Add(unit.UnitFlags);
            yield return new WaitForSeconds(0.2f);
            TryCombine();
            yield return new WaitForSeconds(0.2f);
        }
    }

    void TryCombine()
    {
        var combineSystem = new UnitCombineSystem(Managers.Data.CombineConditionByUnitFalg);
        while (combineSystem.GetCombinableUnitFalgs(_unitFlags).Count() != 0)
        {
            foreach (var flag in combineSystem.GetNeedFlags(combineSystem.GetCombinableUnitFalgs(_unitFlags).First()))
                _unitFlags.Remove(flag);
        }
    }
}

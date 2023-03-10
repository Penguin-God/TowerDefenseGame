using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [ContextMenu("Test Presenters")]
    void TestPresenters()
    {
        var tester = new PresentersTester();
        tester.TestGenerateColorChangeResultText();
    }

    [ContextMenu("Test Combinealbe Checker")]
    void TestUI()
    {
        var tester = new CombineTester();
        tester.TestGetCombinableUnitFalgs();
    }

    [ContextMenu("Test Data Change")]
    void TestDataChange()
    {
        var tester = new DataChangeTester();
        tester.TestChangeAllUnitStat();
        tester.TestChangeUnitDataWithCondition();
    }
}

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

    [ContextMenu("Test Combine")]
    void TestUI()
    {
        var tester = new CombineTester();
        tester.TestGetCombinableUnitFalgs();
    }

    [ContextMenu("Test Path Build")]
    void TestPathBuild()
    {
        var tester = new PresentersTester();
        tester.TestBuildUnitSpawnPath();
    }
}

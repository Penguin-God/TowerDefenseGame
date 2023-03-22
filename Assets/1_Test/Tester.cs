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
}

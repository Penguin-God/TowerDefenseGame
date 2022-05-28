using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    List<string[]> _test = new List<string[]>();


    public void Click()
    {
        string[] _st_test = new string[3] { "ew", "32", "asd" };
        _test.Add(_st_test);
        CSVWriter.WriteCsv(_test, "name2");
        EX.ReadEX("people");
        EX.test();
        EX.SetValue("이름1", "Age", "바꾼이름");
        EX.test();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataUtility : MonoBehaviour
{
    [ContextMenu("Create Unit Numbers Data")]
    void CreateUnitNumbersFile()
    {
        UnitNumbers unitNumbers = new UnitNumbers();
        for (int i = 0; i < 4; i++)
        {
            for (int k = 0; k < 8; k++)
            {
                unitNumbers.unitNumbers.Add(new UnitNumber(i, k));
            }
        }
        
        File.WriteAllText("Assets/0_Multi/Resources/Data/UnitNumbers.json", JsonUtility.ToJson(unitNumbers, true));
        print("Done");
    }

    [ContextMenu("Test")]
    void Test()
    {
        Dictionary<UnitNumber, string> test = new Dictionary<UnitNumber, string>();
        test.Add(new UnitNumber(0, 1), "");
        test.Add(new UnitNumber(0, 2), "");
        test.Add(new UnitNumber(0, 3), "");
        test.Add(new UnitNumber(0, 4), "");

        
        UnitNumber asadddd = new UnitNumber(0, 1);

        print(test.ContainsKey(asadddd));
        print(test.ContainsKey(new UnitNumber(0, 612)));
    }
}

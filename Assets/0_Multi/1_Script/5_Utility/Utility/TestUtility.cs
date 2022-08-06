using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text;
using System;

public class TestUtility : MonoBehaviour
{
    [SerializeField] UnitFlags flag;

    [SerializeField, TextArea] string texts;

    const string basePath = "C:/Users/parkj/Desktop/Current Project/1.ColorRandomDefense/Assets/0_Multi/Resources/Sounds/";
    [ContextMenu("Test")]
    void Test()
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (string path in Directory.GetFiles(basePath, "*.wav", SearchOption.AllDirectories))
        {
            string value = path.Replace(basePath, "").Replace(".wav", "");
            stringBuilder.Append(value.Split('\\')[value.Split('\\').Length - 1]);
            stringBuilder.Append(",");
            stringBuilder.Append(value);
            stringBuilder.Append('\n');
        }
        texts = stringBuilder.ToString();
        // Multi_Managers.Sound.PlayEffect(EffectSoundType.SwordmanAttack);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            Test();
    }

    [SerializeField] int spawnColorMax;
    [SerializeField] int spawnClassMax;
    [ContextMenu("범위 안의 Unit Spawn")]
    void UnitSpawn()
    {
        for (int i = 0; i <= spawnColorMax; i++)
        {
            for (int j = 0; j <= spawnClassMax; j++)
            {
                Multi_SpawnManagers.NormalUnit.Spawn(i, j);
            }
        }
    }
}
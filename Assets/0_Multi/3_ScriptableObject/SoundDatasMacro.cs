using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

[CreateAssetMenu(fileName = "SoundDatasMacro", menuName = "Macro/Sounds")]
public class SoundDatasMacro : ScriptableObject
{
    [SerializeField, TextArea] string _enumTexts;
    const string basePath = "C:/Users/parkj/Desktop/Current Project/1.ColorRandomDefense/Assets/0_Multi/Resources/Sounds/";
    const string filePath = "C:/Users/parkj/Desktop/Current Project/1.ColorRandomDefense/Assets/0_Multi/Resources/Data/SoundData.csv";

    [ContextMenu("Save Csv File")]
    void SaveCsv()
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (string path in Directory.GetFiles(basePath, "*.wav", SearchOption.AllDirectories))
        {
            string value = path.Replace(basePath, "").Replace(".wav", "").Replace("\\", "/");
            stringBuilder.Append(value.Split('/')[value.Split('/').Length - 1]);
            stringBuilder.Append(",");
            stringBuilder.Append(value);
            stringBuilder.Append('\n');
        }
        CsvUtility.SaveCsv(stringBuilder.ToString(), filePath);
    }

    [ContextMenu("Set Enum Text")]
    void SetEnumText()
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (string path in Directory.GetFiles(basePath, "*.wav", SearchOption.AllDirectories))
        {
            string value = path.Replace(basePath, "").Replace(".wav", "");
            stringBuilder.Append(value.Split('\\')[value.Split('\\').Length - 1]);
            stringBuilder.Append(',');
            stringBuilder.Append('\n');
        }
        _enumTexts = stringBuilder.ToString();
    }
}

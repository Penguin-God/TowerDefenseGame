using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

[CreateAssetMenu(fileName = "SoundDatasMacro", menuName = "Macro/Sounds")]
public class SoundDatasMacro : ScriptableObject
{
    [SerializeField, TextArea] string _enumTexts;
    const string clipPath = "C:/Users/parkj/Desktop/Current Project/1.ColorRandomDefense/Assets/0_Multi/Resources/SoundClips/";
    const string filePath = "C:/Users/parkj/Desktop/Current Project/1.ColorRandomDefense/Assets/0_Multi/Resources/Data/SoundData/EffectSoundData.csv";

    [ContextMenu("Save Csv File")]
    void SaveCsv()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("effectType,path");
        stringBuilder.Append('\n');
        foreach (string path in Directory.GetFiles(clipPath, "*.wav", SearchOption.AllDirectories))
        {
            string value = path.Replace(clipPath, "").Replace(".wav", "").Replace("\\", "/");
            stringBuilder.Append(value.Split('/')[value.Split('/').Length - 1]);
            stringBuilder.Append(",");
            stringBuilder.Append(value);
            stringBuilder.Append('\n');
        }
        Save(stringBuilder.ToString(), filePath);
    }

    [ContextMenu("Set Enum Text")]
    void SetEnumText()
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (string path in Directory.GetFiles(clipPath, "*.wav", SearchOption.AllDirectories))
        {
            string value = path.Replace(clipPath, "").Replace(".wav", "");
            stringBuilder.Append(value.Split('\\')[value.Split('\\').Length - 1]);
            stringBuilder.Append(',');
            stringBuilder.Append('\n');
        }
        _enumTexts = stringBuilder.ToString();
    }

    void Save(string csv, string path)
    {
        Stream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
        StreamWriter outStream = new StreamWriter(fileStream, Encoding.UTF8);
        outStream.Write(csv);
        outStream.Close();
    }
}

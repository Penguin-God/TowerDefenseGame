using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Linq;

[CreateAssetMenu(fileName = "SoundDatasMacro", menuName = "Macro/Sounds")]
public class SoundDatasMacro : ScriptableObject
{
    [SerializeField, TextArea] string _enumTexts;
    string ClipsPath => Path.Combine(Application.dataPath, "0_Multi", "Resources", "SoundClips/");
    string FilePath => Path.Combine(Application.dataPath, "0_Multi", "Resources", "Data", "SoundData", "EffectSoundData.csv");
    //const string ClipsPath = "C:/Users/parkj/Desktop/Current Project/1.ColorRandomDefense/Assets/0_Multi/Resources/SoundClips/";
    const string filePath = "C:/Users/parkj/Desktop/Current Project/1.ColorRandomDefense/Assets/0_Multi/Resources/Data/SoundData/EffectSoundData.csv";

    [ContextMenu("Save Csv File")]
    void SaveCsv()
    {
        string csv = Resources.Load<TextAsset>("Data/SoundData/EffectSoundData").text;
        Dictionary<string, float> pathBuVolumn = CsvUtility.GetEnumerableFromCsv<EffcetSound>(csv).ToDictionary(x => x.Path, x => x.Volumn);

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("effectType,volumn,path");
        stringBuilder.Append('\n');
        foreach (string path in Directory.GetFiles(ClipsPath, "*.wav", SearchOption.AllDirectories))
        {
            string resourcesPath = FilePathToResourcesPath(path);
            stringBuilder.Append(GetClipFileName(resourcesPath));
            stringBuilder.Append(",");
            stringBuilder.Append(GetVolumn(pathBuVolumn, resourcesPath));
            stringBuilder.Append(",");
            stringBuilder.Append(resourcesPath);
            stringBuilder.Append('\n');
        }
        Save(stringBuilder.ToString(), FilePath);
    }

    string GetClipFileName(string path) => path.Split('/')[path.Split('/').Length - 1];
    string FilePathToResourcesPath(string path) => path.Replace(ClipsPath, "").Replace(".wav", "").Replace("\\", "/");
    float GetVolumn(Dictionary<string, float> pathBuVolumn, string path)
    {
        if (pathBuVolumn.TryGetValue(path, out float result) && result > 0.001)
            return result;
        else
            return 0.5f;
    }


    [ContextMenu("Set Enum Text")]
    void SetEnumText()
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (string path in Directory.GetFiles(ClipsPath, "*.wav", SearchOption.AllDirectories))
        {
            string value = path.Replace(ClipsPath, "").Replace(".wav", "");
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

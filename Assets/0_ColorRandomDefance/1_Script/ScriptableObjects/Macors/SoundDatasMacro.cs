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

    string ReplacePath => Path.Combine(Application.dataPath, "0_Multi", "Resources", "SoundClips/").Replace("\\", "/");
    string EffectRootPath => Path.Combine(Application.dataPath, "0_Multi", "Resources", "SoundClips", "Effect/");
    string BgmRootPath => Path.Combine(ReplacePath, "Bgm/");

    string EffectFilePath => Path.Combine(Application.dataPath, "0_Multi", "Resources", "Data", "SoundData", "EffectSoundData.csv");
    string BgmFilePath => Path.Combine(Application.dataPath, "0_Multi", "Resources", "Data", "SoundData", "BgmSoundData.csv");

    [ContextMenu("Save Effect Sound Csv File")]
    void SaveEffectSound() => SaveCsv("effectType", EffectRootPath, EffectFilePath, ".wav");

    [ContextMenu("Save Bgm Csv File")]
    void SaveBgm() => SaveCsv("bgmType", BgmRootPath, BgmFilePath, ".mp3");

    void SaveCsv(string enumName, string rootPath, string savePath, string fileExtension)
    {
        string csv = Resources.Load<TextAsset>("Data/SoundData/EffectSoundData").text;
        Dictionary<string, float> pathBuVolumn = CsvUtility.CsvToArray<EffectSound>(csv).ToDictionary(x => x.Path, x => x.Volumn);

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"{enumName},volumn,path");
        stringBuilder.Append('\n');
        foreach (string path in Directory.GetFiles(rootPath, $"*{fileExtension}", SearchOption.AllDirectories))
        {
            string resourcesPath = FilePathToResourcesPath(path.Replace("\\", "/"), fileExtension);
            stringBuilder.Append(GetClipFileName(resourcesPath));
            stringBuilder.Append(",");
            stringBuilder.Append(GetVolumn(pathBuVolumn, resourcesPath));
            stringBuilder.Append(",");
            stringBuilder.Append(resourcesPath);
            stringBuilder.Append('\n');
        }

        new CsvMacroUseCase().Save(stringBuilder.ToString(), savePath);
    }

    string GetClipFileName(string path) => path.Split('/')[path.Split('/').Length - 1];
    string FilePathToResourcesPath(string path, string fileExtension) => path.Replace("\\", "/").Replace(ReplacePath, "").Replace(fileExtension, "");
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
        foreach (string path in Directory.GetFiles(EffectRootPath, "*.wav", SearchOption.AllDirectories))
        {
            string value = path.Replace(EffectRootPath, "").Replace(".wav", "");
            stringBuilder.Append(value.Split('\\')[value.Split('\\').Length - 1]);
            stringBuilder.Append(',');
            stringBuilder.Append('\n');
        }
        _enumTexts = stringBuilder.ToString();
    }
}

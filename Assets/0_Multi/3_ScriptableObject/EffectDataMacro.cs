using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Linq;

[CreateAssetMenu(fileName = "EffectDatasMacro", menuName = "Macro/Effects")]
public class EffectDataMacro : ScriptableObject
{
    string FileSavePath => Path.Combine(new CsvMacroUseCase().DirvePath, "Data", "EffectData.csv");

    string PreFabPath => Path.Combine("Prefabs", "Effect");
    string ParticlePath => Path.Combine("Prefabs", "Particle");
    string MaterialsPath => "Materials";

    [ContextMenu("Save Effect Csv")]
    void SaveBgm()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("_effectType,_name,_path\n");
        stringBuilder.Append(GetCsv(PreFabPath, ".prefab", EffectType.GameObject));
        stringBuilder.Append(GetCsv(ParticlePath, ".prefab", EffectType.Particle));
        stringBuilder.Append(GetCsv(MaterialsPath, ".mat", EffectType.Material));
        Debug.Log(stringBuilder.ToString());
        new CsvMacroUseCase().Save(stringBuilder.ToString(), FileSavePath);
    }

    string GetCsv(string rootPath, string fileExtension, EffectType effectType)
    {

        StringBuilder stringBuilder = new StringBuilder();
        foreach (var data in new CsvMacroUseCase().GetFileDatas(rootPath, fileExtension))
        {
            stringBuilder.Append(data.Name);
            stringBuilder.Append(",");
            stringBuilder.Append(effectType.ToString());
            stringBuilder.Append(",");
            stringBuilder.Append(data.ResourcesPath);
            stringBuilder.Append('\n');
        }

        return stringBuilder.ToString();
    }
}

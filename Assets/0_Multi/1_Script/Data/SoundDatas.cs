using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public struct EffectSound
{
    [SerializeField] EffectSoundType effectType;
    [SerializeField] float volumn;
    [SerializeField] string path;

    public EffectSoundType EffectType => effectType;
    public float Volumn => volumn;
    public string Path => path;
}

public class EffectSoundLoder : ICsvLoader<EffectSoundType, EffectSound>
{
    public Dictionary<EffectSoundType, EffectSound> MakeDict(string csv)
        => CsvUtility.CsvToArray<EffectSound>(csv).ToDictionary(x => x.EffectType, x => x);
}

public struct BgmSound
{
    [SerializeField] BgmType bgmType;
    [SerializeField] float volumn;
    [SerializeField] string path;

    public BgmType BgmType => bgmType;
    public float Volumn => volumn;
    public string Path => path;
}

public class BgmSoundLoder : ICsvLoader<BgmType, BgmSound>
{
    Dictionary<BgmType, BgmSound> ICsvLoader<BgmType, BgmSound>.MakeDict(string csv)
        => CsvUtility.CsvToArray<BgmSound>(csv).ToDictionary(x => x.BgmType, x => x);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public struct EffcetSound
{
    [SerializeField] EffectSoundType effectType;
    [SerializeField] float volumn;
    [SerializeField] string path;

    public EffectSoundType EffectType => effectType;
    public float Volumn => volumn;
    public string Path => path;
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

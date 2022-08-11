using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public struct EffcetSound
{
    [SerializeField] EffectSoundType effectType;
    [SerializeField] string path;

    public EffectSoundType EffectType => effectType;
    public string Path => path;
}

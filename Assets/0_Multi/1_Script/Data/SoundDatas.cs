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

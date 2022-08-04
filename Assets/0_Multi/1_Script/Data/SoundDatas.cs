using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public struct EffcetSound
{
    [SerializeField] int index;
    [SerializeField] string path;

    public EffectSoundType SoundType => (EffectSoundType)index;
    public string Path => path;
}

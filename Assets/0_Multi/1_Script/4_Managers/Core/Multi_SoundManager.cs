using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum EffectSoundType
{
    BossDeadClip,
    Click_XButton,
    Denger,
    GetFood,
    GetPassiveGold,
    GoodsBuySound,
    Lose,
    NewStageClip,
    PopSound,
    PopSound_2,
    SelectColor,
    ShopGoodsClick,
    TowerDieClip,
    TransformWhiteUnit,
    UnitUp,
    Warning,
    Win,
    ArcherAttack,
    MeteorExplosion,
    SpearmanAttack,
    SpearmanSkill,
    SwordmanAttack,
    MageAttack,
}

public enum SoundType
{
    Bgm,
    Effect,
}

public class Multi_SoundManager
{
    AudioSource[] _sources;
    Dictionary<string, AudioClip> _clipByPath = new Dictionary<string, AudioClip>();
    Dictionary<EffectSoundType, string> pathBySound = new Dictionary<EffectSoundType, string>();

    public void Init(Transform parent)
    {
        GameObject root = GameObject.Find("@Sound");
        if(root == null)
        {
            root = new GameObject("@Sound");
            List<AudioSource> audios = new List<AudioSource>();
            foreach (string name in Enum.GetNames(typeof(SoundType)))
            {
                GameObject go = new GameObject(name);
                audios.Add(go.AddComponent<AudioSource>());
                go.transform.parent = root.transform;
            }

            _sources = audios.ToArray();
            _sources[(int)SoundType.Bgm].loop = true;

            string csv = Multi_Managers.Resources.Load<TextAsset>("Data/SoundData/EffectSoundData").text;
            pathBySound = CsvUtility.GetEnumerableFromCsv<EffcetSound>(csv).ToDictionary(x => x.EffectType, x => x.Path);
            foreach (var item in pathBySound)
            {
                Debug.Log($"{item.Key} : {item.Value}");
            }
        }
        root.transform.parent = parent;
    }

    public void PlayBgm(string _path) => PlayBgm(GetOrAddClip(_path));
    public void PlayBgm(AudioClip _clip)
    {
        AudioSource _audioSource = _sources[(int)SoundType.Bgm];
        if (_audioSource.isPlaying) _audioSource.Stop();

        _audioSource.clip = _clip;
        _audioSource.Play();
    }

    public void PlayEffect(EffectSoundType sound, Func<bool> condition, float volumeScale = 1)
    {
        if (condition())
            PlayEffect(sound, volumeScale);
    }
    public void PlayEffect(EffectSoundType sound, float volumeScale = 1) => PlayEffect(pathBySound[sound], volumeScale);
    public void PlayEffect(string path, float volumeScale) => PlayEffect(GetOrAddClip(path), volumeScale);
    public void PlayEffect(AudioClip clip, float volumeScale) => _sources[(int)SoundType.Effect].PlayOneShot(clip, volumeScale);

    AudioClip GetOrAddClip(string path)
    {
        if (path.Contains("SoundClips/") == false) path = $"SoundClips/{path}";

        if (_clipByPath.TryGetValue(path, out AudioClip clip))
            return clip;
        else
        {
            AudioClip _newClip = Multi_Managers.Resources.Load<AudioClip>(path);
            _clipByPath.Add(path, _newClip);
            return _newClip;
        }
    }
}

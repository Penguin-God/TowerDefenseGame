﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SoundType
{
    Bgm,
    Effect,
}

public enum BgmType
{
    Boss,
    Default,
}

public enum EffectSoundType
{
    BossDeadClip,
    Click_XButton,
    DebuffSkill,
    Denger,
    DrawSwordman,
    EnemySelected,
    GetFood,
    GetPassiveGold,
    GoodsBuySound,
    LightningClip,
    Lose,
    NewStageClip,
    PopSound,
    PopSound_2,
    SelectColor,
    ShopGoodsClick,
    ShowOtherPlayerInfo,
    ShowRandomShop,
    Swing,
    TowerDieClip,
    TransformWhiteUnit,
    UnitTp,
    Warning,
    Win,
    ArcherAttack,
    BlackMageSkill,
    BlueMageSkill,
    MageAttack,
    MageBallBonce,
    MeteorExplosion,
    RedMageSkill,
    SpearmanAttack,
    SpearmanSkill,
    SwordmanAttack,
    VioletMageSkill,
    WaterBolt,
    YellowMageSkill,
}

public class SoundManager
{
    AudioSource[] _sources;
    Dictionary<string, AudioClip> _clipByPath = new Dictionary<string, AudioClip>();

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
            _sources[(int)SoundType.Effect].volume = 0.5f;
        }
        root.transform.parent = parent;
    }

    public void PlayBgm(BgmType bgmType) => PlayBgm(Managers.Data.BgmBySound[bgmType].Path, Managers.Data.BgmBySound[bgmType].Volumn);
    public void PlayBgm(string _path, float volumn = 0.25f) => PlayBgm(GetOrAddClip(_path), volumn);
    public void PlayBgm(AudioClip _clip, float volumn = 0.25f)
    {
        AudioSource _audioSource = _sources[(int)SoundType.Bgm];
        if (_audioSource.isPlaying) _audioSource.Stop();

        _audioSource.volume = volumn;
        _audioSource.clip = _clip;
        _audioSource.Play();
    }

    public void PlayEffect(EffectSoundType sound, float volumeScale = -1)
    {
        float applyVolumn = (volumeScale < 0) ? Managers.Data.EffectBySound[sound].Volumn : volumeScale;
        PlayEffect(Managers.Data.EffectBySound[sound].Path, applyVolumn);
    }
    public void PlayEffect(string path, float volumeScale) => PlayEffect(GetOrAddClip(path), volumeScale);
    public void PlayEffect(AudioClip clip, float volumeScale) => _sources[(int)SoundType.Effect].PlayOneShot(clip, volumeScale);

    AudioClip GetOrAddClip(string path)
    {
        if (path.Contains("SoundClips/") == false) path = $"SoundClips/{path}";

        if (_clipByPath.TryGetValue(path, out AudioClip clip))
            return clip;
        else
        {
            AudioClip _newClip = Managers.Resources.Load<AudioClip>(path);
            _clipByPath.Add(path, _newClip);
            return _newClip;
        }
    }
}
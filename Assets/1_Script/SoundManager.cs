using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayClip
{
    public string clipName;
    public AudioClip playClip;
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager m_Instance;
    public static SoundManager instance
    {
        get
        {
            if (m_Instance == null) m_Instance = FindObjectOfType<SoundManager>();
            return m_Instance;
        }
    }

    private void Awake()
    {
        if (instance != this) Destroy(gameObject);

        SetClipDictionary(effectClips, dic_EffectClip);

        dic_EffectClip = new Dictionary<string, AudioClip>();
        for (int i = 0; i < effectClips.Length; i++)
        {
            dic_EffectClip.Add(effectClips[i].clipName, effectClips[i].playClip);
        }
        Debug.Log(dic_EffectClip.Count);

    }

    [SerializeField] AudioClip shopEnterClip;
    [SerializeField] AudioClip towerDeadClip;

    [SerializeField] PlayClip[] effectClips;
    private Dictionary<string, AudioClip> dic_EffectClip;
    void SetClipDictionary(PlayClip[] playClips, Dictionary<string, AudioClip> dic_EffectClip)
    {
        dic_EffectClip = new Dictionary<string, AudioClip>();
        for(int i = 0; i < playClips.Length; i++)
        {
            dic_EffectClip.Add(playClips[i].clipName, playClips[i].playClip);
        }
    }

    public void PlayTowerDeadClip()
    {

    }
}

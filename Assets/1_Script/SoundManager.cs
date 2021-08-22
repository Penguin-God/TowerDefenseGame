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

        dic_EffectClip = new Dictionary<string, AudioClip>();
        for (int i = 0; i < effectClips.Length; i++)
        {
            dic_EffectClip.Add(effectClips[i].clipName, effectClips[i].playClip);
        }
    }

    [SerializeField] AudioSource effectAudio;
    [SerializeField] PlayClip[] effectClips;
    private Dictionary<string, AudioClip> dic_EffectClip;

    public void PlayEffectSound_ByName(string name, float volumeScale = 1f) // 사운드 조절 가능
    {
        AudioClip audioClip = null;
        if (dic_EffectClip.TryGetValue(name, out audioClip)) effectAudio.PlayOneShot(dic_EffectClip[name], volumeScale);
        else Debug.LogWarning("찾을 수 없는 사운드 이름 : " + name);
    }

    public void PlayEffectClip_ByName(string name) // 사운드 조절 불가능 유니티 이벤트 용으로 만듬
    {
        AudioClip audioClip = null;
        if (dic_EffectClip.TryGetValue(name, out audioClip)) effectAudio.PlayOneShot(dic_EffectClip[name]);
        else Debug.LogWarning("찾을 수 없는 사운드 이름 : " + name);
    }
}

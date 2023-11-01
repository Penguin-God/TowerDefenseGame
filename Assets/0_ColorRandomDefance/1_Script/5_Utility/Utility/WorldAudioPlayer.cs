using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldAudioPlayer : MonoBehaviour
{
    CameraController _cameraController;
    SoundManager _soundManager;
    public void DependencyInject(CameraController cameraController, SoundManager soundManager)
    {
        _cameraController = cameraController;
        _soundManager = soundManager;
    }

    bool PlayCondition(ObjectSpot objectSpot) => objectSpot == _cameraController.CameraSpot;

    public void PlayObjectEffectSound(ObjectSpot objectSpot, EffectSoundType soundType, float volumn = -1f)
    {
        if (PlayCondition(objectSpot))
            _soundManager.PlayEffect(soundType, volumn);
    }

    public void PlayObjectEffectSound(ObjectSpot objectSpot, AudioSource audioSource)
    {
        if (PlayCondition(objectSpot))
            audioSource.Play();
    }

    public void AfterPlaySound(EffectSoundType soundType, float delayTime) => StartCoroutine(Co_AfterPlaySound(soundType, delayTime));

    IEnumerator Co_AfterPlaySound(EffectSoundType soundType, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        _soundManager.PlayEffect(soundType);
    }
}

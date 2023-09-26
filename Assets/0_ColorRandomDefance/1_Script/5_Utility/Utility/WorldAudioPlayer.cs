using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldAudioPlayer : MonoBehaviour
{
    CameraController _cameraController;
    SoundManager _soundManager;
    public void ReceiveInject(CameraController cameraController, SoundManager soundManager)
    {
        _cameraController = cameraController;
        _soundManager = soundManager;
    }

    public void PlayObjectEffectSound(ObjectSpot objectSpot, EffectSoundType soundType, float volumn = -1f)
    {
        if (objectSpot == _cameraController.CameraSpot)
            _soundManager.PlayEffect(soundType, volumn);
    }

    public void AfterPlaySound(EffectSoundType soundType, float delayTime) => StartCoroutine(Co_AfterPlaySound(soundType, delayTime));

    IEnumerator Co_AfterPlaySound(EffectSoundType soundType, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        _soundManager.PlayEffect(soundType);
    }
}

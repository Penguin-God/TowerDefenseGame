using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldAudioPlayer
{
    CameraController _cameraController;
    SoundManager _soundManager;
    public WorldAudioPlayer(CameraController cameraController, SoundManager soundManager)
    {
        _cameraController = cameraController;
        _soundManager = soundManager;
    }

    public void PlayObjectEffectSound(ObjectSpot objectSpot, EffectSoundType soundType, float volumn = -1f)
    {
        if(objectSpot == _cameraController.CameraSpot)
            _soundManager.PlayEffect(soundType, volumn);
    }
}

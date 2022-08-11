using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager
{
    Camera currentCamera;

    int _lookWorld_Id;
    public int LookWorld_Id => _lookWorld_Id;

    bool _lookEnemyTower;

    public void Init()
    {
        currentCamera = Camera.main;
        _lookWorld_Id = Multi_Data.instance.Id;
    }

    public void LookWorldChanged()
    {
        _lookWorld_Id = (_lookWorld_Id == 0) ? 1 : 0;
        currentCamera.transform.position = Multi_Data.instance.CameraPositions[_lookWorld_Id];
    }

    public void Clear()
    {
        currentCamera = null;
    }
}

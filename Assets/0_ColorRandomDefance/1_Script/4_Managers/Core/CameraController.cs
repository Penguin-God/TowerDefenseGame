using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController
{
    Camera currentCamera;
    public ObjectSpot CameraSpot { get; private set; }

    int _lookWorld_Id;
    public int LookWorld_Id => _lookWorld_Id;
    public bool IsLookOtherWolrd => _lookWorld_Id != PlayerIdManager.Id;

    bool _isLookEnemyTower;
    public bool IsLookEnemyTower => _isLookEnemyTower;
    int lookTowerId => _isLookEnemyTower ? 1 : 0;

    // 0,0 내 세상
    // 0,1 내 타워
    // 1,0 적 세상
    // 1,1 적 타워
    Vector3[,] positions = new Vector3[2, 2];

    public event Action<bool> OnIsLookMyWolrd;
    public event Action OnLookMyWolrd;
    public event Action OnLookEnemyWorld;

    public void EnterBattleScene()
    {
        positions = new Vector3[2, 2]
        {
            {new Vector3(0, 100, -62), new Vector3(500, 100, -62)},
            {new Vector3(0, 100, 438), new Vector3(500, 100, 438) },
        };

        CameraSpot = new ObjectSpot(PlayerIdManager.Id, true);

        _isLookEnemyTower = false;
        currentCamera = Camera.main;
        _lookWorld_Id = PlayerIdManager.Id;
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        currentCamera.transform.position = positions[_lookWorld_Id, lookTowerId];
        OnIsLookMyWolrd?.Invoke(_lookWorld_Id == PlayerIdManager.Id);
        if (_lookWorld_Id == PlayerIdManager.Id) OnLookMyWolrd?.Invoke();
        else OnLookEnemyWorld?.Invoke();
    }

    public void LookWorldChanged()
    {
        CameraSpot = CameraSpot.ChangeWorldId();
        _lookWorld_Id = (_lookWorld_Id == 0) ? 1 : 0;
        UpdateCameraPosition();
    }

    public void LookEnemyTower()
    {
        CameraSpot = new ObjectSpot(CameraSpot.WorldId, false);
        _isLookEnemyTower = true;
        UpdateCameraPosition();
    }

    public void LookWorld()
    {
        CameraSpot = new ObjectSpot(CameraSpot.WorldId, true);
        _isLookEnemyTower = false;
        UpdateCameraPosition();
    }

    public void Clear()
    {
        OnLookMyWolrd = null;
        OnLookEnemyWorld = null;
        OnIsLookMyWolrd = null;

        currentCamera = null;
        _isLookEnemyTower = false;
        _lookWorld_Id = -1;
    }
}

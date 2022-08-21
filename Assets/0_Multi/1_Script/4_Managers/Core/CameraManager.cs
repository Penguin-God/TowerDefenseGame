using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager
{
    Camera currentCamera;

    int _lookWorld_Id;
    public int LookWorld_Id => _lookWorld_Id;

    bool _isLookEnemyTower;
    public bool IsLookEnemyTower => _isLookEnemyTower;
    int lookTowerId => _isLookEnemyTower ? 1 : 0;

    // 0,0 내 세상
    // 0,1 내 타워
    // 1,0 적 세상
    // 1,1 적 타워
    Vector3[,] positions = new Vector3[2, 2];

    public void Init()
    {
        positions = new Vector3[2, 2]
        {
            {new Vector3(0, 100, -62), new Vector3(500, 100, -62)},
            {new Vector3(0, 100, 438), new Vector3(500, 100, 438) },
        };

        currentCamera = Camera.main;
        _lookWorld_Id = Multi_Data.instance.Id;
        UpdateCameraPosition();
    }

    void UpdateCameraPosition() => currentCamera.transform.position = positions[_lookWorld_Id, lookTowerId];

    public void LookWorldChanged()
    {
        _lookWorld_Id = (_lookWorld_Id == 0) ? 1 : 0;
        UpdateCameraPosition();
    }

    public void LookEnemyTower()
    {
        _isLookEnemyTower = true;
        UpdateCameraPosition();
    }

    public void LookWorld()
    {
        _isLookEnemyTower = false;
        UpdateCameraPosition();
    }

    public void Clear()
    {
        currentCamera = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        var container = new BattleDIContainer(gameObject);
        container.AddService(new PlayerManager("PenguinGod", 0, 0));

        // Screen.SetResolution(1920, 1080, true);
        _isFullScreen = true;
        Managers.Resources.DependencyInject(new PoolManager("@PoolManager"));
        Managers.Sound.StopBgm(); // 로비 BGM 뭐하지?
    }

    bool _isFullScreen;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_isFullScreen)
            {
                Screen.SetResolution(800, 480, false);
                _isFullScreen = false;
            }
            else
            {
                Screen.SetResolution(1920, 1080, true);
                _isFullScreen = true;
            }
        }
    }
}

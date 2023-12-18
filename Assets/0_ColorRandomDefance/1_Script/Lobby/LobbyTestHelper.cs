using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyTestHelper : MonoBehaviour
{
    BattleDIContainer _container;
    public void SetContainer(BattleDIContainer container) => _container = container;

    void Update()
    {
        ShowMeTheMoney();
        SetScreen();
    }

    void ShowMeTheMoney()
    {
        var playerData = _container.GetService<PlayerDataManager>();
        if (Input.GetKeyDown(KeyCode.M))
        {
            playerData.AddGold(1000);
            playerData.AddGem(500);
        }
    }

    bool _isFullScreen;
    void SetScreen()
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

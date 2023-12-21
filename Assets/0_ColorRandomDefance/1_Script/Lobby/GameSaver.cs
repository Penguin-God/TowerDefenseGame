using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaver : MonoBehaviour
{
    PlayerDataManager _playerDataManager;
    
    public void SetData(PlayerDataManager playerDataManager)
    {
        _playerDataManager = playerDataManager;
    }

    void OnApplicationQuit()
    {
        new PlayerPrefabsSaver().Save(_playerDataManager);
    }
}

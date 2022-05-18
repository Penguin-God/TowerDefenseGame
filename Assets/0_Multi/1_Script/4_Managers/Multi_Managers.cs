using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Managers : MonoBehaviourPun
{
    private static Multi_Managers instance;
    private static Multi_Managers Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Multi_Managers>();
                if (instance == null)
                    instance = new GameObject("Multi_Managers").AddComponent<Multi_Managers>();

                instance.Init();
            }

            return instance;
        }
    }

    Multi_DataManager _data = new Multi_DataManager();
    Multi_UI_Manager _ui = new Multi_UI_Manager();
    Multi_ResourcesManager _resources = new Multi_ResourcesManager();
    Multi_PoolManager _pool = new Multi_PoolManager();


    public static Multi_DataManager Data => Instance._data;
    public static Multi_UI_Manager UI => Instance._ui;
    public static Multi_ResourcesManager Resources => Instance._resources;
    public static Multi_PoolManager Pool => Instance._pool;

    void Init()
    {
        // ui 테스트 때문에 잠시 비활성화
        // if (!photonView.IsMine) return;
        _data.Init();
        _pool.Init();

        // temp code
        _ui.ShowSceneUI<Multi_UI_Paint>("Paint");
    }

    public void Clear()
    {
        _ui.Clear();
    }

    #region Test
    [ContextMenu("Init Data")]
    void DataInit() => _data.Init();
    #endregion
}

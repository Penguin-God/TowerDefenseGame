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
            }

            return instance;
        }
    }

    Multi_ResourcesManager _resources = new Multi_ResourcesManager();
    Multi_PoolManager _pool = new Multi_PoolManager();

    public static Multi_ResourcesManager Resources => Instance._resources;
    public static Multi_PoolManager Pool => Instance._pool;

    void Awake()
    {
        if (!photonView.IsMine) return;

        _pool.Init();
    }
}

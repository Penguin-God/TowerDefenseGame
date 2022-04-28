﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_ResourcesManager
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject PhotonInsantiate(string path, Transform parent = null) => PhotonInsantiate(path, Vector3.zero, Quaternion.identity, parent);

    public GameObject PhotonInsantiate(string path, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if(prefab == null)
        {
            Debug.LogWarning($"찾을 수 없는 리소스 경로 {path}");
            return null;
        }
        prefab = PhotonNetwork.Instantiate($"Prefabs/{path}", position, rotation);
        if (parent != null) prefab.transform.SetParent(parent);
        return prefab;
    }
}

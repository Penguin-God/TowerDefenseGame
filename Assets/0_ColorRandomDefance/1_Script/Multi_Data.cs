﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using System;

class CustomUnitFlagsType
{
    public static byte[] Serialize(object obj)
    {
        UnitFlags flag = (UnitFlags)obj;
        byte buffer = (byte)((flag.ColorNumber << 4) | flag.ClassNumber);
        return BitConverter.GetBytes(buffer);
    }

    public static object DeSerialize(byte[] bytes)
    {
        byte buffer = bytes[0];
        UnitFlags flag = new UnitFlags(buffer >> 4, buffer & 0xF);
        return flag;
    }
}

public static class PlayerIdManager
{
    public static byte Id => (byte)(PhotonNetwork.IsMasterClient ? 0 : 1);
    public static byte MasterId => 0;
    public static byte ClientId => 1;
    public static byte InVaildId => 12; // 그냥 0하고 1만 아니면 전부 다 유효하지 않은 값임
    public static byte EnemyId => (byte)(Id == 0 ? 1 : 0);
    public static int MaxPlayerCount => 2;
}

public class Multi_Data : MonoBehaviour
{
    private static Multi_Data m_instance;
    public static Multi_Data instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<Multi_Data>();
                if (m_instance == null)
                    m_instance = new GameObject("multi").AddComponent<Multi_Data>();
            }
            return m_instance;
        }
    }

    private void Awake()
    {
        if (instance != this)
        {
            Debug.LogWarning("Multi Data 2개");
            Destroy(gameObject);
        }
        PhotonPeer.RegisterType(typeof(UnitFlags), 128, CustomUnitFlagsType.Serialize, CustomUnitFlagsType.DeSerialize);
    }


    [Header("World")]
    
    [SerializeField] Vector3[] worldPostions = null;
    public Vector3 GetWorldPosition(int id) => worldPostions[id];

    [SerializeField] Vector3[] enemyTowerWorldPositions = null;
    public Vector3[] EnemyTowerWorldPositions => enemyTowerWorldPositions;
    public Vector3 EnemyTowerWorldPosition => enemyTowerWorldPositions[PlayerIdManager.Id];

    [Header("Enemy")]

    // 적 회전 지점
    [SerializeField] Transform[] enemyTurnPointParents = null;

    public Transform[] GetEnemyTurnPoints(GameObject go) => GetEnemyTurnPoints(go.GetComponent<RPCable>().UsingId);
    Transform[] GetEnemyTurnPoints(int id)
    {
        if (id != 0 && id != 1) print(id);
        Transform[] _result = new Transform[enemyTurnPointParents[id].childCount];
        for (int i = 0; i < _result.Length; i++) _result[i] = enemyTurnPointParents[id].GetChild(i);
        return _result;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Data : MonoBehaviourPun
{
    private static Multi_Data m_instance;
    public static Multi_Data instance
    {
        get
        {
            if (m_instance == null) m_instance = FindObjectOfType<Multi_Data>();
            return m_instance;
        }
    }

    int id;

    [SerializeField] Camera main_camera = null;

    // 카메라 포지션
    [SerializeField] Vector3[] cameraPositions = null;
    public Vector3 CameraPosition => cameraPositions[id];
    public Vector3[] CameraPositions => cameraPositions;

    // 타워 카메라 포지션
    [SerializeField] Vector3[] enemyTowerPositions = null;
    public Vector3 EemeyTowerCamPosition => cameraPositions[id];


    [SerializeField] Vector3[] enemySpawnPos = null;
    public Vector3 EnemySpawnPos => enemySpawnPos[id];

    [SerializeField] Transform[] enemyPoolParent = null;
    public Transform EnemyPoolParent => enemyPoolParent[id];

    // 적 회전 지점
    [SerializeField] Transform[] enemyTurnPointParents = null;
    public Transform[] EnemyTurnPoints
    {
        get
        {
            Transform[] _result = new Transform[enemyTurnPointParents[id].childCount];
            for (int i = 0; i < _result.Length; i++) _result[i] = enemyTurnPointParents[id].GetChild(i);
            return _result;
        }
    }


    [SerializeField] Transform[] enemyTowerSpawnPos = null;
    public Transform EnemyTowerParent => enemyTowerSpawnPos[id];
    public Vector3 EnemyTowerSpawnPos => enemyTowerSpawnPos[id].position;


    [SerializeField] Vector3[] unitSpawnPos = null;
    [SerializeField] Vector3[] unitTowerSpawnPos = null;

    public Vector3 UnitSpawnPos => unitSpawnPos[id] + GetAddPosition(-20, 20, -10, 10);
    //public Vector3 UnitTowerSpawnPos => unitTowerSpawnPos[id] + GetAddPosition(-20, 20, -10, 10);

    Vector3 GetAddPosition(float xMin, float xMax, float zMin, float zMax)
    {
        float xPos = Random.Range(xMin, xMax);
        float zPos = Random.Range(zMin, zMax);

        return new Vector3(xPos, 0, zPos);
    }

    private void Awake()
    {
        if(instance != this)
        {
            Debug.LogWarning("Multi Data 2개");
            Destroy(gameObject);
        }

        SetMultiData();
        SetDebugData();
    }

    void SetMultiData()
    {
        id = (PhotonNetwork.IsMasterClient) ? 0 : 1;

        main_camera.transform.position = CameraPosition;
    }

    [Space][Space][Space]
    [SerializeField] Vector3 debug_cameraPosition;
    [SerializeField] Vector3 debug_enemyTowerPosition;
    [SerializeField] Transform[] debug_EnemyTurnPoints;
    void SetDebugData()
    {
        debug_cameraPosition = CameraPosition;
        debug_enemyTowerPosition = EemeyTowerCamPosition;
        debug_EnemyTurnPoints = EnemyTurnPoints;
    }
}

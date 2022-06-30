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

    private void Awake()
    {
        if (instance != this)
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

    public bool CheckIdSame(GameObject go) => go.GetOrAddComponent<Poolable>().UsingId == id;

    // id가 0이면 호스트 1이면 클라이언트 이 아이디를 이용해서 데이터를 정함
    [SerializeField] int id;
    public int Id => id;

    // 배열의 0번째는 호스트 값 1번째는 클라이언트 값
    [SerializeField] Camera main_camera = null;

    [Header("Camera")]
    // 카메라 포지션
    [SerializeField] Vector3[] cameraPositions = null;
    public Vector3[] CameraPositions => cameraPositions;
    public Vector3 CameraPosition => cameraPositions[id];

    // 타워를 보는 카메라 포지션
    [SerializeField] Vector3[] cameraPositions_LookAtTower = null;
    public Vector3[] CameraPositions_LookAtTower => cameraPositions_LookAtTower;
    public Vector3 CameraPosition_LookAtTower => cameraPositions_LookAtTower[id];
    
    [Header("World")]
    
    [SerializeField] Vector3[] worldPostions = null;
    public Vector3[] WorldPostions => worldPostions;
    public Vector3 WorldPostion => worldPostions[id];
    
    [SerializeField] Vector3[] enemyTowerWorldPositions = null;
    public Vector3[] EnemyTowerWorldPositions => enemyTowerWorldPositions;
    public Vector3 EnemyTowerWorldPosition => enemyTowerWorldPositions[id];

    [Header("Enemy")]
    [SerializeField] Vector3[] enemySpawnPos = null;
    public Vector3 EnemySpawnPos => enemySpawnPos[id];

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

    public Transform[] GetEnemyTurnPoints(GameObject go)
        => GetEnemyTurnPoints(go.GetOrAddComponent<Poolable>().UsingId);
    Transform[] GetEnemyTurnPoints(int id)
    {
        Transform[] _result = new Transform[enemyTurnPointParents[id].childCount];
        for (int i = 0; i < _result.Length; i++) _result[i] = enemyTurnPointParents[id].GetChild(i);
        return _result;
    }

    [SerializeField] Transform[] enemyTowerSpawnPos = null;
    public Transform EnemyTowerParent => enemyTowerSpawnPos[id];
    public Vector3 EnemyTowerSpawnPos => enemyTowerSpawnPos[id].position;

    [Header("Unit")]
    [SerializeField] Vector3[] unitSpawnPos = null;
    [SerializeField] Vector3[] unitTowerSpawnPos = null;

    [Header("Debug data")]
    [SerializeField] Vector3 my_cameraPosition;
    [SerializeField] Vector3 my_enemyTowerPosition;
    [SerializeField] Transform[] my_EnemyTurnPoints;
    void SetDebugData()
    {
        my_cameraPosition = CameraPosition;
        my_enemyTowerPosition = CameraPosition_LookAtTower;
        my_EnemyTurnPoints = GetEnemyTurnPoints(id);
    }
}

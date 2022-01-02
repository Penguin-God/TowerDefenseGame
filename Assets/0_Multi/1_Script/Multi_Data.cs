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

    [SerializeField] Camera main_camera = null;

    // 카메라 포지션
    [SerializeField] Vector3[] cameraPositions = null;
    public Vector3 CameraPosition { get; private set; }

    // 타워 카메라 포지션
    [SerializeField] Vector3[] enemyTowerPositions = null;
    public Vector3 EemeyTowerPosition { get; private set; }

    // 적 회전 지점
    [SerializeField] Transform[] enemyTurnPointParents = null;
    public Transform[] EnemyTurnPoints { get { return enemyTurnPointParents; } }


    private List<GameObject>[] NormalEnemyLists = new List<GameObject>[2];
    public List<GameObject> NormalEnemyList { get; private set; }

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
        if (PhotonNetwork.IsMasterClient) ApplyMultiData(0);
        else ApplyMultiData(1);

        main_camera.transform.position = CameraPosition;
    }

    void ApplyMultiData(int index)
    {
        CameraPosition = cameraPositions[index];
        EemeyTowerPosition = enemyTowerPositions[index];

        NormalEnemyLists[index] = new List<GameObject>();
        NormalEnemyList = NormalEnemyLists[index];
    }

    [Space][Space][Space]
    [SerializeField] Vector3 debug_cameraPosition;
    [SerializeField] Vector3 debug_enemyTowerPosition;
    [SerializeField] Transform[] debug_EnemyTurnPoints;
    void SetDebugData()
    {
        debug_cameraPosition = CameraPosition;
        debug_enemyTowerPosition = EemeyTowerPosition;
        debug_EnemyTurnPoints = EnemyTurnPoints;
    }
}

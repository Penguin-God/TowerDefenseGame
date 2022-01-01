using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Data : MonoBehaviourPun
{
    public static Multi_Data instance
    {
        get
        {
            if (m_instance == null) m_instance = FindObjectOfType<Multi_Data>();
            return m_instance;
        }
    }
    private static Multi_Data m_instance;

    [SerializeField] Camera main_camera = null;

    [SerializeField] Vector3[] cameraPositions = null;
    public Vector3 CameraPosition { get; private set; }

    [SerializeField] Vector3[] enemyTowerPositions = null;
    public Vector3 EemeyTowerPosition { get; private set; }

    [SerializeField] Vector3[] normalEnemuSpawnPositions = null;
    public Vector3 NormalEnemuSpawnPosition { get; private set; }

    [SerializeField] Transform[] enemyTurnPointParents = null;
    public Transform[] EnemyTurnPoints { get; private set; } = new Transform[4];

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
        if (PhotonNetwork.IsMasterClient)
        {
            CameraPosition = cameraPositions[0];
            EemeyTowerPosition = enemyTowerPositions[0];
            NormalEnemuSpawnPosition = normalEnemuSpawnPositions[0];
            for (int i = 0; i < EnemyTurnPoints.Length; i++) EnemyTurnPoints[i] = enemyTurnPointParents[0].GetChild(i);
            NormalEnemyLists[0] = new List<GameObject>();
            NormalEnemyList = NormalEnemyLists[0];
        }
        else
        {
            CameraPosition = cameraPositions[1];
            EemeyTowerPosition = enemyTowerPositions[1];
            NormalEnemuSpawnPosition = normalEnemuSpawnPositions[1];
            for (int i = 0; i < EnemyTurnPoints.Length; i++) EnemyTurnPoints[i] = enemyTurnPointParents[1].GetChild(i);
            NormalEnemyLists[1] = new List<GameObject>();
            NormalEnemyList = NormalEnemyLists[1];
        }

        main_camera.transform.position = CameraPosition;
    }


    [Space][Space][Space]
    [SerializeField] Vector3 debug_cameraPosition;
    [SerializeField] Vector3 debug_enemyTowerPosition;
    [SerializeField] Vector3 debug_normalEnemuSpawnPosition;
    [SerializeField] Transform[] debug_EnemyTurnPoints;
    void SetDebugData()
    {
        debug_cameraPosition = CameraPosition;
        debug_enemyTowerPosition = EemeyTowerPosition;
        debug_normalEnemuSpawnPosition = NormalEnemuSpawnPosition;
        debug_EnemyTurnPoints = EnemyTurnPoints;
    }
}

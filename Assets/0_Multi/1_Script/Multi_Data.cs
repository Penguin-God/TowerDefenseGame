using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

class CustomUnitFlagsType
{
    public static byte[] Serialize(object obj)
    {
        UnitFlags flag = (UnitFlags)obj;
        Vector3 buffer = new Vector3(flag.ColorNumber, flag.ClassNumber, 0);
        return Protocol.Serialize(buffer);
    }

    public static object DeSerialize(byte[] bytes)
    {
        Vector3 buffer = (Vector3)Protocol.Deserialize(bytes);
        UnitFlags flag = new UnitFlags((int)buffer.x, (int)buffer.y);
        return flag;
    }
}

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
        id = PhotonNetwork.IsMasterClient ? 0 : 1;
        PhotonPeer.RegisterType(typeof(UnitFlags), 128, CustomUnitFlagsType.Serialize, CustomUnitFlagsType.DeSerialize);
    }

    // id가 0이면 호스트 1이면 클라이언트 이 아이디를 이용해서 데이터를 정함
    [SerializeField] int id;
    public int Id => id;
    public int EnemyPlayerId => (Id == 0) ? 1 : 0;
    public bool CheckIdSame(int _id) => id == _id;

    [Header("World")]
    
    [SerializeField] Vector3[] worldPostions = null;
    public Vector3 WorldPostion => worldPostions[id];
    public Vector3 GetWorldPosition(int id) => worldPostions[id];

    [SerializeField] Vector3[] enemyTowerWorldPositions = null;
    public Vector3[] EnemyTowerWorldPositions => enemyTowerWorldPositions;
    public Vector3 EnemyTowerWorldPosition => enemyTowerWorldPositions[id];

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Photon.Pun;

[Serializable]
public struct FolderPoolingData
{
    public string folderName;
    public GameObject[] gos;
    public int poolingCount;
}

public class Multi_NormalUnitSpawner : Multi_SpawnerBase
{
    public event Action<Multi_TeamSoldier> OnSpawn;
    //public event Action<Multi_TeamSoldier> OnDead;

    [SerializeField] FolderPoolingData[] allUnitDatas;
    public IReadOnlyList<FolderPoolingData> AllUnitDatas => allUnitDatas;

    [SerializeField] FolderPoolingData swordmanPoolData;
    [SerializeField] FolderPoolingData archerPoolData;
    [SerializeField] FolderPoolingData spearmanPoolData;
    [SerializeField] FolderPoolingData magePoolData;

    // Init용 코드
    #region Init
    protected override void MasterInit()
    {
        SetAllUnit();
        CreatePool(swordmanPoolData.gos, swordmanPoolData.folderName, swordmanPoolData.poolingCount);
        CreatePool(archerPoolData.gos, archerPoolData.folderName, archerPoolData.poolingCount);
        CreatePool(spearmanPoolData.gos, spearmanPoolData.folderName, spearmanPoolData.poolingCount);
        CreatePool(magePoolData.gos, magePoolData.folderName, magePoolData.poolingCount);
    }

    void CreatePool(GameObject[] gos, string folderName, int count)
    {
        for (int i = 0; i < gos.Length; i++)
            CreatePoolGroup(gos[i], BuildPath(_rootPath, folderName, gos[i]), count);
    }

    [ContextMenu("Set All Unit")]
    void SetAllUnit()
    {
        allUnitDatas = new FolderPoolingData[4];
        allUnitDatas[0] = swordmanPoolData;
        allUnitDatas[1] = archerPoolData;
        allUnitDatas[2] = spearmanPoolData;
        allUnitDatas[3] = magePoolData;
    }
    #endregion

    protected override void SetPoolObj(GameObject go)
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        var unit = go.GetComponent<Multi_TeamSoldier>();
    }

    public void Spawn(UnitFlags flag) => Spawn(flag.ColorNumber, flag.ClassNumber);
    public void Spawn(int unitColor, int unitClass) => Spawn_RPC(GetUnitPath(unitColor, unitClass), GetUnitSpawnPos());
    public void Spawn(UnitFlags flag, int id) => Spawn_RPC(GetUnitPath(flag.ColorNumber, flag.ClassNumber), GetUnitSpawnPos(id), id);
    public void Spawn(int unitColor, int unitClass, Vector3 spawnPos, Quaternion rotation, int id)
        => Spawn_RPC(GetUnitPath(unitColor, unitClass), spawnPos, rotation, id);

    string GetUnitPath(int unitColor, int unitClass) => BuildPath(_rootPath, allUnitDatas[unitClass].folderName, allUnitDatas[unitClass].gos[unitColor]);
    Vector3 GetUnitSpawnPos() => Multi_WorldPosUtility.Instance.GetUnitSpawnPositon();
    Vector3 GetUnitSpawnPos(int id) => Multi_WorldPosUtility.Instance.GetUnitSpawnPositon(id);

    [PunRPC]
    protected override GameObject BaseSpawn(string path, Vector3 spawnPos, Quaternion rotation, int id)
    {
        var unit = base.BaseSpawn(path, spawnPos, rotation, id).GetComponent<Multi_TeamSoldier>();
        unit.Spawn();
        OnSpawn?.Invoke(unit);
        return null;
    }
}

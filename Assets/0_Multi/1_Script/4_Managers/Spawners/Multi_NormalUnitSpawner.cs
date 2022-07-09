using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
    public event Action<Multi_TeamSoldier> OnDead;

    [SerializeField] FolderPoolingData[] allUnitDatas;
    public IReadOnlyList<FolderPoolingData> AllUnitDatas => allUnitDatas;

    [SerializeField] FolderPoolingData swordmanPoolData;
    [SerializeField] FolderPoolingData archerPoolData;
    [SerializeField] FolderPoolingData spearmanPoolData;
    [SerializeField] FolderPoolingData magePoolData;

    // Init용 코드
    #region Init
    protected override void Init()
    {
        SetAllUnit();
    }

    protected override void MasterInit()
    {
        CreatePool(swordmanPoolData.gos, swordmanPoolData.folderName, swordmanPoolData.poolingCount);
        CreatePool(archerPoolData.gos, archerPoolData.folderName, archerPoolData.poolingCount);
        CreatePool(spearmanPoolData.gos, spearmanPoolData.folderName, spearmanPoolData.poolingCount);
        CreatePool(magePoolData.gos, magePoolData.folderName, magePoolData.poolingCount);
    }

    void CreatePool(GameObject[] gos, string folderName, int count)
    {
        for (int i = 0; i < gos.Length; i++)
            CreatePool_InGroup<Multi_TeamSoldier>(gos[i], BuildPath(_rootPath, folderName, gos[i]), count);
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

    public override void SettingPoolObject(object obj)
    {
        Multi_TeamSoldier unit = obj as Multi_TeamSoldier;
        Debug.Assert(unit != null, "오브젝트 캐스팅 실패!!");
        SetUnit();

        void SetUnit()
        {
            unit.OnDead += OnDead;

            if (PhotonNetwork.IsMasterClient == false) return;
            unit.OnDead += deadUnit => Multi_Managers.Pool.Push(deadUnit.GetComponent<Poolable>());
        }
    }

    public void Spawn(UnitFlags flag) => Spawn(flag.ColorNumber, flag.ClassNumber);
    public void Spawn(int unitColor, int unitClass) => Spawn_RPC(GetUnitPath(unitColor, unitClass), GetUnitSpawnPos());
    public void Spawn(UnitFlags flag, int id) => Spawn_RPC(GetUnitPath(flag.ColorNumber, flag.ClassNumber), GetUnitSpawnPos(id), id);

    string GetUnitPath(int unitColor, int unitClass) => BuildPath(_rootPath, allUnitDatas[unitClass].folderName, allUnitDatas[unitClass].gos[unitColor]);
    Vector3 GetUnitSpawnPos() => Multi_WorldPosUtility.Instance.GetUnitSpawnPositon();
    Vector3 GetUnitSpawnPos(int id) => Multi_WorldPosUtility.Instance.GetUnitSpawnPositon(id);

    [PunRPC]
    protected override GameObject BaseSpawn(string path, Vector3 spawnPos, int id)
    {
        OnSpawn?.Invoke(base.BaseSpawn(path, spawnPos, id).GetComponent<Multi_TeamSoldier>());
        return null;
    }
}

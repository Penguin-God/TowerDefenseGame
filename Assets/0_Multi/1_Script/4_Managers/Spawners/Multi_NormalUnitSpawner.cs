using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    // TODO Init class만들기
    #region Init
    public override void Init()
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

    public override void SettingPoolObject(object obj)
    {
        Multi_TeamSoldier unit = obj as Multi_TeamSoldier;
        Debug.Assert(unit != null, "오브젝트 캐스팅 실패!!");
        SetUnit(unit);
    }

    // TODO : 구현하기
    void SetUnit(Multi_TeamSoldier unit)
    {
        unit.OnDead += OnDead;
    }
    #endregion

    public void Spawn(UnitFlags flgas) => Spawn(flgas.ColorNumber, flgas.ClassNumber);
    public void Spawn(UnitColor unitColor, UnitClass unitClass) => Spawn((int)unitColor, (int)unitClass);

    // TODO : OnSpawn?.Invoke() 부분 중복 없애기
    public void Spawn(int unitColor, int unitClass)
    {
        Multi_TeamSoldier unit = Multi_Managers.Resources.PhotonInsantiate(
            BuildPath(_rootPath, allUnitDatas[unitClass].folderName, allUnitDatas[unitClass].gos[unitColor]),
            Multi_WorldPosUtility.Instance.GetUnitSpawnPositon() // spawn position
            ).GetComponent<Multi_TeamSoldier>();
        OnSpawn?.Invoke(unit);
    }

    // 하얀 유닛용 스폰
    public void Spawn(int unitColor, int unitClass, Vector3 spawnPos)
    {
        Multi_TeamSoldier unit = Multi_Managers.Resources.PhotonInsantiate(
            BuildPath(_rootPath, allUnitDatas[unitClass].folderName, allUnitDatas[unitClass].gos[unitColor]), spawnPos).GetComponent<Multi_TeamSoldier>();
        OnSpawn?.Invoke(unit);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Multi_NormalUnitSpawner : MonoBehaviourPun
{
    readonly ResourcesPathBuilder PathBuilder = new ResourcesPathBuilder();
    public event Action<Multi_TeamSoldier> OnSpawn = null;

    ServerMonsterManager _multiMonsterManager;
    MultiData<SkillBattleDataContainer> _multiSkillData;
    public void Injection(ServerMonsterManager multiMonsterManager, MultiData<SkillBattleDataContainer> multiSkillData)
    {
        _multiMonsterManager = multiMonsterManager;
        _multiSkillData = multiSkillData;
    }

    public void Spawn(UnitFlags flag, Vector3 spawnPos) 
        => photonView.RPC(nameof(RPCSpawn), RpcTarget.MasterClient, flag, spawnPos, Quaternion.identity, PlayerIdManager.Id);
    public Multi_TeamSoldier Spawn(UnitFlags flag) => Spawn(flag.ColorNumber, flag.ClassNumber);
    public Multi_TeamSoldier Spawn(int colorNum, int classNum) => Spawn(new UnitFlags(colorNum, classNum), PlayerIdManager.Id);
    public Multi_TeamSoldier Spawn(UnitFlags flag, byte id) => Spawn(flag, Vector3.zero, Quaternion.identity, id);
    public Multi_TeamSoldier Spawn(UnitFlags flag, Vector3 spawnPos, Quaternion rotation, byte id)
    {
        // 소환하고 세팅 별개로 하기
        if (rotation == Quaternion.identity || spawnPos == Vector3.zero)
            photonView.RPC(nameof(RPCSpawn), RpcTarget.MasterClient, flag, id);
        else
            photonView.RPC(nameof(RPCSpawn), RpcTarget.MasterClient, flag, spawnPos, rotation, id);
        return null;
    }

    Vector3 GetUnitSpawnPos(int id) => new WorldSpawnPositionCalculator(20, 0, 0, 0).CalculateWorldPostion(Multi_Data.instance.GetWorldPosition(id));

    // MasterOnly
    [PunRPC]
    public Multi_TeamSoldier RPCSpawn(UnitFlags flag, byte id) => RPCSpawn(flag, GetUnitSpawnPos(id), Quaternion.identity, id);

    [PunRPC]
    Multi_TeamSoldier RPCSpawn(UnitFlags flag, Vector3 spawnPos, Quaternion rotation, byte id)
    {
        var unit = Managers.Multi.Instantiater.PhotonInstantiate(PathBuilder.BuildUnitPath(flag), spawnPos, rotation, id).GetComponent<Multi_TeamSoldier>();
        unit.Injection(flag, Managers.Data.Unit.UnitStatByFlag[flag].GetClone(), MultiServiceMidiator.Server.UnitDamageInfo(id, flag), _multiMonsterManager.GetMultiData(unit.UsingID));
        SetUnitData(unit);
        MultiServiceMidiator.Server.AddUnit(unit);
        if (unit.UsingID == PlayerIdManager.MasterId)
            OnSpawn?.Invoke(unit);
        else
            photonView.RPC(nameof(RPC_CallbackSpawn), RpcTarget.Others, unit.GetComponent<PhotonView>().ViewID);
        return unit;
    }

    void SetUnitData(Multi_TeamSoldier unit)
    {
        if(unit.UnitClass == UnitClass.Spearman)
        {
            ThrowSpearDataContainer throwSpearData;
            if (_multiSkillData.GetData(unit.UsingID).TruGetSkillData(SkillType.마창사, out var skillBattleData))
                throwSpearData = Managers.Resources.Load<ThrowSpearDataContainer>("Data/ScriptableObject/MagicThrowSpearData").ChangeAttackRate(skillBattleData.IntSkillData);
            else
                throwSpearData = Managers.Data.Unit.SpearDataContainer;
            unit.GetComponent<Multi_Unit_Spearman>().SetSpearData(throwSpearData);
        }
    }

    [PunRPC]
    void RPC_CallbackSpawn(int viewID) => OnSpawn?.Invoke(Managers.Multi.GetPhotonViewTransfrom(viewID).GetComponent<Multi_TeamSoldier>());
}

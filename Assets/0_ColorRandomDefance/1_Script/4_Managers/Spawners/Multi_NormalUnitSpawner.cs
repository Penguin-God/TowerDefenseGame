﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Multi_NormalUnitSpawner : MonoBehaviourPun
{
    protected readonly ResourcesPathBuilder PathBuilder = new ResourcesPathBuilder();
    public event Action<Multi_TeamSoldier> OnSpawn = null;

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

    Vector3 GetUnitSpawnPos(int id) => Multi_WorldPosUtility.Instance.GetUnitSpawnPositon(id);

    // MasterOnly
    [PunRPC]
    Multi_TeamSoldier RPCSpawn(UnitFlags flag, byte id) => RPCSpawn(flag, GetUnitSpawnPos(id), Quaternion.identity, id);

    [PunRPC]
    Multi_TeamSoldier RPCSpawn(UnitFlags flag, Vector3 spawnPos, Quaternion rotation, byte id)
    {
        var unit = Managers.Multi.Instantiater.PhotonInstantiate(PathBuilder.BuildUnitPath(flag), spawnPos, rotation, id).GetComponent<Multi_TeamSoldier>();
        unit.SetInfo(flag, Managers.Data.Unit.UnitStatByFlag[flag].GetClone(), MultiServiceMidiator.Server.UnitDamageInfo(id, flag));
        MultiServiceMidiator.Server.AddUnit(unit);
        Multi_UnitManager.Instance.Master.AddUnit(unit);
        if (unit.UsingID == PlayerIdManager.MasterId)
            OnSpawn?.Invoke(unit);
        else
            photonView.RPC(nameof(RPC_CallbackSpawn), RpcTarget.Others, unit.GetComponent<PhotonView>().ViewID);
        return unit;
    }

    [PunRPC]
    void RPC_CallbackSpawn(int viewID) => OnSpawn?.Invoke(Managers.Multi.GetPhotonViewTransfrom(viewID).GetComponent<Multi_TeamSoldier>());
}

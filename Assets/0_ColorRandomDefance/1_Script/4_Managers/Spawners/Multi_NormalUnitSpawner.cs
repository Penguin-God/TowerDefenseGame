using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_NormalUnitSpawner : MonoBehaviourPun
{
    protected readonly ResourcesPathBuilder PathBuilder = new ResourcesPathBuilder();

    public void Spawn(UnitFlags flag) => Spawn(flag.ColorNumber, flag.ClassNumber);
    public void Spawn(int colorNum, int classNum) => Spawn(new UnitFlags(colorNum, classNum), Multi_Data.instance.Id);
    public void Spawn(UnitFlags flag, int id) => Spawn(flag, Vector3.zero, Quaternion.identity, id);
    public void Spawn(UnitFlags flag, Vector3 spawnPos, Quaternion rotation, int id)
    {
        if (rotation == Quaternion.identity || spawnPos == Vector3.zero)
            photonView.RPC(nameof(RPCSpawn), RpcTarget.MasterClient, flag, id);
        else
            photonView.RPC(nameof(RPCSpawn), RpcTarget.MasterClient, flag, spawnPos, rotation, id);
    }

    Vector3 GetUnitSpawnPos(int id) => Multi_WorldPosUtility.Instance.GetUnitSpawnPositon(id);

    [PunRPC]
    void RPCSpawn(UnitFlags flag, int id) => RPCSpawn(flag, GetUnitSpawnPos(id), Quaternion.identity, id);

    [PunRPC]
    void RPCSpawn(UnitFlags flag, Vector3 spawnPos, Quaternion rotation, int id)
    {
        var unit = Managers.Multi.Instantiater.PhotonInstantiate(PathBuilder.BuildUnitPath(flag), spawnPos, rotation, id).GetComponent<Multi_TeamSoldier>();
        unit.Spawn();
        Multi_UnitManager.Instance.Master.AddUnit(unit);
    }
}

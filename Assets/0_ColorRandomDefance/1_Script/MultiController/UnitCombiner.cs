using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public abstract class UnitCombiner : MonoBehaviourPun
{
    protected UnitCombineSystem _combineSystem;
    protected void Init(DataManager data)
    {
        _combineSystem = new UnitCombineSystem(data.CombineConditionByUnitFalg);
    }

    public bool TryCombine(UnitFlags targetFlag)
    {
        if (CanCombine(targetFlag))
        {
            Combine(targetFlag, PlayerIdManager.Id);
            return true;
        }
        return false;
    }

    public abstract bool CanCombine(UnitFlags targetFlag);

    [PunRPC]
    protected abstract void Combine(UnitFlags targetFlag, byte id);
}

public class ClientUnitController : UnitCombiner
{
    UnitManager _unit;
    public void Init(DataManager data, UnitManager unit)
    {
        base.Init(data);
        _unit = unit;
    }

    public override bool CanCombine(UnitFlags targetFlag) => _combineSystem.CheckCombineable(targetFlag, _unit.GetUnitCount);

    [PunRPC]
    protected override void Combine(UnitFlags targetFlag, byte id) => photonView.RPC(nameof(Combine), RpcTarget.MasterClient, targetFlag, id);
}

public class ServerUnitController : UnitCombiner
{
    ServerManager _server;

    public void Init(DataManager data, ServerManager server)
    {
        base.Init(data);
        _server = server;
    }

    public override bool CanCombine(UnitFlags targetFlag) 
        => _combineSystem.CheckCombineable(targetFlag, _server.GetUnits(PlayerIdManager.MasterId).Select(x => x.UnitFlags));

    [PunRPC]
    protected override void Combine(UnitFlags targetFlag, byte id)
    {
        if(_combineSystem.CheckCombineable(targetFlag, _server.GetUnits(id).Select(x => x.UnitFlags)) == false)
            return;

        foreach (var needFlag in _combineSystem.GetNeedFlags(targetFlag))
            _server.GetUnits(id).Where(x => x.UnitFlags == needFlag).First().Dead();

        Multi_SpawnManagers.NormalUnit.Spawn(targetFlag, id);
    }
}
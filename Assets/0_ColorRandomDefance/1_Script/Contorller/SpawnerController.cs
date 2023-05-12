using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class SpawnerController : MonoBehaviourPun
{
    protected UnitSummoner _unitSummoner;

    public void Init(Multi_GameManager game) => _unitSummoner = new UnitSummoner(game);

    [PunRPC]
    public bool TryDrawUnit()
    {
        if (_unitSummoner.CanSummonUnit() == false)
            return false;

        DrawUnit();
        return true;
    }

    protected abstract void DrawUnit();
}

public class ClientSpawnerController : SpawnerController
{
    protected override void DrawUnit() => photonView.RPC(nameof(TryDrawUnit), RpcTarget.MasterClient);
}


public class ServerSpawnerController : SpawnerController
{
    protected override void DrawUnit()
    {
        Multi_SpawnManagers.NormalUnit.Spawn(_unitSummoner.SummonUnitColor(), UnitClass.Swordman);
    }
}

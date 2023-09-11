using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiServiceAttacher
{
    public UnitCombiner AttacherUnitController(BattleDIContainer container)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var server = container.AddComponent<ServerUnitController>();
            server.Init(Managers.Data, MultiServiceMidiator.Server);
            return server;
        }
        else
        {
            var client = container.AddComponent<ClientUnitController>();
            client.Init(Managers.Data, Managers.Unit);
            return client;
        }
    }
}

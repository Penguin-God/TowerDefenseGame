using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiDataWriteController
{
    ServerManager _server;
    public void Init()
    {
        _server = MultiServiceMidiator.Server;
        
        var unit = Multi_UnitManager.Instance;
        unit.OnUnitCountChange += WriteMasterUnitCount;

        var game = Multi_GameManager.Instance;
        game.BattleData.OnMaxUnitChanged += WriteMasterMaxUnitCount;

        var oppent = MultiServiceMidiator.Oppent;
        oppent.OnUnitCountChanged += WriteClientUnitCount;
        oppent.OnUnitMaxCountChanged += WriteClientMaxUnitCount;
    }

    void WriteMasterUnitCount(int count) => MasterData.CurrentUnitCount = count;
    void WriteClientUnitCount(int count) => ClientData.CurrentUnitCount = count;

    void WriteMasterMaxUnitCount(int count) => MasterData.MaxUnitCount = count;
    void WriteClientMaxUnitCount(int count) => ClientData.MaxUnitCount = count;

    MultiBattleData MasterData => _server.GetCountData(PlayerIdManager.MasterId);
    MultiBattleData ClientData => _server.GetCountData(PlayerIdManager.ClientId);
}

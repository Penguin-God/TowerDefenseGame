using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

interface IMultiSkillDataCreater
{
    public bool SKillDataCreateDone();
    public MultiData<SkillBattleDataContainer> GetMultiSkillData();
    public void CreateMultiSKillData();
}

public class MultiBattleSkillDataCreater : MonoBehaviour, IMultiSkillDataCreater
{
    bool _sKillDataCreateDone;
    public bool SKillDataCreateDone() => _sKillDataCreateDone;

    MultiData<SkillBattleDataContainer> _multiSkillData = new MultiData<SkillBattleDataContainer>();
    public MultiData<SkillBattleDataContainer> GetMultiSkillData() => _multiSkillData;

    public void CreateMultiSKillData()
    {
        _multiSkillData.SetData(PlayerIdManager.Id, BattleSkillDataCreater.CreateSkillData(Managers.ClientData, Managers.Data));
        SendSkillData();
    }

    void SendSkillData()
    {
        var client = Managers.ClientData;
        var main = Managers.ClientData.EquipSkillManager.MainSkill;
        var sub = Managers.ClientData.EquipSkillManager.SubSkill;
        GetComponent<PhotonView>().RPC(nameof(SetEnemySkillData), RpcTarget.OthersBuffered, main, client.GetSkillLevel(main), sub, client.GetSkillLevel(sub));
    }

    [PunRPC]
    void SetEnemySkillData(SkillType mainSkill, int mainLevel, SkillType subSkill, int subLevel)
    {
        _multiSkillData.SetData(PlayerIdManager.EnemyId, BattleSkillDataCreater.CreateSkillData(mainSkill, mainLevel, subSkill, subLevel, Managers.Data.UserSkill));
        _sKillDataCreateDone = true;
    }
}

public class TestBattleSkillDataCreater : IMultiSkillDataCreater
{
    public bool SKillDataCreateDone() => true;

    MultiData<SkillBattleDataContainer> _multiSkillData = new MultiData<SkillBattleDataContainer>();
    public MultiData<SkillBattleDataContainer> GetMultiSkillData() => _multiSkillData;

    public void CreateMultiSKillData()
    {
        _multiSkillData.SetData(PlayerIdManager.Id, BattleSkillDataCreater.CreateSkillData(Managers.ClientData, Managers.Data));
        _multiSkillData.SetData(PlayerIdManager.EnemyId, new SkillBattleDataContainer());
    }
}

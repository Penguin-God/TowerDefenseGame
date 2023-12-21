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
        _multiSkillData.SetData(PlayerIdManager.Id, BattleSkillDataCreater.CreateSkillData(new PlayerPrefabsLoder().Load(), Managers.Data));
        SendSkillData();
    }

    void SendSkillData()
    {
        var data = new PlayerPrefabsLoder().Load();
        var main = data.EquipSkillManager.MainSkill;
        var sub = data .EquipSkillManager.SubSkill;
        GetComponent<PhotonView>().RPC(nameof(SetEnemySkillData), RpcTarget.OthersBuffered, main, data.SkillInventroy.GetSkillInfo(main).Level, sub, data.SkillInventroy.GetSkillInfo(sub).Level);
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
        _multiSkillData.SetData(PlayerIdManager.Id, BattleSkillDataCreater.CreateSkillData(new PlayerPrefabsLoder().Load(), Managers.Data));
        _multiSkillData.SetData(PlayerIdManager.EnemyId, new SkillBattleDataContainer());
    }
}

public static class BattleSkillDataCreater
{
    public static SkillBattleDataContainer CreateSkillData(PlayerDataManager playerDataManager, DataManager data)
    {
        var main = playerDataManager.EquipSkillManager.MainSkill;
        var sub = playerDataManager.EquipSkillManager.SubSkill;
        return CreateSkillData(main, playerDataManager.SkillInventroy.GetSkillInfo(main).Level, sub, playerDataManager.SkillInventroy.GetSkillInfo(sub).Level, data.UserSkill);
    }

    public static SkillBattleDataContainer CreateSkillData(SkillType mainSkill, int mainLevel, SkillType subSkill, int subLevel, DataManager.UserSkillData data)
    {
        var result = new SkillBattleDataContainer();
        if (mainSkill == SkillType.None || subSkill == SkillType.None) return result;
        result.ChangeEquipSkill(data.GetSkillBattleData(mainSkill, mainLevel));
        result.ChangeEquipSkill(data.GetSkillBattleData(subSkill, subLevel));
        return result;
    }
}
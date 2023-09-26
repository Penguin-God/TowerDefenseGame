using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UnitFiller
{
    readonly SkillBattleDataContainer _skillData;
    readonly MonsterManager _monsterManager;

    public UnitFiller(SkillBattleDataContainer skillData, MonsterManager monsterManager)
    {
        _skillData = skillData;
        _monsterManager = monsterManager;
    }

    public void FillUnit(Multi_TeamSoldier unit, UnitFlags flag)
    {
        unit.Injection(new Unit(flag, CreateUnitStats(flag, unit.UsingID), new ObjectSpot(unit.UsingID, true)), _monsterManager);
        SetUnitData(unit);
    }

    UnitStats CreateUnitStats(UnitFlags flag, byte id)
    {
        UnitStat stat = Managers.Data.Unit.UnitStatByFlag[flag].GetClone();
        UnitDamageInfo damInfo = MultiServiceMidiator.Server.UnitDamageInfo(id, flag);
        return new UnitStats(damInfo, stat.AttackDelayTime, 1f, stat.AttackRange, stat.Speed);
    }

    void SetUnitData(Multi_TeamSoldier unit)
    {
        if (unit.UnitClass == UnitClass.Spearman)
        {
            ThrowSpearDataContainer throwSpearData;
            if (_skillData.TruGetSkillData(SkillType.마창사, out var skillBattleData))
                throwSpearData = Managers.Resources.Load<ThrowSpearDataContainer>("Data/ScriptableObject/MagicThrowSpearData").ChangeAttackRate(skillBattleData.IntSkillData);
            else
                throwSpearData = Managers.Data.Unit.SpearDataContainer;
            unit.GetComponent<Multi_Unit_Spearman>().SetSpearData(throwSpearData);
        }
    }

    public static UnitSkillController CreateMageSkillController(Multi_TeamSoldier mage, BattleDIContainer battleDIContainer)
    {
        IReadOnlyList<float> skillStats = null;
        if (Managers.Data.MageStatByFlag.TryGetValue(mage.UnitFlags, out MageUnitStat stat))
            skillStats = stat.SkillStats;

        switch (mage.UnitColor)
        {
            case UnitColor.Red: return null;
            case UnitColor.Blue: return null;
            case UnitColor.Yellow: return new GainGoldController(mage.transform, (int)skillStats[0], mage.UsingID, battleDIContainer.GetComponent<WorldAudioPlayer>());
            case UnitColor.Green: return null;
            case UnitColor.Orange: return null;
            case UnitColor.Violet: return null;
            case UnitColor.White: return null;
            case UnitColor.Black: return null;
            default: return null;
        }
    }
}

public class Multi_NormalUnitSpawner : MonoBehaviourPun
{
    readonly ResourcesPathBuilder PathBuilder = new ResourcesPathBuilder();
    
    ServerMonsterManager _multiMonsterManager;
    MultiData<SkillBattleDataContainer> _multiSkillData;
    public void ReceiveInject(ServerMonsterManager multiMonsterManager, MultiData<SkillBattleDataContainer> multiSkillData)
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

    // MasterOnly
    [PunRPC]
    public Multi_TeamSoldier RPCSpawn(UnitFlags flag, byte id) => RPCSpawn(flag, SpawnPositionCalculator.CalculateWorldSpawnPostion(id), Quaternion.identity, id);

    [PunRPC]
    Multi_TeamSoldier RPCSpawn(UnitFlags flag, Vector3 spawnPos, Quaternion rotation, byte id)
    {
        var unit = Managers.Multi.Instantiater.PhotonInstantiate(PathBuilder.BuildUnitPath(flag), spawnPos, rotation, id).GetComponent<Multi_TeamSoldier>();
        new UnitFiller(_multiSkillData.GetData(unit.UsingID), _multiMonsterManager.GetMultiData(unit.UsingID)).FillUnit(unit, flag);
        //unit.Injection(flag, Managers.Data.Unit.UnitStatByFlag[flag].GetClone(), MultiServiceMidiator.Server.UnitDamageInfo(id, flag), _multiMonsterManager.GetMultiData(unit.UsingID));
        //SetUnitData(unit);
        AddUnitToManager(unit);
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

    void AddUnitToManager(Multi_TeamSoldier unit)
    {
        MultiServiceMidiator.Server.AddUnit(unit);
        if (unit.UsingID == PlayerIdManager.MasterId) Managers.Unit.AddUnit(unit);
        else photonView.RPC(nameof(AddUnit), RpcTarget.Others, unit.GetComponent<PhotonView>().ViewID);
    }

    [PunRPC] void AddUnit(int viewID) => Managers.Unit.AddUnit(Managers.Multi.GetPhotonViewComponent<Multi_TeamSoldier>(viewID));
}

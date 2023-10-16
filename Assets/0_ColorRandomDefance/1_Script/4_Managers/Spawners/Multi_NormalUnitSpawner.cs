﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UnitFiller
{
    readonly BattleDIContainer _container;

    public UnitFiller(BattleDIContainer container) => _container = container;

    public void FillUnit(Multi_TeamSoldier unit, UnitFlags flag, UnitDamageInfo damInfo, MonsterManagerController monsterManager, SkillBattleDataContainer skillData)
    {
        unit.Injection(new Unit(flag, CreateUnitStats(flag, damInfo)), monsterManager);
        SetUnitData(unit, skillData);
    }

    UnitStats CreateUnitStats(UnitFlags flag, UnitDamageInfo damInfo)
    {
        UnitStatData stat = Managers.Data.Unit.UnitStatByFlag[flag];
        return new UnitStats(damInfo, stat.AttackDelayTime, stat.AttackSpeed, stat.AttackRange, stat.Speed);
    }

    void SetUnitData(Multi_TeamSoldier unit, SkillBattleDataContainer skillData)
    {
        new UnitPassiveCreator(Managers.Data).AttachedPassive(unit.gameObject, unit.UnitFlags);

        if (unit.UnitClass == UnitClass.Spearman)
        {
            ThrowSpearDataContainer throwSpearData;
            if (skillData.TruGetSkillData(SkillType.마창사, out var skillBattleData))
                throwSpearData = Managers.Resources.Load<ThrowSpearDataContainer>("Data/ScriptableObject/MagicThrowSpearData").ChangeAttackRate(skillBattleData.IntSkillData);
            else
                throwSpearData = Managers.Data.Unit.SpearDataContainer;
            unit.GetComponent<Multi_Unit_Spearman>().SetSpearData(throwSpearData);
        }

        if(unit.UnitClass == UnitClass.Mage)
            unit.GetComponent<Multi_Unit_Mage>().InjectSkillController(CreateMageSkillController(unit));
    }

    UnitSkillController CreateMageSkillController(Multi_TeamSoldier mage)
    {
        IReadOnlyList<float> skillStats = null;
        if (Managers.Data.MageStatByFlag.TryGetValue(mage.UnitFlags, out MageUnitStat stat))
            skillStats = stat.SkillStats;

        switch (mage.UnitColor)
        {
            case UnitColor.Red: return new MeteorShotController(skillStats[0], skillStats[1], _container.GetComponent<MeteorController>());
            case UnitColor.Blue: return new IceCloudController((int)skillStats[0]);
            case UnitColor.Yellow: return new GainGoldController((int)skillStats[0]);
            case UnitColor.Green: return new ShotBounceBall(skillStats[0], skillStats[1], mage.GetComponent<ManaSystem>(), UnitAttackControllerGenerator.GenerateTemplate<BounceBallShotController>(mage), mage);
            case UnitColor.Orange: return new MagicFountainController((int)skillStats[0], skillStats[1], _container.GetComponent<WorldAudioPlayer>());
            case UnitColor.Violet: return new PoisonCloudController((int)skillStats[0], skillStats[1]);
            case UnitColor.Black: return new MultiVectorShotController(skillStats[0]);
            default: return null;
        }
    }
}

public class Multi_NormalUnitSpawner : MonoBehaviourPun
{
    readonly ResourcesPathBuilder PathBuilder = new ResourcesPathBuilder();
    UnitFiller _unitFiller;
    MonsterManagerController _monsterManagerController;
    MultiData<SkillBattleDataContainer> _multiSkillData;
    BattleEventDispatcher _dispatcher;
    UnitStatController _statController;

    public void ReceiveInject(BattleDIContainer container)
    {
        _unitFiller = new UnitFiller(container);
        _multiSkillData = container.GetMultiActiveSkillData();
        _monsterManagerController = container.GetService<MonsterManagerController>();
        _dispatcher = container.GetEventDispatcher();
        _statController = container.GetService<UnitStatController>();
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
        FillUnit(unit, flag);
        AddUnitToManager(unit);
        _dispatcher.NotifyUnitSpawn(unit);
        return unit;
    }

    void FillUnit(Multi_TeamSoldier unit, UnitFlags flag)
    {
        byte id = unit.UsingID;
        _unitFiller.FillUnit(unit, flag, _statController.GetDamageInfo(flag, id), _monsterManagerController, _multiSkillData.GetData(id));
        photonView.RPC(nameof(FillOtherUnit), RpcTarget.Others, unit.GetComponent<PhotonView>().ViewID, flag);
    }

    [PunRPC]
    void FillOtherUnit(int viewID, UnitFlags flag)
    {
        var unit = Managers.Multi.GetPhotonViewComponent<Multi_TeamSoldier>(viewID);
        _unitFiller.FillUnit(unit, flag, new UnitDamageInfo(0, 0), _monsterManagerController, _multiSkillData.GetData(unit.UsingID));
    }

    void AddUnitToManager(Multi_TeamSoldier unit)
    {
        MultiServiceMidiator.Server.AddUnit(unit);
        if (unit.UsingID == PlayerIdManager.MasterId) Managers.Unit.AddUnit(unit);
        else photonView.RPC(nameof(AddUnit), RpcTarget.Others, unit.GetComponent<PhotonView>().ViewID);
    }

    [PunRPC]
    void AddUnit(int viewID)
    {
        var unit = Managers.Multi.GetPhotonViewComponent<Multi_TeamSoldier>(viewID);
        Managers.Unit.AddUnit(unit);
        _dispatcher.NotifyUnitSpawn(unit);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BattleScene : BaseScene
{
    protected override void Init()
    {
        if (PhotonNetwork.InRoom == false)
        {
            print("방에 없누 ㅋㅋ");
            return;
        }
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        Managers.Data.Init();
        if(PhotonNetwork.IsMasterClient)
            GetComponent<PhotonView>().RPC(nameof(InitGame), RpcTarget.All);
    }

    [PunRPC]
    void InitGame()
    {
        MultiServiceMidiator.Instance.Init();
        Managers.Unit.Init(new UnitControllerAttacher().AttacherUnitController(MultiServiceMidiator.Instance.gameObject), Managers.Data);
        new WorldInitializer(gameObject).Init();
    }

    public override void Clear()
    {
        EventIdManager.Clear();
        Managers.Pool.Clear();
    }
}

class UserSkillInitializer
{
    public IEnumerable<UserSkill> InitUserSkill(BattleDIContainer container)
    {
        List<UserSkill> userSkills = new List<UserSkill>();
        foreach (var skillType in Managers.ClientData.EquipSkillManager.EquipSkills)
        {
            if (skillType == SkillType.None)
                continue;
            var userSkill = UserSkillFactory.CreateUserSkill(skillType, container);
            userSkill.InitSkill();
            userSkills.Add(userSkill);
        }
        return userSkills;
    }
}

class WorldInitializer
{
    BattleDIContainer _battleDIContainer;
    public WorldInitializer(GameObject go)
    {
        _battleDIContainer = go.AddComponent<BattleDIContainer>();
    }

    public void Init()
    {
        new MultiInitializer().InjectionBattleDependency(_battleDIContainer);

        Managers.Camera.EnterBattleScene();
        InitMonoBehaviourContainer();
        InitObjectPools();
        BindUnitEvent();
        InitEffect();
    }

    void InitMonoBehaviourContainer()
    {
        _battleDIContainer.AddService<UnitClickController>();
        _battleDIContainer.AddService<RewradController>();
        _battleDIContainer.AddService<UnitColorChangerRpcHandler>();
    }

    void InitObjectPools()
    {
        Managers.Pool.Init();
        if (PhotonNetwork.IsMasterClient == false) return;

        new UnitPoolInitializer().InitPool();
        new MonsterPoolInitializer().InitPool();
        new WeaponPoolInitializer().InitPool();
    }

    void BindUnitEvent()
    {
        Multi_SpawnManagers.NormalUnit.OnSpawn += Managers.Unit.AddUnit;
        Managers.Unit.OnCombine += new UnitPassiveController().AddYellowSwordmanCombineGold;
    }

    void InitEffect()
    {
        foreach (var data in CsvUtility.CsvToArray<EffectData>(Managers.Resources.Load<TextAsset>("Data/EffectData").text))
        {
            switch (data.EffectType)
            {
                case EffectType.GameObject:
                    Managers.Pool.CreatePool_InGroup(data.Path, 3, "Effects");
                    break;
            }
        }
    }
}

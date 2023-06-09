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

        if(PhotonNetwork.IsMasterClient)
            GetComponent<PhotonView>().RPC(nameof(InitGame), RpcTarget.All);
    }

    [PunRPC]
    void InitGame()
    {
        MultiServiceMidiator.Instance.Init();
        Managers.Unit.Init(new UnitControllerAttacher().AttacherUnitController(MultiServiceMidiator.Instance.gameObject), Managers.Data);
        new WorldInitializer(gameObject).Init();

        CreatePools();
        gameObject.AddComponent<UnitClickController>();
        gameObject.AddComponent<RewradController>();
    }

    void CreatePools()
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        new UnitPoolInitializer().InitPool();
        new MonsterPoolInitializer().InitPool();
        new WeaponPoolInitializer().InitPool();
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

        InitMonoBehaviourContainer();
        Managers.Camera.EnterBattleScene();
        Managers.Pool.Init();
        InitEffect();
        BindUnitEvent();
    }

    void BindUnitEvent()
    {
        Multi_SpawnManagers.NormalUnit.OnSpawn += Managers.Unit.AddUnit;
        Managers.Unit.OnCombine += new UnitPassiveController().AddYellowSwordmanCombineGold;
    }

    void InitMonoBehaviourContainer()
    {
        _battleDIContainer.AddService<UnitColorChangerRpcHandler>();
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

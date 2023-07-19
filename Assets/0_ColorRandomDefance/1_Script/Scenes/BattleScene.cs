using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BattleScene : BaseScene
{
    BattleDIContainer _battleDIContainer;
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
        SendSkillData();
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            SetEnemyData(SkillType.None, 0, SkillType.None, 0);
            InitGame();
        }
        else
            StartCoroutine(Co_InitGame());
    }

    void SendSkillData()
    {
        var client = Managers.ClientData;
        var main = Managers.ClientData.EquipSkillManager.MainSkill;
        var sub = Managers.ClientData.EquipSkillManager.SubSkill;
        GetComponent<PhotonView>().RPC(nameof(SetEnemyData), RpcTarget.Others, main, client.GetSkillLevel(main), sub, client.GetSkillLevel(sub));
    }

    public BattleDIContainer GetBattleContainer() => _battleDIContainer;

    ActiveUserSkillDataContainer _activeUserSkillDataContainer;
    [PunRPC] 
    void SetEnemyData(SkillType mainSkill, byte mainLevel, SkillType subSkill, byte subLevel)
    {
        _activeUserSkillDataContainer = new ActiveUserSkillDataContainer(mainSkill, mainLevel, subSkill, subLevel, Managers.Data);
    }

    IEnumerator Co_InitGame()
    {
        yield return new WaitUntil(() => _activeUserSkillDataContainer != null);
        InitGame();
    }
    void InitGame()
    {
        MultiServiceMidiator.Instance.Init();
        _battleDIContainer = new BattleDIContainer(gameObject);
        new WorldInitializer(_battleDIContainer).Init(_activeUserSkillDataContainer);
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
        _battleDIContainer = new BattleDIContainer(go);
    }

    public WorldInitializer(BattleDIContainer container)
    {
        _battleDIContainer = container;
    }

    public BattleDIContainer Init(ActiveUserSkillDataContainer data)
    {
        new BattleDIContainerInitializer().InjectBattleDependency(_battleDIContainer, data);

        Managers.Camera.EnterBattleScene();
        InitMonoBehaviourContainer();
        InitObjectPools();
        BindUnitEvent();
        InitEffect();
        return _battleDIContainer;
    }

    void InitMonoBehaviourContainer()
    {
        _battleDIContainer.AddComponent<UnitClickController>();
        _battleDIContainer.AddComponent<RewradController>();
        _battleDIContainer.AddComponent<UnitColorChangerRpcHandler>();
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

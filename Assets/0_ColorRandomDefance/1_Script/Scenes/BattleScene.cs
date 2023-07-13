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
        GetComponent<PhotonView>().RPC(nameof(SetEnemyData), RpcTarget.Others, Managers.ClientData.EquipSkillManager.MainSkill, Managers.ClientData.EquipSkillManager.SubSkill);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            InitGame();
        else
            StartCoroutine(Co_InitGame());
    }

    public BattleDIContainer GetBattleContainer() => _battleDIContainer;

    EquipSkillData _equipSkillData = null;
    [PunRPC] void SetEnemyData(SkillType mainSkill, SkillType subSkill) => _equipSkillData = new EquipSkillData(mainSkill, subSkill);

    IEnumerator Co_InitGame()
    {
        yield return new WaitUntil(() => _equipSkillData != null);
        InitGame();
    }
    void InitGame()
    {
        MultiServiceMidiator.Instance.Init();
        _battleDIContainer = new BattleDIContainer(gameObject);
        new WorldInitializer(_battleDIContainer).Init(_equipSkillData);
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

    public void Init(EquipSkillData enemySKillData)
    {
        new BattleDIContainerInitializer().InjectionBattleDependency(_battleDIContainer, enemySKillData);

        Managers.Camera.EnterBattleScene();
        InitMonoBehaviourContainer();
        InitObjectPools();
        BindUnitEvent();
        InitEffect();
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

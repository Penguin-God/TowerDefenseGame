using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.IO;
using System.Net.NetworkInformation;

public class BattleDIContainer
{
    GameObject _componenentContainer;
    public BattleDIContainer(GameObject componenentContainer) => _componenentContainer = componenentContainer;
    public T AddComponent<T>() where T : MonoBehaviour => _componenentContainer.AddComponent<T>();
    public T GetComponent<T>() => _componenentContainer.GetComponent<T>();

    Dictionary<Type, object> _services = new Dictionary<Type, object>();
    public T AddService<T>() where T : class, new() => AddService(new T());
    public T AddService<T>(T instance) where T : class
    {
        if (_services.ContainsKey(typeof(T)) == false)
            _services[typeof(T)] = instance;

        return _services[typeof(T)] as T;
    }

    public T GetService<T>() where T : class
    {
        if (_services.ContainsKey(typeof(T)))
            return _services[typeof(T)] as T;

        throw new InvalidOperationException("Service of type " + typeof(T).Name + " not found.");
    }

    public MultiData<SkillBattleDataContainer> GetMultiActiveSkillData() => GetService<MultiData<SkillBattleDataContainer>>();
    public SkillBattleDataContainer GetSkillData(byte id) => GetMultiActiveSkillData().GetData(id);
    public BattleEventDispatcher GetEventDispatcher() => GetService<BattleEventDispatcher>();
}

public class BattleDIContainerInitializer
{
    Multi_GameManager game;
    DataManager data;
    BattleEventDispatcher dispatcher;
    public void InjectBattleDependency(BattleDIContainer container, MultiData<SkillBattleDataContainer> multiSKillData)
    {
        game = Multi_GameManager.Instance;
        data = Managers.Data;
        dispatcher = container.AddService<BattleEventDispatcher>();
        container.AddService(multiSKillData);

        AddService(container);
        InjectService(container);

        InitManagers(container);
        InjectionOnlyMaster(container);

        container.GetComponent<EffectInitializer>().SettingEffect(new UserSkillInitializer().InitUserSkill(container, multiSKillData.GetData(PlayerIdManager.Id)));
        Done(container); // 꼭 마지막에 해야 하는 것들
    }

    void AddService(BattleDIContainer container)
    {
        AddMultiService<SwordmanGachaController, MasterSwordmanGachaController>(container);
        new MultiServiceAttacher().AttacherUnitController(container);
        container.AddComponent<CurrencyManagerMediator>();
        container.AddComponent<UnitMaxCountController>();
        container.AddComponent<MonsterManagerProxy>();
        container.AddComponent<WinOrLossController>();
        container.AddComponent<EffectInitializer>();
        container.AddComponent<OpponentStatusSender>();
        container.AddComponent<EnemySpawnNumManager>();
        container.AddComponent<MeteorController>();
        container.AddComponent<MultiEffectManager>();
        container.AddComponent<BuildingClickContoller>();

        container.AddService(new BattleUI_Mediator(Managers.UI));
    }

    void InjectService(BattleDIContainer container)
    {
        container.GetComponent<WinOrLossController>().Init(dispatcher);
        container.GetComponent<OpponentStatusSender>().Init(dispatcher);
        container.GetComponent<SwordmanGachaController>().Init(game, data.BattleDataContainer.UnitSummonData);
        container.GetComponent<CurrencyManagerMediator>().Init(game);
        container.GetComponent<UnitMaxCountController>().Init(null, game);
        container.GetComponent<MonsterManagerProxy>().Init(dispatcher);
        container.GetComponent<MultiEffectManager>().Inject(Managers.Effect);
        container.GetComponent<BuildingClickContoller>().Injection(container.GetService<BattleUI_Mediator>(), Managers.UI);

        new UnitCombineNotifier(Managers.UI).Init(Managers.Unit);
    }


    void InjectionOnlyMaster(BattleDIContainer container)
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        var server = MultiServiceMidiator.Server;

        container.AddComponent<MonsterSpawnerContorller>()
            .Injection(container.GetComponent<IMonsterManager>(), container.GetComponent<EnemySpawnNumManager>(), dispatcher, new SpeedManagerCreater(container));

        container.GetComponent<MasterSwordmanGachaController>().Init(server, container.GetComponent<CurrencyManagerMediator>(), data.BattleDataContainer.UnitSummonData);
        container.GetComponent<UnitMaxCountController>().Init(server, game);
        Multi_SpawnManagers.NormalUnit.Injection(container.GetComponent<MonsterManagerProxy>().MultiMonsterManager, container.GetMultiActiveSkillData());
    }

    void InitManagers(BattleDIContainer container)
    {
        Multi_SpawnManagers.Instance.Init();
        InitSound();
        Init_UI(container);
        game.Init(container.GetComponent<CurrencyManagerMediator>(), container.GetComponent<UnitMaxCountController>(), data.BattleDataContainer, dispatcher);
        StageManager.Instance.Injection(dispatcher);
        Managers.Unit.Init(container.GetComponent<UnitController>(), data.CombineConditionByUnitFalg);
    }

    void Init_UI(BattleDIContainer container)
    {
        Managers.UI.ShowSceneUI<BattleButton_UI>().SetInfo(container.GetComponent<SwordmanGachaController>());
        Managers.UI.ShowSceneUI<UI_Status>().Injection(container.GetService<BattleEventDispatcher>(), container.GetMultiActiveSkillData());
        var enemySelector = Managers.UI.ShowSceneUI<UI_EnemySelector>();
        enemySelector.SetInfo(container.GetComponent<EnemySpawnNumManager>());

        var uiMediator = container.GetService<BattleUI_Mediator>();
        uiMediator.RegisterUI(BattleUI_Type.UnitUpgrdeShop, "InGameShop/UI_UnitUpgradeShop");
        uiMediator.RegisterUI(BattleUI_Type.WhiteUnitShop, "InGameShop/WhiteUnitShop");
        uiMediator.RegisterUI(BattleUI_Type.BalckUnitCombineTable, "InGameShop/BlackUnitShop");
        uiMediator.RegisterUI(BattleUI_Type.UnitMaxCountExpendShop, "InGameShop/UnitCountExpendShop_UI");
    }

    void InitSound()
    {
        var sound = Managers.Sound;
        Managers.Sound.PlayBgm(BgmType.Default);

        Multi_SpawnManagers.BossEnemy.rpcOnSpawn += () => sound.PlayBgm(BgmType.Boss);
        Multi_SpawnManagers.BossEnemy.rpcOnDead += () => sound.PlayBgm(BgmType.Default);

        Multi_SpawnManagers.BossEnemy.rpcOnDead += () => sound.PlayEffect(EffectSoundType.BossDeadClip);
        Multi_SpawnManagers.TowerEnemy.rpcOnDead += () => sound.PlayEffect(EffectSoundType.TowerDieClip);

        StageManager.Instance.OnUpdateStage += (stage) => sound.PlayEffect(EffectSoundType.NewStageClip);
    }

    void Done(BattleDIContainer container)
    {
        container.AddComponent<BattleReadyController>().EnterBattle(container.GetComponent<EnemySpawnNumManager>(), container.GetService<BattleEventDispatcher>());
    }

    void AddMultiService<TClient, TMaster> (BattleDIContainer container) where TClient : MonoBehaviour where TMaster : MonoBehaviour
    {
        if (PhotonNetwork.IsMasterClient)
            container.AddComponent<TMaster>();
        else
            container.AddComponent<TClient>();
    }
}

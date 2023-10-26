using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Linq;

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
    public BattleEventDispatcher GetEventDispatcher() => GetService<BattleEventDispatcher>();
    public Multi_NormalUnitSpawner GetUnitSpanwer() => GetComponent<Multi_NormalUnitSpawner>();
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

        InitSkill(container);
    }

    void AddService(BattleDIContainer container)
    {
        AddMultiService<SwordmanGachaController, MasterSwordmanGachaController>(container);
        new MultiServiceAttacher().AttacherUnitController(container);
        container.AddComponent<CurrencyManagerMediator>();
        container.AddComponent<UnitMaxCountController>();
        container.AddComponent<EnemySpawnNumManager>();
        container.AddComponent<MultiEffectManager>();
        container.AddComponent<TextShowAndHideController>();
        container.AddComponent<NormalMonsterSpawner>();
        container.AddComponent<Multi_BossEnemySpawner>();
        container.AddComponent<Multi_NormalUnitSpawner>();
        container.AddComponent<BattleRewardHandler>();
        container.AddComponent<WorldAudioPlayer>();
        container.AddComponent<MultiUnitStatController>();
        container.AddComponent<MeteorController>();
        container.AddComponent<UnitColorChangerRpcHandler>();

        container.AddService(new UnitManagerController(dispatcher));
        container.AddService(new UnitStatController(CreateUnitStatManager(), container.GetService<UnitManagerController>().WorldUnitManager));
        container.AddService(new BattleUI_Mediator(Managers.UI, container));
        container.AddService(new BuyAction(container.GetUnitSpanwer(), container.GetComponent<MultiUnitStatController>()));
        container.AddService(new GoodsBuyController(game, container.GetComponent<TextShowAndHideController>()));
        container.AddService(new MonsterManagerController(dispatcher));
    }

    void InjectService(BattleDIContainer container)
    {
        container.GetComponent<SwordmanGachaController>().Init(game, data.BattleDataContainer.UnitSummonData);
        container.GetComponent<CurrencyManagerMediator>().Init(game);
        container.GetComponent<UnitMaxCountController>().Init(null, game);
        container.GetComponent<MultiEffectManager>().Inject(Managers.Effect);
        container.GetComponent<TextShowAndHideController>().Inject(Managers.UI);
        container.GetComponent<NormalMonsterSpawner>().Inject(new MonsterDecorator(container), container.GetService<MonsterManagerController>());
        container.GetComponent<Multi_BossEnemySpawner>().Inject(new MonsterDecorator(container));
        container.GetComponent<BattleRewardHandler>()
            .Inject(container.GetEventDispatcher(), container.GetComponent<Multi_BossEnemySpawner>(), new StageUpGoldRewardCalculator(data.BattleDataContainer.StageUpGold));
        container.GetComponent<WorldAudioPlayer>().ReceiveInject(Managers.Camera, Managers.Sound);
        container.GetComponent<MultiUnitStatController>().DependencyInject(container.GetService<UnitStatController>());
        container.GetComponent<MeteorController>().DepencyInject(container.GetComponent<WorldAudioPlayer>());
        container.GetComponent<UnitColorChangerRpcHandler>().DependencyInject(container.GetUnitSpanwer());
    }

    WorldUnitDamageManager CreateUnitStatManager()
    {
        var multiUnitStat = new MultiData<UnitDamageInfoManager>(() => new UnitDamageInfoManager(Managers.Data.Unit.DamageInfoByFlag));
        return new WorldUnitDamageManager(multiUnitStat);
    }

    void InitManagers(BattleDIContainer container)
    {
        Multi_SpawnManagers.Instance.Init();
        InitSound(container);
        Init_UI(container);
        game.Init(container.GetComponent<CurrencyManagerMediator>(), container.GetComponent<UnitMaxCountController>(), data.BattleDataContainer, dispatcher);
        StageManager.Instance.Injection(dispatcher);
        Managers.Unit.Inject(container.GetComponent<UnitCombiner>(), new UnitCombineSystem(data.CombineConditionByUnitFalg));
        // 지금 컨트롤러랑 싱글턴 병행 중
        Multi_SpawnManagers.NormalUnit.ReceiveInject(container);
        container.GetUnitSpanwer().ReceiveInject(container);
    }

    void InjectionOnlyMaster(BattleDIContainer container)
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        var server = MultiServiceMidiator.Server;
        container.AddComponent<MonsterSpawnerContorller>().Inject(container);

        container.GetComponent<MasterSwordmanGachaController>().Init(server, container.GetComponent<CurrencyManagerMediator>(), data.BattleDataContainer.UnitSummonData, container.GetUnitSpanwer());
        container.GetComponent<UnitMaxCountController>().Init(server, game);
    }

    void Init_UI(BattleDIContainer container)
    {
        Managers.UI.ShowSceneUI<UI_Status>().Injection(container.GetService<BattleEventDispatcher>(), container.GetMultiActiveSkillData());
        var enemySelector = Managers.UI.ShowSceneUI<UI_EnemySelector>();
        enemySelector.SetInfo(container.GetComponent<EnemySpawnNumManager>());

        // 절대 여기서 Show를 해서는 안 되!! 이유는 skill에서 바꿀 수도 있음
        var uiMediator = container.GetService<BattleUI_Mediator>();
        uiMediator.RegisterUI(BattleUI_Type.UnitUpgrdeShop, "InGameShop/UI_BattleShop");
        uiMediator.RegisterUI(BattleUI_Type.WhiteUnitShop, "InGameShop/WhiteUnitShop");
        uiMediator.RegisterUI(BattleUI_Type.BalckUnitCombineTable, "InGameShop/BlackUnitShop");
        uiMediator.RegisterUI(BattleUI_Type.UnitMaxCountExpendShop, "InGameShop/UnitCountExpendShop_UI");
        uiMediator.RegisterUI<UI_BattleButtons>(BattleUI_Type.BattleButtons);
    }

    void InitSound(BattleDIContainer container)
    {
        var sound = Managers.Sound;
        Managers.Sound.PlayBgm(BgmType.Default);
        Multi_SpawnManagers.TowerEnemy.rpcOnDead += () => sound.PlayEffect(EffectSoundType.TowerDieClip);

        dispatcher.OnStageUp += (stage) => sound.PlayEffect(EffectSoundType.NewStageClip);
    }

    void InitSkill(BattleDIContainer container)
    {
        var skillInitializer = new UserSkillInitializer();

        MultiData<SkillBattleDataContainer> multiSkllData = container.GetMultiActiveSkillData();
        var allPlayerSkillTypes = multiSkllData.Services.SelectMany(skillData => skillData.AllSKills).Distinct();
        foreach (var skillType in allPlayerSkillTypes)
            skillInitializer.AddSkillDependency(container, skillType);

        var mySkills = skillInitializer.InitUserSkill(container, multiSkllData.GetData(PlayerIdManager.Id));
        container.AddComponent<EffectInitializer>().SettingEffect(mySkills, container.GetEventDispatcher());
    }

    void AddMultiService<TClient, TMaster> (BattleDIContainer container) where TClient : MonoBehaviour where TMaster : MonoBehaviour
    {
        if (PhotonNetwork.IsMasterClient)
            container.AddComponent<TMaster>();
        else
            container.AddComponent<TClient>();
    }
}
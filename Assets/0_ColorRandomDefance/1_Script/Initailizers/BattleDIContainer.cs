using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Linq;
using System.Reflection;

public class BattleDIContainer
{
    GameObject _componenentContainer;
    public BattleDIContainer(GameObject componenentContainer) => _componenentContainer = componenentContainer;
    public T AddComponent<T>() where T : MonoBehaviour => _componenentContainer.AddComponent<T>();
    public T GetComponent<T>() => _componenentContainer.GetComponent<T>();
    public object GetComponent(Type type) => _componenentContainer.GetComponent(type.Name);

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

    public object GetService(Type type)
    {
        if (_services.ContainsKey(type))
            return _services[type];

        throw new InvalidOperationException("Service of type " + type.Name + " not found.");
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
    BattleDIContainer _container;
    public void InjectBattleDependency(BattleDIContainer container, MultiData<SkillBattleDataContainer> multiSKillData)
    {
        _container = container;
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

    void Add<T>() where T : MonoBehaviour => _container.AddComponent<T>();
    void Add<T>(T service) where T : class => _container.AddService(service);

    T Get<T>() where T : class
    {
        if (typeof(T).IsInterface)
        {
            Debug.LogError("인터페이스는 GetComponent 써야 함");
            return null;
        }

        if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T)))
            return _container.GetComponent<T>();
        else
            return _container.GetService<T>();
    }

    void AddService(BattleDIContainer container)
    {
        AddMultiService<SwordmanGachaController, MasterSwordmanGachaController>(container);
        container.AddComponent<CurrencyManagerMediator>();
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
        Add<MultiBattleDataController>();
        Add<UnitCombineMultiController>();

        Add(new UnitCombineSystem(data.CombineConditionByUnitFalg));
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
        container.GetComponent<MultiEffectManager>().Inject(Managers.Effect);
        container.GetComponent<WorldAudioPlayer>().DependencyInject(Managers.Camera, Managers.Sound);
        container.GetComponent<BattleRewardHandler>()
            .DependencyInject(container.GetEventDispatcher(), container.GetComponent<Multi_BossEnemySpawner>(), new StageUpGoldRewardCalculator(data.BattleDataContainer.StageUpGold));
        container.GetComponent<NormalMonsterSpawner>().DependencyInject(new MonsterDecorator(container), container.GetService<MonsterManagerController>());
        container.GetComponent<Multi_BossEnemySpawner>().DependencyInject(new MonsterDecorator(container));

        Inject<MultiUnitStatController>();
        Inject<MeteorController>();
        Inject<UnitColorChangerRpcHandler>();
        Inject<UnitCombineMultiController>();
    }

    object Get(Type type)
    {
        if (type.IsInterface)
        {
            Debug.LogError("인터페이스는 GetComponent 써야 함");
            return null;
        }

        if (typeof(MonoBehaviour).IsAssignableFrom(type))
            return _container.GetComponent(type);
        else
            return _container.GetService(type);
    }

    public void Inject<T>() => Inject(typeof(T));
    public void Inject(Type type)
    {
        object instance = Get(type);
        var methodInfo = instance.GetType().GetMethod("DependencyInject", BindingFlags.Instance | BindingFlags.Public);

        if (methodInfo == null)
        {
            Debug.Log("null임");
            return;
        }
        
        var parameters = methodInfo.GetParameters();
        object[] args = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
            args[i] = Get(parameters[i].ParameterType);
        methodInfo.Invoke(instance, args);
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
        game.Init(container.GetComponent<CurrencyManagerMediator>(), Get<MultiBattleDataController>(), data.BattleDataContainer, dispatcher);
        StageManager.Instance.Injection(dispatcher);
        Managers.Unit.Inject(new UnitCombineSystem(data.CombineConditionByUnitFalg));
        // 지금 컨트롤러랑 싱글턴 병행 중
        Multi_SpawnManagers.NormalUnit.ReceiveInject(container);
        container.GetUnitSpanwer().ReceiveInject(container);
    }

    void InjectionOnlyMaster(BattleDIContainer container)
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        container.AddComponent<MonsterSpawnerContorller>().Inject(container);
        Get<MasterSwordmanGachaController>().Init(Get<MultiBattleDataController>(), Get<UnitManagerController>().WorldUnitManager, Get<CurrencyManagerMediator>(), data.BattleDataContainer.UnitSummonData, container.GetUnitSpanwer());
    }

    void Init_UI(BattleDIContainer container)
    {
        Managers.UI.ShowSceneUI<UI_Status>().Injection(container.GetService<BattleEventDispatcher>(), container.GetMultiActiveSkillData());
        var enemySelector = Managers.UI.ShowSceneUI<UI_EnemySelector>();
        enemySelector.SetInfo(container.GetComponent<EnemySpawnNumManager>());

        var uiMediator = container.GetService<BattleUI_Mediator>();
        
        uiMediator.RegisterUI(BattleUI_Type.WhiteUnitShop, "InGameShop/WhiteUnitShop");
        uiMediator.RegisterUI(BattleUI_Type.BalckUnitCombineTable, "InGameShop/BlackUnitShop");

        var shop = uiMediator.ShowPopupUI(BattleUI_Type.BalckUnitCombineTable).GetComponentInChildren<BalckUnitShop_UI>();
        shop.DependencyInject(Get<UnitCombineMultiController>());
        shop.transform.parent.gameObject.SetActive(false);

        uiMediator.RegisterUI(BattleUI_Type.UnitMaxCountExpendShop, "InGameShop/UnitCountExpendShop_UI");

        Managers.UI.ShowPopupUI<UI_UnitManagedWindow>("UnitManagedWindow").DepencyInject(Get<UnitCombineMultiController>());
        Managers.UI.ShowPopupUI<UI_UnitManagedWindow>("UnitManagedWindow").gameObject.SetActive(false);

        // 얘들은 절대 여기서 Show를 해서는 안 되!! 이유는 skill에서 바꿀 수도 있음
        uiMediator.RegisterUI(BattleUI_Type.UnitUpgrdeShop, "InGameShop/UI_BattleShop");
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
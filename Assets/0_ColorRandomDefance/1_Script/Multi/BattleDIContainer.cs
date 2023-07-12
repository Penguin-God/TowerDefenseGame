using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

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
}

public class MultiInitializer
{
    Multi_GameManager game;
    DataManager data;
    BattleEventDispatcher dispatcher;
    public void InjectionBattleDependency(BattleDIContainer container)
    {
        game = Multi_GameManager.Instance;
        data = Managers.Data;
        dispatcher = container.AddService<BattleEventDispatcher>();

        AddComponent(container);
        InitComponent(container);
        Multi_SpawnManagers.Instance.Init();

        InjectionOnlyMaster(container);

        InitManagers(container);
        container.GetComponent<EffectInitializer>().SettingEffect(new UserSkillInitializer().InitUserSkill(container));
        Done(container); // 꼭 마지막에 해야 하는 것들
    }

    void AddComponent(BattleDIContainer container)
    {
        AddMultiService<SwordmanGachaController, MasterSwordmanGachaController>(container);
        container.AddComponent<CurrencyManagerMediator>();
        container.AddComponent<UnitMaxCountController>();
        container.AddComponent<MonsterManagerProxy>();
        container.AddComponent<WinOrLossController>();
        container.AddComponent<EffectInitializer>();
        container.AddComponent<OpponentStatusSender>();
        container.AddComponent<EnemySpawnNumManager>();
        container.AddComponent<MeteorController>();
        container.AddComponent<EffectSynchronizer>();
    }


    void InitComponent(BattleDIContainer container)
    {
        container.GetComponent<WinOrLossController>().Init(dispatcher);
        container.GetComponent<OpponentStatusSender>().Init(dispatcher);
        container.GetComponent<SwordmanGachaController>().Init(game, data.BattleDataContainer.UnitSummonData);
        container.GetComponent<CurrencyManagerMediator>().Init(game);
        container.GetComponent<UnitMaxCountController>().Init(null, game);
        container.GetComponent<MonsterManagerProxy>().Init(dispatcher);
    }


    void InjectionOnlyMaster(BattleDIContainer container)
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        var server = MultiServiceMidiator.Server;

        var monsterSpawnController = container.AddComponent<MonsterSpawnerContorller>();
        monsterSpawnController.Injection(container.GetComponent<IMonsterManager>(), container.GetComponent<EnemySpawnNumManager>(), dispatcher);
        container.GetComponent<MasterSwordmanGachaController>().Init(server, container.GetComponent<CurrencyManagerMediator>(), data.BattleDataContainer.UnitSummonData);
        container.GetComponent<UnitMaxCountController>().Init(server, game);
        Multi_SpawnManagers.NormalUnit.Init(container.GetComponent<MonsterManagerProxy>().MultiMonsterManager);
    }

    void InitManagers(BattleDIContainer container)
    {
        InitSound();
        Init_UI(container);
        game.Init(container.GetComponent<CurrencyManagerMediator>(), container.GetComponent<UnitMaxCountController>(), data.BattleDataContainer);
        StageManager.Instance.Injection(dispatcher);
        // TODO : 유닛 매니저 Init 여기로 옮기기
    }

    void Init_UI(BattleDIContainer container)
    {
        Managers.UI.ShowPopupUI<CombineResultText>("CombineResultText");

        Managers.UI.ShowSceneUI<BattleButton_UI>().SetInfo(container.GetComponent<SwordmanGachaController>());
        Managers.UI.ShowSceneUI<UI_Status>().SetInfo(container.GetService<BattleEventDispatcher>());

        var enemySelector = Managers.UI.ShowSceneUI<UI_EnemySelector>();
        enemySelector.SetInfo(container.GetComponent<EnemySpawnNumManager>());
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

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
    public void InjectionBattleDependency(BattleDIContainer container)
    {
        var game = Multi_GameManager.Instance;
        var data = Managers.Data;
        var dispatcher = container.AddService<BattleEventDispatcher>();

        // add
        AddMultiService<SwordmanGachaController, MasterSwordmanGachaController>(container);
        container.AddComponent<CurrencyManagerMediator>();
        container.AddComponent<UnitMaxCountController>();
        IMonsterManager monsterManagerProxy = container.AddComponent<MonsterManagerProxy>();
        container.AddComponent<WinOrLossController>().Init(dispatcher);
        container.AddComponent<EffectInitializer>();
        container.AddComponent<OpponentStatusSender>().Init(dispatcher);
        container.AddComponent<EnemySpawnNumManager>();
        container.AddComponent<MeteorController>();
        container.AddComponent<EffectSynchronizer>();
        
        // set
        container.GetComponent<SwordmanGachaController>().Init(game, data.BattleDataContainer.UnitSummonData);
        container.GetComponent<CurrencyManagerMediator>().Init(game);
        container.GetComponent<UnitMaxCountController>().Init(null, game);
        container.GetComponent<MonsterManagerProxy>().Init(dispatcher);

        Multi_SpawnManagers.Instance.Init();

        if (PhotonNetwork.IsMasterClient)
        {
            var server = MultiServiceMidiator.Server;
            var monsterSpawnController = container.AddComponent<MonsterSpawnerContorller>();

            monsterSpawnController.Injection(monsterManagerProxy, container.GetComponent<EnemySpawnNumManager>(), dispatcher);
            container.GetComponent<MasterSwordmanGachaController>().Init(server, container.GetComponent<CurrencyManagerMediator>(), data.BattleDataContainer.UnitSummonData);
            container.GetComponent<UnitMaxCountController>().Init(server, game);
            Multi_SpawnManagers.NormalUnit.Init(container.GetComponent<MonsterManagerProxy>().MultiMonsterManager);
        }

        InitSound();
        Init_UI(container);
        game.Init(container.GetComponent<CurrencyManagerMediator>(), container.GetComponent<UnitMaxCountController>(), data.BattleDataContainer);
        StageManager.Instance.Injection(dispatcher);
        container.GetComponent<EffectInitializer>().SettingEffect(new UserSkillInitializer().InitUserSkill(container));
        Done(container);
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

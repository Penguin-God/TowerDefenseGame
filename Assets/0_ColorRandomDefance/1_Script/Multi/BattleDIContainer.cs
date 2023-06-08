using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.ComponentModel;
using Codice.Client.Common;

public class BattleDIContainer : MonoBehaviourPun
{
    public T AddService<T>() where T : MonoBehaviour => gameObject.AddComponent<T>();
    public T GetService<T>() => GetComponent<T>();
}

public class MultiInitializer
{
    BattleEventDispatcher _dispatcher = new BattleEventDispatcher();

    public void InjectionBattleDependency(BattleDIContainer container)
    {
        var game = Multi_GameManager.Instance;
        var data = Managers.Data;
        // add
        AddMultiService<SwordmanGachaController, MasterSwordmanGachaController>(container);
        container.AddService<CurrencyManagerMediator>();
        container.AddService<UnitMaxCountController>();
        IMonsterManager monsterManagerProxy = container.AddService<MonsterManagerProxy>();
        container.AddService<WinOrLossController>().Init(_dispatcher);
        container.AddService<EffectInitializer>();
        container.AddService<OpponentStatusSender>().Init(_dispatcher);
        container.AddService<EnemySpawnNumManager>();

        // set
        container.GetService<SwordmanGachaController>().Init(game, data.BattleDataContainer.UnitSummonData);
        container.GetService<CurrencyManagerMediator>().Init(game);
        container.GetService<UnitMaxCountController>().Init(null, game);
        container.GetService<MonsterManagerProxy>().Init(_dispatcher);

        Multi_SpawnManagers.Instance.Init();

        if (PhotonNetwork.IsMasterClient)
        {
            var server = MultiServiceMidiator.Server;
            var monsterSpawnController = container.AddService<MonsterSpawnerContorller>();

            monsterSpawnController.Init(monsterManagerProxy, _dispatcher);
            container.GetService<MasterSwordmanGachaController>().Init(server, container.GetService<CurrencyManagerMediator>(), data.BattleDataContainer.UnitSummonData);
            container.GetService<UnitMaxCountController>().Init(server, game);
            Multi_SpawnManagers.NormalUnit.Init(container.GetService<MonsterManagerProxy>().MultiMonsterManager);
        }

        Init_UI(container);
        game.Init(container.GetService<CurrencyManagerMediator>(), container.GetService<UnitMaxCountController>(), data.BattleDataContainer);
        container.GetService<EffectInitializer>().SettingEffect(new UserSkillInitializer().InitUserSkill(container));
        Done(container);
    }

    void Init_UI(BattleDIContainer container)
    {
        Managers.UI.ShowPopupUI<CombineResultText>("CombineResultText");

        Managers.UI.ShowSceneUI<BattleButton_UI>().SetInfo(container.GetService<SwordmanGachaController>());
        Managers.UI.ShowSceneUI<UI_Status>().SetInfo(_dispatcher);

        var enemySelector = Managers.UI.ShowSceneUI<UI_EnemySelector>();
        enemySelector.SetInfo(container.GetService<EnemySpawnNumManager>());
        Managers.Camera.OnIsLookMyWolrd += (isLookMy) => enemySelector.gameObject.SetActive(!isLookMy);
    }

    void Done(BattleDIContainer container)
    {
        container.AddService<BattleReadyController>().SetInfo(container.GetService<EnemySpawnNumManager>(), _dispatcher);
    }

    void AddMultiService<TClient, TMaster> (BattleDIContainer container) where TClient : MonoBehaviour where TMaster : MonoBehaviour
    {
        if (PhotonNetwork.IsMasterClient)
            container.AddService<TMaster>();
        else
            container.AddService<TClient>();
    }
}

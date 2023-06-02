using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BattleDIContainer : MonoBehaviourPun
{
    public T AddService<T>() where T : MonoBehaviour => gameObject.AddComponent<T>();
    public T GetService<T>() => GetComponent<T>();
}

public class MultiInitializer
{
    BattleEventDispatcher dispatcher = new BattleEventDispatcher();

    public void InjectionBattleDependency(BattleDIContainer container)
    {
        // add
        AddMultiService<SwordmanGachaController, MasterSwordmanGachaController>(container);
        container.AddService<CurrencyManagerMediator>();
        container.AddService<UnitMaxCountController>();
        IMonsterManager monsterManagerProxy = container.AddService<MonsterManagerProxy>();
        container.AddService<WinOrLossController>().Init(dispatcher);

        // set
        container.GetService<SwordmanGachaController>().Init(Multi_GameManager.Instance, container.GetService<IBattleCurrencyManager>());
        container.GetService<CurrencyManagerMediator>().Init(Multi_GameManager.Instance);
        container.GetService<UnitMaxCountController>().Init(null, Multi_GameManager.Instance);
        container.GetService<MonsterManagerProxy>().Init(dispatcher);

        Multi_SpawnManagers.Instance.Init();

        if (PhotonNetwork.IsMasterClient)
        {
            var server = MultiServiceMidiator.Server;
            var monsterSpawnController = container.AddService<MonsterSpawnerContorller>();

            monsterSpawnController.Init(monsterManagerProxy);
            container.GetService<MasterSwordmanGachaController>().Init(server, container.GetService<CurrencyManagerMediator>());
            container.GetService<UnitMaxCountController>().Init(server, Multi_GameManager.Instance);
            Multi_SpawnManagers.NormalUnit.Init(container.GetService<MonsterManagerProxy>().MultiMonsterManager);
        }

        Managers.UI.ShowSceneUI<UI_Status>().SetInfo(dispatcher);
        Multi_GameManager.Instance.Init(container.GetService<CurrencyManagerMediator>(), container.GetService<UnitMaxCountController>());
    }

    void AddMultiService<TClient, TMaster> (BattleDIContainer container) where TClient : MonoBehaviour where TMaster : MonoBehaviour
    {
        if (PhotonNetwork.IsMasterClient)
            container.AddService<TMaster>();
        else
            container.AddService<TClient>();
    }
}

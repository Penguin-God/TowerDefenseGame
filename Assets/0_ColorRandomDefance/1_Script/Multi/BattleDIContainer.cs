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
        var game = Multi_GameManager.Instance;
        var data = Managers.Data;
        // add
        AddMultiService<SwordmanGachaController, MasterSwordmanGachaController>(container);
        container.AddService<CurrencyManagerMediator>();
        container.AddService<UnitMaxCountController>();
        IMonsterManager monsterManagerProxy = container.AddService<MonsterManagerProxy>();
        container.AddService<WinOrLossController>().Init(dispatcher);
        container.AddService<EffectInitializer>();

        // set
        container.GetService<SwordmanGachaController>().Init(game, data.BattleDataContainer.UnitSummonData);
        container.GetService<CurrencyManagerMediator>().Init(game);
        container.GetService<UnitMaxCountController>().Init(null, game);
        container.GetService<MonsterManagerProxy>().Init(dispatcher);

        Multi_SpawnManagers.Instance.Init();

        if (PhotonNetwork.IsMasterClient)
        {
            var server = MultiServiceMidiator.Server;
            var monsterSpawnController = container.AddService<MonsterSpawnerContorller>();

            monsterSpawnController.Init(monsterManagerProxy);
            container.GetService<MasterSwordmanGachaController>().Init(server, container.GetService<CurrencyManagerMediator>(), data.BattleDataContainer.UnitSummonData);
            container.GetService<UnitMaxCountController>().Init(server, game);
            Multi_SpawnManagers.NormalUnit.Init(container.GetService<MonsterManagerProxy>().MultiMonsterManager);
        }

        Managers.UI.ShowSceneUI<UI_Status>().SetInfo(dispatcher);
        game.Init(container.GetService<CurrencyManagerMediator>(), container.GetService<UnitMaxCountController>(), data.BattleDataContainer);
        container.GetService<EffectInitializer>().SettingEffect(new UserSkillInitializer().InitUserSkill());
    }

    void AddMultiService<TClient, TMaster> (BattleDIContainer container) where TClient : MonoBehaviour where TMaster : MonoBehaviour
    {
        if (PhotonNetwork.IsMasterClient)
            container.AddService<TMaster>();
        else
            container.AddService<TClient>();
    }
}

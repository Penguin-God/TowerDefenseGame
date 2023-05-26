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
    public void InjectionBattleDependency(BattleDIContainer container)
    {
        AddMultiService<CurrencyManagerProxy, MasterCurrencyManager>(container); 
        AddMultiService<SwordmanGachaController, MasterSwordmanGachaController>(container);

        container.GetService<CurrencyManagerProxy>().Init(new CurrencyManager());
        container.GetService<SwordmanGachaController>().Init(Multi_GameManager.Instance, container.GetService<IBattleCurrencyManager>());

        if (PhotonNetwork.IsMasterClient)
        {
            var server = MultiServiceMidiator.Server;
            container.GetService<MasterCurrencyManager>().Init(server);
            container.GetService<MasterSwordmanGachaController>().Init(server);
        }

        Multi_GameManager.Instance.Init(container.GetService<IBattleCurrencyManager>());
    }

    void AddMultiService<TClient, TMaster> (BattleDIContainer container) where TClient : MonoBehaviour where TMaster : MonoBehaviour
    {
        if (PhotonNetwork.IsMasterClient)
            container.AddService<TMaster>();
        else
            container.AddService<TClient>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiDIContainer : MonoBehaviourPun
{
    public T AddService<T>() where T : MonoBehaviour => gameObject.AddComponent<T>();
    public T GetService<T>() => GetComponent<T>();
}

public class MultiInitializer
{
    public void InjectionMultiDependency(MultiDIContainer multiDIContainer)
    {
        AddService<MasterCurrencyManager, CurrencyManagerProxy>(multiDIContainer);
    }

    void AddService<TMaster, TProxy> (MultiDIContainer container) where TMaster : MonoBehaviour where TProxy : MonoBehaviour
    {
        if (PhotonNetwork.IsMasterClient)
            container.AddService<TMaster>();
        else
            container.AddService<TProxy>();
    }
}

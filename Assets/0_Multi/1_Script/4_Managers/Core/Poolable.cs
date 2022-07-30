using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class Poolable : MonoBehaviourPun
{
    public bool IsUsing;
    public string Path;

    //void Awake()
    //{
    //    Component component = GetSpawnComponent();
    //    if (component != null)
    //        Multi_SpawnManagers.Instance.SpawnerByType[component.GetType()].SettingPoolObject(component);
    //}

    //Component GetSpawnComponent() 
    //    => GetComponents<Component>().FirstOrDefault(x => Multi_SpawnManagers.Instance.SpawnerByType.ContainsKey(x.GetType()));
}

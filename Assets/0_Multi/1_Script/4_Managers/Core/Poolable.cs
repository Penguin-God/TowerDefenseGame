using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Poolable : MonoBehaviour
{
    public bool IsUsing;
    public string Path;

    void Awake()
    {
        Component component = 
            GetComponents<Component>().FirstOrDefault(x => Multi_SpawnManagers.Instance.SpawnerByType.ContainsKey(x.GetType()));

        if (component != null)
        {
            Multi_SpawnerBase ba = Multi_SpawnManagers.Instance.SpawnerByType[component.GetType()];
            print(ba == null);
            if(ba != null)
                Multi_SpawnManagers.Instance.SpawnerByType[component.GetType()].SettingPoolObject(component);
        }


        GetComponents<Component>().ToList().ForEach(x => print(x.GetType().Name));
    }
}

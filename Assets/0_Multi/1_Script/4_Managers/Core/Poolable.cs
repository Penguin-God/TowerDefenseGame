using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class Poolable : MonoBehaviourPun
{
    [SerializeField] int _usingId = -1;
    public int UsingId => _usingId;

    public bool IsUsing;
    public string Path;

    void Awake()
    {
        Component component = GetSpawnComponent();
        if (component != null)
            Multi_SpawnManagers.Instance.SpawnerByType[component.GetType()].SettingPoolObject(component);
    }

    Component GetSpawnComponent() 
        => GetComponents<Component>().FirstOrDefault(x => Multi_SpawnManagers.Instance.SpawnerByType.ContainsKey(x.GetType()));


    public void SetId_RPC(int id) => gameObject.GetComponent<PhotonView>().RPC("_SetId", RpcTarget.All, id);

    [PunRPC]
    void _SetId(int id) => _usingId = id;
}

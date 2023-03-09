using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                    instance = new GameObject(typeof(T).Name).AddComponent<T>();
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    void Awake()
    {
        Init();
    }

    protected virtual void Init() { }
}

public class SingletonPun<T> : Singleton<T> where T : Component 
{
    protected PhotonView photonView;
    protected override void Init()
    {
        photonView = gameObject.GetComponent<PhotonView>();
        if (photonView == null) 
            Debug.LogError($"멀티 singleton {typeof(T).Name}에 photonView가 존재하지 않음");
    }
}

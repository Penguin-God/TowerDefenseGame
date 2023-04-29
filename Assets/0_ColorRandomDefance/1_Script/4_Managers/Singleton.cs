using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Singleton<T> : MonoBehaviour where T : Component
{
    static bool _isQuiiting = false;
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null && _isQuiiting == false)
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

    void OnApplicationQuit()
    {
        instance = null;
        _isQuiiting = true;
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

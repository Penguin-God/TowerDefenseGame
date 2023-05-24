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

    void OnApplicationQuit()
    {
        instance = null;
        _isQuiiting = true;
    }

    public virtual void Init() { }
}

public class SingletonPun<T> : MonoBehaviour where T : Component
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
            }

            return instance;
        }
    }

    protected PhotonView photonView;
    public virtual void Init()
    {
        photonView = gameObject.GetComponent<PhotonView>();
        if (photonView == null) 
            Debug.LogError($"멀티 singleton {typeof(T).Name}에 photonView가 존재하지 않음");
    }
}

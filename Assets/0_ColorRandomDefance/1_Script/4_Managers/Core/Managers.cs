using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Managers : MonoBehaviour
{
    private static Managers instance;
    private static Managers Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Managers>();
                if (instance == null)
                    instance = new GameObject("Multi_Managers").AddComponent<Managers>();

                DontDestroyOnLoad(instance.gameObject);
                instance.Init();
            }

            return instance;
        }
    }

    DataManager _data = new DataManager();
    UI_Manager _ui = new UI_Manager();
    Multi_SoundManager _sound = new Multi_SoundManager();
    ResourcesManager _resources = new ResourcesManager();
    PoolManager _pool = new PoolManager();
    Multi_ClientData _clientData = new Multi_ClientData();
    Scene_Manager _scene = new Scene_Manager();
    CameraManager _camera = new CameraManager();
    EffectManager _effect = new EffectManager();
    MultiManager _multi = new MultiManager();

    public static DataManager Data => Instance._data;
    public static UI_Manager UI => Instance._ui;
    public static Multi_SoundManager Sound => Instance._sound;
    public static ResourcesManager Resources => Instance._resources;
    public static PoolManager Pool => Instance._pool;
    public static Multi_ClientData ClientData => Instance._clientData;
    public static Scene_Manager Scene => instance._scene;
    public static CameraManager Camera => instance._camera;
    public static EffectManager Effect => instance._effect;
    public static MultiManager Multi => instance._multi;


    void Init()
    {
        _data.Init();
        _clientData.Init();
        _sound.Init(transform);
    }

    public static void Clear()
    {
        Camera.Clear();
        Scene.Clear();
        UI.Clear();
    }

    [ContextMenu("LoadScene")]
    void LoadScene()
    {
        PhotonNetwork.LeaveRoom();
        Scene.LoadScene(SceneTyep.클라이언트);
    }
}
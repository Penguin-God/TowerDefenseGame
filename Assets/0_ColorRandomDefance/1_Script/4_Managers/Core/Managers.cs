using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    DataManager _data = new();
    UI_Manager _ui = new();
    Multi_SoundManager _sound = new();
    ResourcesManager _resources = new();
    PoolManager _pool = new();
    Multi_ClientData _clientData = new();
    Scene_Manager _scene = new();
    CameraManager _camera = new();
    EffectManager _effect = new();
    MultiManager _multi = new();
    UnitManager _unit = new();

    public static DataManager Data => Instance._data;
    public static UI_Manager UI => Instance._ui;
    public static Multi_SoundManager Sound => Instance._sound;
    public static ResourcesManager Resources => Instance._resources;
    public static PoolManager Pool => Instance._pool;
    public static Multi_ClientData ClientData => Instance._clientData;
    public static Scene_Manager Scene => Instance._scene;
    public static CameraManager Camera => Instance._camera;
    public static EffectManager Effect => Instance._effect;
    public static MultiManager Multi => Instance._multi;
    public static UnitManager Unit => Instance._unit;


    void Init()
    {
        _data.Init();
        _clientData.Init();
        _sound.Init(transform);
    }

    public static void Clear()
    {
        Unit.Clear();
        Camera.Clear();
        Scene.Clear();
        UI.Clear();
    }
}
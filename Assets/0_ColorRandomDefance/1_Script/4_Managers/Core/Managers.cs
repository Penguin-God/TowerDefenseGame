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
            if (instance == null && Application.isPlaying)
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
    SoundManager _sound = new();
    ResourcesManager _resources = new();
    ClientDataManager _clientData = new();
    Scene_Manager _scene = new();
    CameraController _camera = new();
    EffectManager _effect = new();
    MultiManager _multi = new();
    UnitControllerManager _unit = new();

    public static DataManager Data => Instance._data;
    public static UI_Manager UI => Instance._ui;
    public static SoundManager Sound => Instance._sound;
    public static ResourcesManager Resources => Instance._resources;
    public static ClientDataManager ClientData => Instance._clientData;
    public static Scene_Manager Scene => Instance._scene;
    public static CameraController Camera => Instance._camera;
    public static EffectManager Effect => Instance._effect;
    public static MultiManager Multi => Instance._multi;
    public static UnitControllerManager Unit => Instance._unit;


    void Init()
    {
        _resources.DependencyInject(new PoolManager("@PoolManager"));
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
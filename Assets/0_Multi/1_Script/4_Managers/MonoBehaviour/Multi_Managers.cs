using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Managers : MonoBehaviour
{
    private static Multi_Managers instance;
    private static Multi_Managers Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Multi_Managers>();
                if (instance == null)
                    instance = new GameObject("Multi_Managers").AddComponent<Multi_Managers>();

                DontDestroyOnLoad(instance.gameObject);
                instance.Init();
            }

            return instance;
        }
    }

    Multi_DataManager _data = new Multi_DataManager();
    Multi_UI_Manager _ui = new Multi_UI_Manager();
    Multi_SoundManager _sound = new Multi_SoundManager();
    Multi_ResourcesManager _resources = new Multi_ResourcesManager();
    Multi_PoolManager _pool = new Multi_PoolManager();
    Multi_ClientData _clientData = new Multi_ClientData();
    SkillManager _skill = new SkillManager();
    Scene_Manager _scene = new Scene_Manager();
    CameraManager _camera = new CameraManager();
    EffectManager _effect = new EffectManager();

    public static Multi_DataManager Data => Instance._data;
    public static Multi_UI_Manager UI => Instance._ui;
    public static Multi_SoundManager Sound => Instance._sound;
    public static Multi_ResourcesManager Resources => Instance._resources;
    public static Multi_PoolManager Pool => Instance._pool;
    public static Multi_ClientData ClientData => Instance._clientData;
    public static SkillManager Skill => Instance._skill;
    public static Scene_Manager Scene => instance._scene;
    public static CameraManager Camera => instance._camera;
    public static EffectManager Effect => instance._effect;


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
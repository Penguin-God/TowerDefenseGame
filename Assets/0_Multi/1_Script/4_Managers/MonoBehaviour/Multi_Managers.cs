﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Managers : MonoBehaviourPun
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

    public static Multi_DataManager Data => Instance._data;
    public static Multi_UI_Manager UI => Instance._ui;
    public static Multi_SoundManager Sound => Instance._sound;
    public static Multi_ResourcesManager Resources => Instance._resources;
    public static Multi_PoolManager Pool => Instance._pool;
    public static Multi_ClientData ClientData => Instance._clientData;
    public static SkillManager Skill => Instance._skill;

    void Init()
    {
        print("Init"); // TODO : Scene만들기 init 한번만 하게 하기
        _data.Init();
        _clientData.Init();

        _skill.Init();
        FirstInit();

        _ui.Init();
        _sound.Init();


        if (PhotonNetwork.IsMasterClient == false) return;
        _pool.Init();
    }

    void FirstInit()
    {
        if (ClientData.SkillByType[SkillType.시작골드증가].EquipSkill == true)
        {
            Debug.Log("시작 골드 증가 사용");
        }
        else
        {
            Debug.Log("시작 골드 증가 없음.....");
        }

        if (ClientData.SkillByType[SkillType.시작식량증가].EquipSkill == true)
        {
            Debug.Log("시작 식량 증가 사용");
        }
        else
        {
            Debug.Log("시작 식량 증가 없음.....");
        }

        if (ClientData.SkillByType[SkillType.최대유닛증가].EquipSkill == true)
        {
            Debug.Log("시작 최대 유닛 증가 사용");
        }
        else
        {
            Debug.Log("시작 최대 유닛 증가 없음.....");
        }
    }

    void Start()
    {
        // temp code : 나중에 씬으로 옮길 것
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        _ui.ShowSceneUI<Multi_UI_Paint>("Paint");
    }

    public void Clear()
    {
        _ui.Clear();
    }

    #region Test
    [ContextMenu("Init Data")]
    void DataInit() => _data.Init();
    #endregion
}

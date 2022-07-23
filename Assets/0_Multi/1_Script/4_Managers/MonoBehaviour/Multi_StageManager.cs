using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;

public class Multi_StageManager : MonoBehaviourPun
{
    private static Multi_StageManager instance;
    public static Multi_StageManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Multi_StageManager>();
                if (instance == null) instance = new GameObject("Multi_StageManager").AddComponent<Multi_StageManager>();
            }
            return instance;
        }
    }

    public event Action<int> OnUpdateStage;

    [SerializeField] int currentStage = 0;
    public int CurrentStage => currentStage;

    [SerializeField] Slider timerSlider;
    [SerializeField] float stageTime = 40f;
    WaitForSeconds StageWait;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Multi_GameManager.instance.OnStart += UpdateStage;
        }

        timerSlider.value = 0;
        StageWait = new WaitForSeconds(Multi_SpawnManagers.NormalEnemy.EnemySpawnTime);
    }

    private void Update()
    {
        if(timerSlider.value > 0)
            timerSlider.value -= Time.deltaTime;
    }
    
    void UpdateStage() 
    {
        currentStage += 1;
        photonView.RPC("UpdateStage", RpcTarget.All, currentStage);
    }

    [PunRPC]
    void UpdateStage(int stage) // 무한반복하는 재귀 함수( Co_Stage() 마지막 부분에 다시 NewStageStart()를 호출함)
    {
        currentStage = stage;
        OnUpdateStage?.Invoke(stage);

        timerSlider.maxValue = stageTime;
        timerSlider.value = stageTime;

        StartCoroutine(Co_Stage());
    }


    IEnumerator Co_Stage()
    {
        yield return StageWait;
        yield return new WaitUntil(() => timerSlider.value <= 0); // 스테이지 타이머 0이되면

        if(PhotonNetwork.IsMasterClient)
            UpdateStage();
    }
}

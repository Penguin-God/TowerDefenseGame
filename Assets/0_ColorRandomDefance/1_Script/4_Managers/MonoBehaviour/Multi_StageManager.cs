using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public readonly int STAGE_TIME = 100;
    [SerializeField] int currentStage = 0;
    public int CurrentStage => currentStage;

    WaitForSeconds StageWait;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            Multi_GameManager.instance.OnGameStart += UpdateStage;

        StageWait = new WaitForSeconds(STAGE_TIME);
    }

    void OnDestroy()
    {
        OnUpdateStage = null;
    }

    void UpdateStage() 
    {
        currentStage++;
        photonView.RPC(nameof(UpdateStage), RpcTarget.All, currentStage);
    }

    [PunRPC]
    void UpdateStage(int stage) // 무한반복하는 재귀 함수( Co_Stage() 마지막 부분에 다시 NewStageStart()를 호출함)
    {
        currentStage = stage;
        OnUpdateStage?.Invoke(stage);

        StartCoroutine(Co_Stage());
    }


    IEnumerator Co_Stage()
    {
        yield return StageWait;
        
        if(PhotonNetwork.IsMasterClient)
            UpdateStage();
    }
}

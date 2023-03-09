using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class StageManager : SingletonPun<StageManager>
{
    public event Action<int> OnUpdateStage;
    public readonly float STAGE_TIME = 32.5f;
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

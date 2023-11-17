using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class StageManager : SingletonPun<StageManager>
{
    public event Action<int> OnUpdateStage;
    [SerializeField] int currentStage = 0;
    public int CurrentStage => currentStage;

    WaitForSeconds StageWait;

    BattleEventDispatcher _dispatcher;
    public void Injection(BattleEventDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        SetInfo();
    }

    void Awake()
    {
        base.Init();
    }

    void SetInfo()
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        _dispatcher.OnGameStart += UpdateStage;
        StageWait = new WaitForSeconds(Multi_GameManager.Instance.BattleData.BattleData.StageTime);
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
        _dispatcher.NotifyStageUp(stage);
        StartCoroutine(Co_Stage());
    }

    IEnumerator Co_Stage()
    {
        yield return StageWait;
        
        if(PhotonNetwork.IsMasterClient)
            UpdateStage();
    }
}

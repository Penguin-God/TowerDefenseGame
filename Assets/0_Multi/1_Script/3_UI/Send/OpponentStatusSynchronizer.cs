using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OpponentStatusSynchronizer : MonoBehaviourPun
{
    UI_OpponentStatus _opponentStatus;
    public void Init(UI_OpponentStatus opponentStatus)
    {
        _opponentStatus = opponentStatus;
    }

    void Start() // start에서 해야 이벤트 등록이 정상적으로 됨. ㅈ같은 시간 커플링 같으니라고
    {
        // 이 코드는 전체적으로 문제가 많음. 아마 동기화 관련 매니저를 만들어서 거기서 이벤트를 받는 게 좋을 듯.
        // 물론 당장은 바빠서 이 정도로만 하고 멈추도록 하겠다.
        RequestUpdateUnitMaxCount(15);
        Multi_GameManager.instance.BattleData.OnMaxUnitChanged -= RequestUpdateUnitMaxCount;
        Multi_GameManager.instance.BattleData.OnMaxUnitChanged += RequestUpdateUnitMaxCount;
    }

    void RequestUpdateUnitMaxCount(int count) => photonView.RPC(nameof(UpdateUnitMax), RpcTarget.Others, count);
    [PunRPC]
    void UpdateUnitMax(int count) => _opponentStatus.UpdateUnitMaxCount(count);
}

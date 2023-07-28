using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WhiteUnit : MonoBehaviourPun
{
    [SerializeField] int classNumber;
    [SerializeField] int maxColor = 5;
    [SerializeField] float aliveTime;
    Multi_WhiteUnitTimer timer = null;

    void OnEnable()
    {
        timer = Managers.Effect.TrackingToTarget("WhiteUnitTimer", transform, new Vector3(0, 4, 3)).GetComponent<Multi_WhiteUnitTimer>();
        if (GetComponent<RPCable>().UsingId == PlayerIdManager.Id)
            photonView.RPC(nameof(SetupTimer), RpcTarget.All, Multi_GameManager.Instance.BattleData.BattleData.WhiteUnitTime);

        if (PhotonNetwork.IsMasterClient == false) return;
        timer.Slider.onValueChanged.AddListener(ChangedColor);
    }

    [PunRPC] void SetupTimer(float time) => timer.Setup(transform, time);

    void OnDisable()
    {
        timer.Off();
        timer = null;
    }

    public void ChangedColor(float value)
    {
        if(value <= 0)
            UnitColorChangerRpcHandler.ChangeUnitColor(photonView.ViewID);
    }
}

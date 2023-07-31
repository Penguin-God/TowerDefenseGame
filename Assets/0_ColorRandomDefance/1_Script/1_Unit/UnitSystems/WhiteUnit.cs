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

        if (PhotonNetwork.IsMasterClient)
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
        {
            bool isMasterUnit = PlayerIdManager.IsMasterId(GetComponent<RPCable>().OwnerId);
            UnitFlags previousFlag = GetComponent<Multi_TeamSoldier>().UnitFlags;
            UnitFlags result = UnitColorChangerRpcHandler.ChangeUnitColorWithViewId(photonView.ViewID);
            if (isMasterUnit)
                ShowText(previousFlag, result);
            else
                photonView.RPC(nameof(ShowText), RpcTarget.Others, previousFlag, result);
        }
    }

    [PunRPC]
    void ShowText(UnitFlags previousFlag, UnitFlags changeFlag)
    {
        string text = $"보유 중인 {new UnitColorChangeTextPresenter().GenerateColorChangeResultText(previousFlag, changeFlag)}";
        Managers.UI.ShowDefualtUI<UI_PopupText>().ShowTextForTime(text);
    }
}

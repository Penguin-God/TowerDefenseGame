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

    TextShowAndHideController _textController;
    UnitColorChangerRpcHandler _unitColorChanger;
    void Awake()
    {
        _textController = FindObjectOfType<TextShowAndHideController>();
        _unitColorChanger = FindObjectOfType<UnitColorChangerRpcHandler>();
    }

    void OnEnable()
    {
        timer = Managers.Effect.TrackingTarget("WhiteUnitTimer", transform, new Vector3(0, 4, 3)).GetComponent<Multi_WhiteUnitTimer>();
        if (GetComponent<RPCable>().UsingId == PlayerIdManager.Id) // 하얀 유닛의 주인의 WhiteUnitTime을 사용해야 되서 RPC 씀
            photonView.RPC(nameof(SetupTimer), RpcTarget.All, Multi_GameManager.Instance.BattleData.WhiteUnitTime);

        if (PhotonNetwork.IsMasterClient)
            timer.Slider.onValueChanged.AddListener(ChangedColor);
    }

    [PunRPC] void SetupTimer(float time) => timer.Setup(time);

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
            UnitFlags result = _unitColorChanger.ChangeUnitColorWithViewId(photonView.ViewID);
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
        _textController.ShowTextForTime(text);
    }
}

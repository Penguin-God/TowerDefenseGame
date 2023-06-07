using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BattleReadyController : MonoBehaviourPun
{
    int _readyCount;
    UI_BattleStartController _battleStartControllerUI;
    public void EnterBattle(EnemySpawnNumManager manager)
    {
        _battleStartControllerUI = Managers.UI.ShowDefualtUI<UI_BattleStartController>();
        foreach (var ui in Managers.UI.SceneUIs)
            ui.gameObject.SetActive(false);
        Managers.UI.GetSceneUI<UI_EnemySelector>().gameObject.SetActive(true);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) // 임시. 나중에 혼자서 테스트할 수 있는 환결 구축 필요
            manager.SetClientSpawnNumber(0);
        StartCoroutine(Co_ActiveReadyButton(manager));
    }

    IEnumerator Co_ActiveReadyButton(EnemySpawnNumManager monsterSpawnManger)
    {
        yield return new WaitUntil(() => monsterSpawnManger.IsMonsterSelect(PlayerIdManager.Id));
        _battleStartControllerUI.ActiveReadyButton(Ready);
    }

    void Ready()
    {
        Managers.UI.GetSceneUI<UI_EnemySelector>().gameObject.SetActive(false);
        photonView.RPC(nameof(AddReadyCount), RpcTarget.MasterClient);
    }

    [PunRPC]
    void AddReadyCount()
    {
        _readyCount++;
        if (AllPlayerIsReady(_readyCount))
            photonView.RPC(nameof(BattleStart), RpcTarget.All);
    }

    bool AllPlayerIsReady(int readyCount) => readyCount >= PhotonNetwork.CurrentRoom.PlayerCount;

    [PunRPC]
    void BattleStart()
    {
        Managers.Resources.Destroy(_battleStartControllerUI.gameObject);
        foreach (var ui in Managers.UI.SceneUIs)
            ui.gameObject.SetActive(true);
        StartCoroutine(Co_NotifyGameStartEvent());
        Managers.UI.GetSceneUI<UI_EnemySelector>().gameObject.SetActive(false);
        Multi_GameManager.Instance.GameStart();
    }

    IEnumerator Co_NotifyGameStartEvent()
    {
        yield return new WaitForSeconds(0.05f); // 시간 커플링 때문에 딜레이 줌
        Multi_GameManager.Instance.BattleData.MaxUnit += 0;
    }
}

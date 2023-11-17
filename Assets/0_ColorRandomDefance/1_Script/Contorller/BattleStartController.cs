using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BattleStartController : MonoBehaviourPun
{
    BattleEventDispatcher _dispatcher;
    BattleUI_Mediator _uiMediator;
    public void Inject(BattleEventDispatcher dispatcher, BattleUI_Mediator uiMediator)
    {
        _dispatcher = dispatcher;
        _uiMediator = uiMediator;
    }

    int _readyCount;
    UI_BattleStartController _battleStartControllerUI;
    public void EnterBattle(EnemySpawnNumManager manager, SkillBattleDataContainer enemySkillData)
    {
        _battleStartControllerUI = Managers.UI.ShowDefualtUI<UI_BattleStartController>();
        _battleStartControllerUI.EnterBattle(enemySkillData);
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
        Managers.UI.GetSceneUI<UI_EnemySelector>().gameObject.SetActive(false);
        _uiMediator.ShowSceneUI<UI_BattleButtons>(BattleUI_Type.BattleButtons);
        _uiMediator.ShowSceneUI<UI_Paint>(BattleUI_Type.Paint);
        StartCoroutine(Co_NotifyGameStartEvent());
        _dispatcher.NotifyGameStart();
        Managers.Camera.OnIsLookMyWolrd += (isLookMy) => Managers.UI.GetSceneUI<UI_EnemySelector>().gameObject.SetActive(!isLookMy);
    }

    IEnumerator Co_NotifyGameStartEvent()
    {
        yield return new WaitForSeconds(0.05f); // 시간 커플링 때문에 딜레이 줌
        _dispatcher.NotifyMaxUnitCountChange(Multi_GameManager.Instance.BattleData.MaxUnit);
    }
}

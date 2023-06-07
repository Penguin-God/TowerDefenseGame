using System.Collections;
using UnityEngine;
using Photon.Pun;

public class WinOrLossController : MonoBehaviourPun
{
    public void Init(BattleEventDispatcher dispatcher)
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        dispatcher.OnMonsterCountChanged -= CheckMasterOver;
        dispatcher.OnMonsterCountChanged += CheckMasterOver;

        dispatcher.OnOpponentMonsterCountChange -= CheckClientOver;
        dispatcher.OnOpponentMonsterCountChange += CheckClientOver;
    }

    void CheckMasterOver(int monsterCount)
    {
        if (CheckGameOver(monsterCount))
            photonView.RPC(nameof(GameEnd), RpcTarget.All, PlayerIdManager.MasterId);
    }

    void CheckClientOver(int monsterCount)
    {
        if (CheckGameOver(monsterCount))
            photonView.RPC(nameof(GameEnd), RpcTarget.All, PlayerIdManager.ClientId);
    }

    bool CheckGameOver(int monsterCount) => monsterCount >= Multi_GameManager.Instance.BattleData.MaxEnemyCount;


    [PunRPC]
    void GameEnd(byte loserId)
    {
        if (loserId == PlayerIdManager.Id)
            Lose();
        else
            Win();
    }

    void Win() => GameEnd("승리");
    void Lose() => GameEnd("패배");

    public void GameEnd(string message)
    {
        ShowGameEndText(message);
        Time.timeScale = 0;
        StartCoroutine(Co_AfterReturnLobby());
    }

    void ShowGameEndText(string msg)
    {
        var ui = Managers.UI.ShowDefualtUI<UI_PopupText>();
        ui.Show(msg, 100f, Color.red);
        ui.OnRaycastTarget();
    }

    IEnumerator Co_AfterReturnLobby()
    {
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1;
        Managers.Scene.LoadScene(SceneTyep.Lobby);
        Managers.Clear();
    }
}
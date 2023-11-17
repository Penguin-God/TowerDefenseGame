using System.Collections;
using UnityEngine;
using Photon.Pun;

public class WinOrLossController : MonoBehaviourPun
{
    public void Inject(BattleEventDispatcher dispatcher, TextShowAndHideController textController)
    {
        _textController = textController;
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

    bool CheckGameOver(int monsterCount) => monsterCount >= Multi_GameManager.Instance.BattleData.MaxMonsterCount;


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

    TextShowAndHideController _textController;
    void ShowGameEndText(string text)
    {
        // UI 배리어
        _textController.ShowText(text, Color.red);
    }

    IEnumerator Co_AfterReturnLobby()
    {
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1;
        Managers.Scene.LoadScene(SceneTyep.Lobby);
        Managers.Clear();
    }
}
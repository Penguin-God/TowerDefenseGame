using System.Collections;
using UnityEngine;
using Photon.Pun;

public class WinOrLossController : MonoBehaviourPunCallbacks
{
    void Start()
    {
        Multi_EnemyManager.Instance.OnEnemyCountChanged += CheckGameOver;
    }

    void CheckGameOver(int enemyCount)
    {
        if (enemyCount >= Multi_GameManager.instance.BattleData.MaxEnemyCount)
        {
            Lose();
            photonView.RPC(nameof(Win), RpcTarget.Others);
        }
    }

    [PunRPC]
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
        var ui = Managers.UI.ShowUI<UI_PopupText>();
        ui.Show(msg, 100f, Color.red);
        ui.OnRaycastTarget();
    }

    IEnumerator Co_AfterReturnLobby()
    {
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Managers.Scene.LoadScene(SceneTyep.Lobby);
        Managers.Clear();
    }
}
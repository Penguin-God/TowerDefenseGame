using System.Collections;
using UnityEngine;
using Photon.Pun;

public readonly struct GameRewardData
{
    public readonly int Score;
    public readonly int Gold;
    public readonly int Gem;

    public GameRewardData(int score, int gold, int gem)
    {
        Score = score;
        Gold = gold;
        Gem = gem;
    }
}

public class WinOrLossController : MonoBehaviourPun
{
    public void Inject(BattleEventDispatcher dispatcher, TextShowAndHideController textController)
    {
        _textController = textController;
        if (PhotonNetwork.IsMasterClient == false) return;

        dispatcher.OnAnyMonsterCountChanged += CheckGameOver;
    }

    void CheckGameOver(int masterCount, int clientCount)
    {
        if(CheckGameOver(masterCount)) photonView.RPC(nameof(GameEnd), RpcTarget.All, PlayerIdManager.MasterId);
        else if(CheckGameOver(clientCount)) photonView.RPC(nameof(GameEnd), RpcTarget.All, PlayerIdManager.ClientId);

        bool CheckGameOver(int count) => count >= Multi_GameManager.Instance.BattleData.MaxMonsterCount;
    }


    [PunRPC]
    void GameEnd(byte loserId)
    {
        bool win = loserId != PlayerIdManager.Id;
        var rewardData = CreateRewardData(win);

        GainReward(rewardData);
        ShowGameEndText(BuildMessage(win, rewardData));
        Time.timeScale = 0;
        StartCoroutine(Co_AfterReturnLobby());
    }

    void GainReward(GameRewardData rewardData)
    {
        var playerDataManager = new PlayerPrefabsLoder().Load();
        playerDataManager.ChangeScore(rewardData.Score);
        playerDataManager.Gold.Add(rewardData.Gold);
        playerDataManager.Gem.Add(rewardData.Gem);
        new PlayerPrefabsSaver().Save(playerDataManager);
    }

    GameRewardData CreateRewardData(bool win)
    {
        if (win) return new GameRewardData(10, 500, 5);
        else return new GameRewardData(-10, 200, 1);
    }

    string BuildMessage(bool win, GameRewardData rewardData)
    {
        if (win) return $"승리!! 점수 +{BuildDataText()}";
        else return $"패배 점수 {BuildDataText()}";

        string BuildDataText() => $"{rewardData.Score}, 골드 {rewardData.Gold}원 획득, 젬 {rewardData.Gem}개 획득";
    }

    TextShowAndHideController _textController;
    void ShowGameEndText(string text)
    {
        // UI 배리어 넣기
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{
    //void Start()
    //{
    //    string gameId = null;

    //    gameId = "4174571";



    //    if (Advertisement.isSupported && !Advertisement.isInitialized)
    //    {
    //        Advertisement.Initialize(gameId);
    //    }
    //}

    private void Awake()
    {
        Advertisement.Initialize("4174571", true);
    }

    //private void Update()
    //{
    //    Advertisement.Initialize("4174571", true);
    //}
    public void ShowAD()
    {
        Debug.Log("클릭 됨");
        if (Advertisement.IsReady())
        {
            Advertisement.Show("Video");
        }
    }

    public void ShowRewardAd()
    {
        if (Advertisement.IsReady())
        {
            ShowOptions options = new ShowOptions { resultCallback = ResultedAds };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    private void ResultedAds(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Failed:
                Debug.LogError("광고 보기에 실패했습니다.");
                break;
            case ShowResult.Skipped:
                Debug.Log("광고를 스킵했습니다.");
                break;
            case ShowResult.Finished:
                GameManager.instance.Gold += 10;
                Debug.Log("광고 보기를 완료했습니다.");
                break;
        }
    }



    // 4174570 Apple
    // 4174571 Android
}

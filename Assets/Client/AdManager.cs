using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdManager : MonoBehaviour
{

    int Wood;
    int Iron;
    int Hammer;
    int StartGoldPrice;
    int StartFoodPrice;
    int PlusTouchDamegePrice;
    int StartGold;

    public ClientManager clientManager;

    private void Start()
    {
        Wood = PlayerPrefs.GetInt("Wood");
        Iron = PlayerPrefs.GetInt("Iron");
        Hammer = PlayerPrefs.GetInt("Hammer");
        StartGold = PlayerPrefs.GetInt("StartGold");
        StartGoldPrice = PlayerPrefs.GetInt("StartGoldPrice");
        StartFoodPrice = PlayerPrefs.GetInt("StartFoodPrice");
        PlusTouchDamegePrice = PlayerPrefs.GetInt("PlusTouchDamegePrice");
    }
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
        Advertisement.Initialize("4174571", false);
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
            Advertisement.Show("Interstitial_Android");
        }
    }

    public void ShowRewardAd()
    {
        if (Advertisement.IsReady())
        {
            ShowOptions options = new ShowOptions { resultCallback = ResultedAds };
            Advertisement.Show("Rewarded_Android", options);
        }
    }

    public GameObject ReWardButton;
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
                GameManager.instance.Gold += StartGold;
                UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
                Debug.Log("광고 보기를 완료했습니다.");
                ReWardButton.SetActive(false);


                break;
        }
    }

    private void IronResultedAds(ShowResult result)
    {
        Iron = PlayerPrefs.GetInt("Iron");
        StartGoldPrice = PlayerPrefs.GetInt("StartGoldPrice");
        switch (result)
        {
            case ShowResult.Failed:
                Debug.LogError("광고 보기에 실패했습니다.");
                break;
            case ShowResult.Skipped:
                Debug.Log("광고를 스킵했습니다.");
                break;
            case ShowResult.Finished:
                Iron += Random.Range(10, StartGoldPrice + 1);
                PlayerPrefs.SetInt("Iron",Iron);
                clientManager.UpdateIronText(Iron);
                Debug.Log("광고 보기를 완료했습니다.");
                PlayerPrefs.Save();


                break;
        }
    }
    public void IronShowRewardAd()
    {
        if (Advertisement.IsReady())
        {
            ShowOptions options = new ShowOptions { resultCallback = IronResultedAds };
            Advertisement.Show("Rewarded_Android", options);
        }
    }

    private void WoodResultedAds(ShowResult result)
    {
        Wood = PlayerPrefs.GetInt("Wood");
        StartFoodPrice = PlayerPrefs.GetInt("StartFoodPrice");
        switch (result)
        {
            case ShowResult.Failed:
                Debug.LogError("광고 보기에 실패했습니다.");
                break;
            case ShowResult.Skipped:
                Debug.Log("광고를 스킵했습니다.");
                break;
            case ShowResult.Finished:
                Wood += Random.Range(10, StartFoodPrice + 1);
                PlayerPrefs.SetInt("Wood", Wood);
                clientManager.UpdateWoodText(Wood);
                Debug.Log("광고 보기를 완료했습니다.");
                PlayerPrefs.Save();


                break;
        }
    }
    public void WoodShowRewardAd()
    {
        if (Advertisement.IsReady())
        {
            ShowOptions options = new ShowOptions { resultCallback = WoodResultedAds };
            Advertisement.Show("Rewarded_Android", options);
        }
    }

    private void HammerResultedAds(ShowResult result)
    {
        Hammer = PlayerPrefs.GetInt("Hammer");
        PlusTouchDamegePrice = PlayerPrefs.GetInt("PlusTouchDamegePrice");
        switch (result)
        {
            case ShowResult.Failed:
                Debug.LogError("광고 보기에 실패했습니다.");
                break;
            case ShowResult.Skipped:
                Debug.Log("광고를 스킵했습니다.");
                break;
            case ShowResult.Finished:
                Hammer += Random.Range(1, PlusTouchDamegePrice + 1);
                PlayerPrefs.SetInt("Hammer", Hammer);
                clientManager.UpdateHammerText(Hammer);
                Debug.Log("광고 보기를 완료했습니다.");
                PlayerPrefs.Save();

                break;
        }
    }
    public void HammerShowRewardAd()
    {
        if (Advertisement.IsReady())
        {
            ShowOptions options = new ShowOptions { resultCallback = HammerResultedAds };
            Advertisement.Show("Rewarded_Android", options);
        }
    }



    // 4174570 Apple
    // 4174571 Android
}

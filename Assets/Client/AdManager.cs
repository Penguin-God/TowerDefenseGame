using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{
    public void ShowAD()
    {
        Debug.Log("클릭 됨");
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }

    private void Awake()
    {
        Advertisement.Initialize("4174571", true);
    }

    // 4174570 Apple
    // 4174571 Android
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;

public class Multi_StageManager : MonoBehaviourPun
{
    private static Multi_StageManager instance;
    public static Multi_StageManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Multi_StageManager>();
                if (instance == null) instance = new GameObject("Multi_StageManager").AddComponent<Multi_StageManager>();
            }
            return instance;
        }
    }

    #region events
    public event Action<int> OnUpdateStage;
    #endregion

    [SerializeField] int currentStage = 0;
    [SerializeField] int maxStage;

    public int CurrentStage => currentStage;
    public int MaxStage => maxStage;

    [SerializeField] Slider timerSlider;
    [SerializeField] GameObject skipButton = null;
    [SerializeField] float stageTime = 40f;
    [SerializeField] float enemySpawnTimne = 40f;
    WaitForSeconds StageWait;

    void Start()
    {
        skipButton.GetComponent<Button>().onClick.AddListener(Skip);
        Multi_GameManager.instance.OnStart += UpdateStage;

        StageWait = new WaitForSeconds(Multi_SpawnManagers.NormalEnemy.EnemySpawnTime);
    }

    private void Update()
    {
        if (Multi_GameManager.instance.gameStart && currentStage < maxStage)
            timerSlider.value -= Time.deltaTime;
    }

    //void UpdateStage() 
    //{


    //    // TODO : Multi_SoundManager 만들면 거기로 옮기기
    //    //SoundManager.instance.PlayEffectSound_ByName("NewStageClip", 0.6f);
    //}

    // 나중에 스테이지를 2개 이상 건너뛰는 기능을 만들지도?
    public void UpdateStage() // 무한반복하는 재귀 함수( Co_Stage() 마지막 부분에 다시 NewStageStart()를 호출함)
    {
        currentStage += 1;
        OnUpdateStage?.Invoke(currentStage);

        timerSlider.maxValue = stageTime;
        timerSlider.value = stageTime;

        StartCoroutine(Co_Stage());
    }

    IEnumerator Co_Stage()
    {
        yield return StageWait;
        skipButton.SetActive(true);
        yield return new WaitUntil(() => timerSlider.value <= 0); // 스테이지 타이머 0이되면
        skipButton.SetActive(false);

        UpdateStage();
    }

    #region callback function
    public void Skip() => timerSlider.value = 0;
    #endregion
}

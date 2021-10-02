using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class TutorialManager : MonoBehaviour
{
    [Header("튜토리얼 설명 텍스트들이 들어감")]
    [SerializeField] GameObject[] arr_TutorialExplanation = null;

    [Header("튜토리얼 때 클릭하는 버튼")]
    [SerializeField] GameObject[] arr_TutorialButton = null;

    [Space][Space][Space]
    [SerializeField] Light mainLight = null;
    [SerializeField] Light subLight = null;
    [SerializeField] float mainLigth_OffIntensity = 0f;
    [SerializeField] Light spotLight = null;

    public Dictionary<GameObject, Action> dic_TutorialAction = new Dictionary<GameObject, Action>();


    public bool isTutorialExplanation = false;
    public bool isUITutorial = true;

    Button[] allButton = null;
    
    public void UIFalse(GameObject img)
    {
        isUITutorial = false;
        img.SetActive(false);
    }

    private void Update()
    {
        if (isUITutorial)
        {
            for(int i = 0;  i < allButton.Length; i++)
            {
                if(!arr_TutorialButton.Contains(allButton[i].gameObject)) allButton[i].enabled = false;
            }
        } 
    }

    private void Start()
    {
        allButton = FindObjectsOfType<Button>();
        SetDictionary();
    }

    void SetDictionary()
    {
        dic_TutorialAction.Add(arr_TutorialExplanation[0], () => Set_SpotLight(FindObjectOfType<NomalEnemy>().transform.position));
        dic_TutorialAction.Add(arr_TutorialExplanation[1], () => Set_SpotLight(FindObjectOfType<Unit_Swordman>().transform.position));
    }

    public void TutorialStart()
    {
        StartCoroutine(Co_Tutorial());
    }
    IEnumerator Co_Tutorial()
    {
        yield return new WaitUntil(() => !isUITutorial );
        yield return new WaitForSeconds(0.1f);
        // 인터페이스를 이용해 isTutorial를 false로 만드는 함수 강제하고 WaitUntil 조건에 사용하기
        for (int i = 0; i < arr_TutorialExplanation.Length; i++)
        {
            GameObject tutor_Text = arr_TutorialExplanation[i];
            tutor_Text.SetActive(true);
            dic_TutorialAction[tutor_Text]();
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0)); // 임시
            tutor_Text.SetActive(false);
            yield return null; // 임시 (마우스 입력 너무 빨리 받아서)
        }
    }

    void OffLigth()
    {
        mainLight.intensity = mainLigth_OffIntensity;
        subLight.intensity = 0;
        Time.timeScale = 0f;
    }

    void OnLigth()
    {
        mainLight.intensity = 1f;
        subLight.intensity = 0.3f;
        Time.timeScale = 1f;
    }

    void Set_SpotLight(Vector3 spot_position)
    {
        spotLight.gameObject.SetActive(true);
        spotLight.transform.position = spot_position + Vector3.up * 5;
        OffLigth();
    }
}

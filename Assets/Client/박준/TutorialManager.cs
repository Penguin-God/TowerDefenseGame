using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] Transform tutorTextParent = null;
    [Header("튜토리얼 설명 텍스트들이 들어감")]
    [SerializeField] GameObject[] arr_TutorialExplanationText = null;

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

    private void Start()
    {
        arr_TutorialExplanationText = new GameObject[tutorTextParent.childCount];
        for(int i = 0; i < arr_TutorialExplanationText.Length; i++)
        {
            arr_TutorialExplanationText[i] = tutorTextParent.GetChild(i).gameObject;
        }

        allButton = FindObjectsOfType<Button>();
        for (int i = 0; i < allButton.Length; i++)
        {
            allButton[i].enabled = false;
        }

        SetDictionary();
        TutorialStart();
    }

    void SetDictionary()
    {
        dic_TutorialAction.Add(arr_TutorialExplanationText[1], () => Set_SpotLight(FindObjectOfType<NomalEnemy>().transform.position));
        dic_TutorialAction.Add(arr_TutorialExplanationText[2], () => Set_SpotLight(FindObjectOfType<Unit_Swordman>().transform.position));
    }

    public void TutorialStart()
    {
        StartCoroutine(Co_Tutorial());
    }

    IEnumerator Co_Tutorial()
    {
        yield return new WaitForSeconds(0.1f);
        // 인터페이스를 이용해 isTutorial를 false로 만드는 함수 강제하고 WaitUntil 조건에 사용하기
        for (int i = 0; i < arr_TutorialExplanationText.Length; i++)
        {
            GameObject tutor_Text = arr_TutorialExplanationText[i];

            tutor_Text.SetActive(true);
            if(dic_TutorialAction.ContainsKey(tutor_Text)) dic_TutorialAction[tutor_Text]();

            ITutorial tutor = tutor_Text.GetComponent<ITutorial>();
            if (tutor != null)
            {
                tutor.TutorialAction();
                yield return new WaitUntil(() => tutor.EndCurrentTutorialAction()); // 임시
                tutor.TutorialEndAction();
            }

            tutor_Text.SetActive(false);
            yield return null; // 마우스 입력 등 튜토리얼 너무 빨리 넘어가서
        }
    }

    public void OffLigth()
    {
        mainLight.intensity = mainLigth_OffIntensity;
        subLight.intensity = 0;
        Time.timeScale = 0f;
    }

    public void OnLigth()
    {
        mainLight.intensity = 1f;
        subLight.intensity = 0.3f;
        Time.timeScale = 1f;
    }

    public void Set_SpotLight(Vector3 spot_position)
    {
        spotLight.gameObject.SetActive(true);
        spotLight.transform.position = spot_position + Vector3.up * 5;
        OffLigth();
    }


    [SerializeField] GameObject blind_UI = null;
    public void SetBlindUI(bool active)
    {
        blind_UI.SetActive(active);
    }
}

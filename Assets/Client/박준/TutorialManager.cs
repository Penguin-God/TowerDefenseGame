using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialManager : MonoBehaviour
{
    [Header("튜토리얼 설명 텍스트들이 들어감")]
    [SerializeField] GameObject[] arr_TutorialExplanation = null;

    [Space][Space][Space]
    [SerializeField] Light mainLight = null;
    [SerializeField] Light subLight = null;
    [SerializeField] float mainLigth_OffIntensity = 0f;
    [SerializeField] Light spotLight = null;

    public Dictionary<GameObject, Action> dic_TutorialAction = new Dictionary<GameObject, Action>();

    private void Start()
    {
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

    public bool isTutorial = false;
    IEnumerator Co_Tutorial()
    {
        yield return new WaitForSeconds(0.1f);
        // 인터페이스를 이용해 isTutorial를 false로 만드는 함수 강제하고 WaitUntil 조건에 사용하기
        for (int i = 0; i < arr_TutorialExplanation.Length; i++)
        {
            GameObject tutor = arr_TutorialExplanation[i];
            tutor.SetActive(true);
            dic_TutorialAction[tutor]();
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0)); // 임시
            arr_TutorialExplanation[i].SetActive(false);
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

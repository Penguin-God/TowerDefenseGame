using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] Transform tutorTextParent = null;
    [Header("튜토리얼 설명 텍스트들이 들어감")]
    [SerializeField] GameObject[] arr_TutorialExplanationText = null;

    [Header("튜토리얼 때 클릭하는 버튼")]
    [SerializeField] GameObject[] arr_TutorialButton = null;

    public bool isTutorialExplanation = false;
    public bool isUITutorial = true;

    private void Start()
    {
        arr_TutorialExplanationText = new GameObject[tutorTextParent.childCount];
        for(int i = 0; i < arr_TutorialExplanationText.Length; i++)
        {
            arr_TutorialExplanationText[i] = tutorTextParent.GetChild(i).gameObject;
        }

        TutorialStart();
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
            Time.timeScale = 0;
            GameObject tutor_Text = arr_TutorialExplanationText[i];
            tutor_Text.SetActive(true);

            ITutorial tutor = tutor_Text.GetComponent<ITutorial>();
            if (tutor != null)
            {
                tutor.TutorialAction();
                yield return new WaitUntil(() => tutor.EndCurrentTutorialAction());
            }

            tutor_Text.SetActive(false);
            Time.timeScale = 1;
            yield return new WaitForSeconds(0.1f); // 마우스 입력 등 튜토리얼이 너무 빨리 넘어가서
        }
    }
}

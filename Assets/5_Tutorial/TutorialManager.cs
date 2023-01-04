using System.Collections;
using UnityEngine;
using System.Linq;

public class TutorialManager : MonoBehaviour
{
    private TutorialFuntions tutorialFuntions = null;
    private void Awake()
    {
        tutorialFuntions = GetComponent<TutorialFuntions>();
    }

    public void TutorialStart(Transform tutorParent) 
        => StartCoroutine(Co_Tutorial(tutorParent.Cast<Transform>().Select(x => x.gameObject).ToArray()));

    IEnumerator Co_Tutorial(GameObject[] arr_TutorExplanation)
    {
        // 타임 스케일의 영향을 받지 않는 대기
        yield return new WaitForSecondsRealtime(0.1f);

        // 인터페이스를 이용해 isTutorial를 false로 만드는 함수 강제하고 WaitUntil 조건에 사용함
        for (int i = 0; i < arr_TutorExplanation.Length; i++)
        {
            GameObject tutor_Text = arr_TutorExplanation[i];
            tutor_Text.SetActive(true);

            ITutorial tutor = tutor_Text.GetComponent<ITutorial>();
            if (tutor != null)
            {
                tutor.TutorialAction();
                yield return new WaitUntil(() => tutor.EndCurrentTutorialAction());
            }

            tutor_Text.SetActive(false);
            yield return new WaitForSecondsRealtime(0.02f); // 마우스 입력 등 튜토리얼이 너무 빨리 넘어가서
        }
        // 모든 튜토리얼이 끝나면 게임 진행
        tutorialFuntions.GameProgress();
    }
}

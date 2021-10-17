using UnityEngine;

// 특정 행동을 하는게 아니라 UI에 관련된 설명만 읽는 스크립트 (UI가 없으면 전체 블라인드 있으면 UI 강조)
public class TutorailUI_Explanation : MonoBehaviour, ITutorial
{
    [SerializeField] TutorialFuntions tutorFuntions = null;
    [SerializeField] RectTransform showUITransform = null;

    public bool EndCurrentTutorialAction()
    {
        return Input.GetMouseButtonUp(0);
    }

    public void TutorialAction()
    {
        tutorFuntions.SetAllButton(false);
        if (showUITransform != null) tutorFuntions.SetBlindUI(showUITransform);
    }
}

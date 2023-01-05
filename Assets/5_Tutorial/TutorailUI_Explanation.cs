using UnityEngine;

// 특정 행동을 하는게 아니라 UI에 관련된 설명만 읽는 스크립트 (UI가 없으면 전체 블라인드 있으면 UI 강조)
public class TutorailUI_Explanation : MonoBehaviour, ITutorial
{
    [SerializeField] TutorialFuntions tutorFuntions = null;
    [SerializeField] string uiName;
    [SerializeField] protected RectTransform showUITransform = null;

    public bool EndCondition()
    {
        return Input.GetMouseButtonUp(0);
    }

    public virtual void TutorialAction()
    {
        if(string.IsNullOrEmpty(uiName) == false)
            showUITransform = GameObject.Find(uiName).GetComponent<RectTransform>();
        tutorFuntions.SetAllButton(false);
        if (showUITransform != null) tutorFuntions.SetBlindUI(showUITransform);
    }

    void OnDisable()
    {
        tutorFuntions.Reset_FocusUI();
    }
}

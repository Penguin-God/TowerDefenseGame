using UnityEngine;

public class TutorailUI_Explanation : MonoBehaviour, ITutorial
{
    [SerializeField] TutorialFuntions tutorFuntions = null;
    [SerializeField] RectTransform showUITransform = null;
    [SerializeField] GameObject BlindUI = null;

    public bool EndCurrentTutorialAction()
    {
        return Input.GetMouseButtonUp(0);
    }

    public void TutorialAction()
    {
        BlindUI.SetActive(true);
        if (showUITransform != null) tutorFuntions.SetBlindUI(showUITransform);
    }

    [SerializeField] bool isGameProgress;
    private void OnDisable()
    {
        BlindUI.SetActive(false);
        if (isGameProgress) tutorFuntions.GameProgress();
    }
}

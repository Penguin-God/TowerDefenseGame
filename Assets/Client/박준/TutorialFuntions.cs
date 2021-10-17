using UnityEngine;
using UnityEngine.UI;

public class TutorialFuntions : MonoBehaviour
{
    [Space][Space][Space]
    [SerializeField] Light mainLight = null;
    [SerializeField] Light subLight = null;
    [SerializeField] float mainLigth_OffIntensity = 0f;

    [SerializeField] Light spotLight = null;

    public void OffLigth()
    {
        Time.timeScale = 0;
        mainLight.intensity = mainLigth_OffIntensity;
        subLight.intensity = 0.1f;
    }

    public void OnLigth()
    {
        Time.timeScale = 1;
        spotLight.gameObject.SetActive(false);
        mainLight.intensity = 1f;
        subLight.intensity = 0.3f;
    }

    public void Set_SpotLight(Vector3 spot_position)
    {
        spotLight.gameObject.SetActive(true);
        spotLight.transform.position = spot_position + Vector3.up * 5;
    }


    [SerializeField] GameObject lightFocus_UI = null;
    public void SetBlindUI(RectTransform rect_tf)
    {
        lightFocus_UI.SetActive(true);
        RectTransform rect = lightFocus_UI.GetComponent<RectTransform>();
        rect.pivot = rect_tf.pivot;
        rect.anchorMin = rect_tf.anchorMin;
        rect.anchorMax = rect_tf.anchorMax;
        rect.position = rect_tf.position;

        rect.sizeDelta = rect_tf.sizeDelta;
        Time.timeScale = 0;
    }

    public void Reset_FocusUI()
    {
        lightFocus_UI.SetActive(false);
        lightFocus_UI.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
    }

    public void GameProgress()
    {
        Time.timeScale = 1;
        spotLight.gameObject.SetActive(false);
        Reset_FocusUI();
        SetAllButton(true);
    }

    [SerializeField] Button[] allButtons = null;

    [ContextMenu("버튼 배열 세팅")]
    void SettingButtonArray()
    {
        // 꺼져 있는 게임 오브젝트도 가져옴
        allButtons = Resources.FindObjectsOfTypeAll<Button>();
    }

    public void SetAllButton(bool active)
    {
        for (int i = 0; i < allButtons.Length; i++)
        {
            allButtons[i].enabled = active;
        }
    }
}

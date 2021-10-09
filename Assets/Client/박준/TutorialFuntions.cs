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
        mainLight.intensity = mainLigth_OffIntensity;
        subLight.intensity = 0.1f;
    }

    public void OnLigth()
    {
        spotLight.gameObject.SetActive(false);
        mainLight.intensity = 1f;
        subLight.intensity = 0.3f;
    }

    public void Set_SpotLight(Vector3 spot_position)
    {
        spotLight.gameObject.SetActive(true);
        spotLight.transform.position = spot_position + Vector3.up * 5;
    }


    [SerializeField] GameObject blind_UI = null;
    public void SetBlindUI(bool active)
    {
        blind_UI.SetActive(active);
    }

    public void SetBlindUI(RectTransform rect_tf)
    {
        RectTransform rect = blind_UI.GetComponent<RectTransform>();
        rect.anchorMin = rect_tf.anchorMin;
        rect.anchorMax = rect_tf.anchorMax;
        rect.position = rect_tf.position;
        //rect.anchoredPosition = rect_tf.anchoredPosition;
        rect.sizeDelta = rect_tf.sizeDelta;
        rect.pivot = rect_tf.pivot;
        Time.timeScale = 0;
    }

    public void GameProgress()
    {
        spotLight.gameObject.SetActive(false);
        SetBlindUI(false);
        SetAllButton(true);
    }

    public void SetAllButton(bool active)
    {
        Button[] allbutton = FindObjectsOfType<Button>();
        for (int i = 0; i < allbutton.Length; i++)
        {
            allbutton[i].enabled = active;
        }
    }
}

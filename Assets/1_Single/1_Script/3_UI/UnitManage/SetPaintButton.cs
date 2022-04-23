using UnityEngine;
using UnityEngine.UI;

public class SetPaintButton : MonoBehaviour
{
    [SerializeField] Color color;
    [SerializeField] Image paint;
    [SerializeField] GameObject[] obj_Colors;
    [SerializeField] GameObject obj_showColor;
    //[SerializeField] GameObject obj_DefaultImage;
    [SerializeField] Button colorButton;

    private void Start()
    {
        colorButton.onClick.AddListener(() => SettingPaintButton());
    }

    public void SettingPaintButton()
    {
        obj_showColor.SetActive(true);
        for(int i = 0; i < obj_Colors.Length; i++)
        {
            if(obj_Colors[i] != obj_showColor) obj_Colors[i].SetActive(false);
        }

        SetColor();
        SoundManager.instance.PlayEffectSound_ByName("SelectColor");
    }

    public void SetColor()
    {
        paint.color = color;
    }
}

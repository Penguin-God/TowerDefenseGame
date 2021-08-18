using UnityEngine;
using UnityEngine.UI;

public class SetPaintButton : MonoBehaviour
{
    [SerializeField] Color color;
    [SerializeField] Image paint;
    [SerializeField] GameObject[] obj_Colors;
    [SerializeField] GameObject obj_showColor;
    [SerializeField] GameObject obj_DefaultImage;
    public void SettingPaintButton()
    {
        if (obj_DefaultImage.activeSelf) obj_DefaultImage.SetActive(false);
        obj_showColor.SetActive(true);
        for(int i = 0; i < obj_Colors.Length; i++)
        {
            if(obj_Colors[i] != obj_showColor) obj_Colors[i].SetActive(false);
        }
        color = GetComponent<Image>().color;
        paint.color = color;
    }
}

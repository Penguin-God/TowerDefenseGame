using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint : MonoBehaviour
{
    [SerializeField] GameObject paint;

    public void SetPaint()
    {
        if (paint.activeSelf) paint.SetActive(false);
        else paint.SetActive(true);
        SoundManager.instance.PlayEffectSound_ByName("SelectColor");
    }
}

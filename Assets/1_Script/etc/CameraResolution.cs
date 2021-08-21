using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraResolution : MonoBehaviour
{
    [SerializeField] float width;
    [SerializeField] float height;
    [SerializeField] CanvasScaler canvasScaler;

    private void Awake()
    {
        canvasScaler = FindObjectOfType<CanvasScaler>();
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;

        float scaleHeight = ((float)Screen.width / Screen.height) / (width / height);
        float scaleWidth = 1f / scaleHeight;
        if(scaleHeight < 1)
        {
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;
        }

        camera.rect = rect;

        if (Screen.width >= width) canvasScaler.matchWidthOrHeight = 1;
        else canvasScaler.matchWidthOrHeight = 0;
    }
}

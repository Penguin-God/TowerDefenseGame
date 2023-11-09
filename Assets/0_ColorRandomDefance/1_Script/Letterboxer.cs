using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letterboxer : MonoBehaviour
{
    float targetAspect = 800f / 480f; // Ÿ�� ȭ�� ���� ����

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        float windowAspect = (float)Screen.width / (float)Screen.height; // ���� ȭ�� ����
        float scaleHeight = windowAspect / targetAspect; // ȭ�� ���� ������
        Rect rect = cam.rect;

        if (scaleHeight < 1.0f)
        {
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }
        cam.rect = rect;
    }
}

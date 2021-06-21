using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraWidth : MonoBehaviour
{
    Camera mainCamera;
    float height;
    float width;
    void Start()
    {
        mainCamera = GetComponent<Camera>();

        height = 2f * mainCamera.orthographicSize;
        width = height * mainCamera.aspect;

        //float desiredAspect = 800f / 480f;
        //float cameraSize = desiredAspect / mainCamera.aspect;

        //mainCamera.orthographicSize = height * cameraSize;

        mainCamera.fieldOfView = calcVertivalFOV(height, mainCamera.aspect);

        Debug.Log(mainCamera.fieldOfView);
        Debug.Log(mainCamera);
    }

    private float horizontalFOV = 120f;
    //somewhere in update if screen is resizable

    private float calcVertivalFOV(float hFOVInDeg, float aspectRatio)
    {
        float hFOVInRads = hFOVInDeg * Mathf.Deg2Rad;
        float vFOVInRads = 2 * Mathf.Atan(Mathf.Tan(hFOVInRads / 2) / aspectRatio);
        float vFOV = vFOVInRads * Mathf.Rad2Deg;
        return vFOV;
    }
}

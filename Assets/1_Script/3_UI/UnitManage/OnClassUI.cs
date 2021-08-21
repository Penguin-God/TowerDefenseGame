using UnityEngine;

public class OnClassUI : MonoBehaviour
{
    [SerializeField] GameObject[] unitColorUI;

    private void OnEnable()
    {
        for(int i = 0; i < unitColorUI.Length; i++) unitColorUI[i].SetActive(false);
    }

    private void OnDisable()
    {
        unitColorUI[unitColorUI.Length - 1].SetActive(true);
    }
}

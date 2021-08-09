using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddOnClickEvent : MonoBehaviour
{
    [SerializeField] GameObject[] obj_UnitImages;
    [SerializeField] GameObject[] obj_UnitManageUI;
    GameObject currentShowUI = null;

    StoryMode storyMode;
    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        storyMode = FindObjectOfType<StoryMode>();
        for (int i = 0; i < obj_UnitManageUI.Length; i++)
        {
            int index = i;
            obj_UnitImages[index].GetComponent<Button>().onClick.AddListener(() => SetOnClick(index));
        }
    }

    void SetOnClick(int i)
    {
        string name = obj_UnitImages[i].GetComponent<SetUnitButton>().unitName;
        storyMode.unitTagName = name;
        obj_UnitManageUI[i].SetActive(true);
        if (currentShowUI != null && currentShowUI != obj_UnitManageUI[i]) currentShowUI.SetActive(false);
        currentShowUI = obj_UnitManageUI[i];
        audioSource.Play();
    }

    private void OnDisable()
    {
        if (currentShowUI != null) currentShowUI.SetActive(false);
        storyMode.unitTagName = "";
    }
}

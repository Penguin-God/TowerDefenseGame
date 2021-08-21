using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUnitButton : MonoBehaviour
{
    public string unitName;
    Text txt_UnitCount;
    UnitManageWindowDictionary unitWindowDic;

    string unitClass;
    private void Awake()
    {
        addClikcEvent = GetComponentInParent<AddOnClickEvent>();
        txt_UnitCount = GetComponentInChildren<Text>();
        unitClass = txt_UnitCount.text;
        txt_UnitCount.text = unitClass + " : 0";

        Button button = GetComponent<Button>();

        unitWindowDic = GetComponentInParent<UnitManageWindowDictionary>();
        button.onClick.AddListener(() => unitWindowDic.ShowUnitManageWindow(unitName));

        button.onClick.AddListener(() => FindObjectOfType<StoryMode>().unitTagName = unitName);
        button.onClick.AddListener(() => SoundManager.instance.PlayEffectSound_ByName("SelectColor"));
    }

    AddOnClickEvent addClikcEvent = null;
    private void OnEnable()
    {
        //if(addClikcEvent != null) 
        StartCoroutine("Co_UnitCount");
    }

    private void OnDisable()
    {
        StopCoroutine("Co_UnitCount");
    }

    int count = 0;
    WaitForSeconds ws = new WaitForSeconds(0.4f);
    IEnumerator Co_UnitCount()
    {
        while (true)
        {
            count = GameObject.FindGameObjectsWithTag(unitName).Length;
            txt_UnitCount.text = unitClass + " : " + count;
            yield return ws;
        }
    }
}

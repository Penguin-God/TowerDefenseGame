using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGround : UI_Popup
{
    protected override void Init()
    {
        base.Init();
        gameObject.SetActive(false);
    }
    public void SetText(string newText) => GetComponentInChildren<Text>().text = newText;
    public void SetPosition(Vector3 pos) => GetComponentInChildren<Image>().transform.position = pos;
}

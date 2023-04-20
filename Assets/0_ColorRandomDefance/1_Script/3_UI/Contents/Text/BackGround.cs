using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGround : UI_Popup
{
    Text _text;
    Image _image;
    protected override void Init()
    {
        base.Init();
        gameObject.SetActive(false);
        _text = GetComponentInChildren<Text>();
        _image = GetComponentInChildren<Image>();
    }
    public void SetText(string newText) => _text.text = newText;
    public void SetFontSize(int size) => _text.fontSize = size;
    public void SetSize(Vector2 size)
    {
        _image.GetComponent<RectTransform>().sizeDelta = size;
        _text.GetComponent<RectTransform>().sizeDelta = size * 2;
    }
    public void SetPosition(Vector3 pos) => _image.transform.position = pos;
}

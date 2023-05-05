using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackGround : UI_Popup
{
    TextMeshProUGUI _text;
    Image _image;

    public override void Setup()
    {
        base.Init();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _image = GetComponentInChildren<Image>();
    }

    public void SetText(string newText) => _text.text = newText;
    public void SetLineSpace(float space) => _text.lineSpacing = space;
    public void SetFontSize(int size) => _text.fontSize = size;
    public void SetSize(Vector2 size) => _image.GetComponent<RectTransform>().sizeDelta = size;

    public void SetAnchor(TextAlignmentOptions textAnchor) => _text.alignment = textAnchor;

    public void SetPosition(Vector3 pos) => _image.transform.position = pos;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackGround : UI_Popup
{
    TextMeshProUGUI Text => GetComponentInChildren<TextMeshProUGUI>(true);
    Image Image => GetComponentInChildren<Image>(true);

    public void SetText(string newText) => Text.text = newText;
    public void SetLineSpace(float space) => Text.lineSpacing = space;
    public void SetFontSize(int size) => Text.fontSize = size;
    public void SetSize(Vector2 size) => Image.GetComponent<RectTransform>().sizeDelta = size;

    public void SetAnchor(TextAlignmentOptions textAnchor) => Text.alignment = textAnchor;

    public void SetPosition(Vector3 pos) => StartCoroutine(Co_SetPosition(pos));
    IEnumerator Co_SetPosition(Vector3 pos) // 최초 생성 시 위치가 이상한 거 때문에 대기 줌.
    {
        Image.gameObject.SetActive(false);
        yield return null;
        Image.transform.position = pos;
        Image.gameObject.SetActive(true);
    }
}

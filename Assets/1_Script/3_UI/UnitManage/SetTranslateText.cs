using UnityEngine;
using UnityEngine.UI;

public class SetTranslateText : MonoBehaviour
{
    private Text text;
    private void Awake()
    {
        text = GetComponentInChildren<Text>();
        text.fontSize = 22;
    }

    private void OnEnable()
    {
        text.text = (GameManager.instance.playerEnterStoryMode) ? "필드로" : "적군의 성으로";
    }
}

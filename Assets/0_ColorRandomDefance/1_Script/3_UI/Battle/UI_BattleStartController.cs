using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_BattleStartController : UI_Base
{
    [SerializeField] Button _readyButton;

    protected override void Init()
    {
        _readyButton.GetComponentInChildren<TextMeshProUGUI>().text = "��ȯ�� ���͸� ������ �ֽʽÿ�";
        _readyButton.enabled = false;
    }

    public void ActiveReadyButton(UnityAction onReady)
    {
        _readyButton.GetComponentInChildren<TextMeshProUGUI>().text = "������ ������ �غ� �Ǿ��ٸ� �� ��ư�� Ŭ�����ּ���";
        _readyButton.enabled = true;
        _readyButton.onClick.AddListener(Ready);
        _readyButton.onClick.AddListener(onReady);
    }

    void Ready()
    {
        _readyButton.GetComponentInChildren<TextMeshProUGUI>().text = "�غ� �Ϸ�";
        _readyButton.enabled = false;
    }
}

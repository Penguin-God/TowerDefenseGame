using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestName : MonoBehaviour
{
    public InputField nameInputField;
    public Text savedNameText;
    public WebServerTest webServer;


    // ������ �̸� ���� ��ư�� ������ �� ȣ��� �Լ�
    public void SaveName()
    {
        string playerName = nameInputField.text;

        print("�̸�:" + playerName);
        webServer.AddPlayer(playerName);
    }

}

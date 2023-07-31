using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestName : MonoBehaviour
{
    public InputField nameInputField;
    public Text savedNameText;
    public WebServerTest webServer;


    // 유저가 이름 저장 버튼을 눌렀을 때 호출될 함수
    public void SaveName()
    {
        string playerName = nameInputField.text;

        print("이름:" + playerName);
        webServer.AddPlayer(playerName);
    }

}

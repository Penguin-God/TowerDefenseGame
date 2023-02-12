using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Net;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;



public class WebServerTest : MonoBehaviour
{
    [Serializable]
    public class Player
    {
        public int Id;
        public int UserId;
        public string UserName;
        public List<Skill> skills;
        public DateTime Date;
    }

    [Serializable]
    public class Skill
    {
        public int SkillId;
        public string SkillName;
        public int SkillExp;
        public int OwnerId;
        //public Player Owner;
    }

    //public List<Skill> testObj = new List<Skill>() { new Skill { SkillName = "태극", SkillExp = 123 , Owner = new Player { UserName = 777777} }, new Skill { SkillName = "검유강", SkillExp = 11 } };
    //dbSkill _skills = new dbSkill { SkillName = "태극", SkillExp = 123, Owner = new Player { UserName = 77777 } };
    //dbSkill _skills2 = new dbSkill { SkillName = "검유강", SkillExp = 321, Owner = new Player { UserName = 88888 } };
    

    void Start()
    {
        StartCoroutine(GetTextID(1));
    }

    private void Update()
    {
        // 쓰기
        if (Input.GetKeyDown(KeyCode.P))
        {
            Player player = new Player() { UserName = "Gunal", skills = new List<Skill>() { new Skill() { SkillName = "태극"} } };
            string jsonfile = JsonUtility.ToJson(player);
            print(jsonfile);
            StartCoroutine(Upload(jsonfile));
        }

        // 읽기
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(GetText());
        }
    }

    

    IEnumerator Upload(string jsonfile)
    {
        using (UnityWebRequest request = UnityWebRequest.Post("https://localhost:44319/api/api", jsonfile))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonfile);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();
            Debug.Log("Post 결과");
            Debug.Log(request.downloadHandler.text);
            Debug.Log("Post결과 끗");
            //if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            //{
            //    Debug.Log(request.error);
            //}
            //else
            //{
            //    Debug.Log(request.downloadHandler.text);
            //}
        }
    }

    IEnumerator GetText()
    {
        Debug.Log("테스트 시작");
        UnityWebRequest www = UnityWebRequest.Get("https://localhost:44319/api/api");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }

    IEnumerator GetTextID(int Id)
    {
        Debug.Log("테스트 시작");
        UnityWebRequest www = UnityWebRequest.Get($"https://localhost:44319/api/api");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
}

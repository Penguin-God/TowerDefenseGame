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
    //[Serializable]
    //public class Player
    //{
    //    public int Id;
    //    public int UserId;
    //    public string UserName;
    //    public List<Skill> skills;
    //    public DateTime Date;
    //}

    //[Serializable]
    //public class Skill
    //{
    //    public int SkillId;
    //    public string SkillName;
    //    public int SkillExp;
    //    public int OwnerId;
    //    //public Player Owner;
    //}

    [Serializable]
    public class Player
    {
        public int Id;
        public int UserId;
        public string Name;
        public int Score;
        public int Gold;
        public int Gem;
        public DateTime Date;

        public List<Skill> Skills;

    }
    [Serializable]
    public class Skill
    {
        public int SkillId;
        public string SkillName;
        public int SkillExp;
        public int SkillLevel;

        public int OwnerId;
        public string OwnerName;
        public Player Owner;
    }

    //public List<Skill> testObj = new List<Skill>() { new Skill { SkillName = "태극", SkillExp = 123 , Owner = new Player { UserName = 777777} }, new Skill { SkillName = "검유강", SkillExp = 11 } };
    //dbSkill _skills = new dbSkill { SkillName = "태극", SkillExp = 123, Owner = new Player { UserName = 77777 } };
    //dbSkill _skills2 = new dbSkill { SkillName = "검유강", SkillExp = 321, Owner = new Player { UserName = 88888 } };


    void Start()
    {
        StartCoroutine(GetTextID("Player", 1));
    }

    private void Update()
    {
        // 쓰기  Skills = new List<Skill>() { new Skill() { SkillName = "태극", OwnerId = 1, SkillExp = 11, SkillLevel = 10 } }
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddPlayer("Gunal");
        }

        // 읽기
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(GetText("Player"));
        }
    }

    public void AddPlayer(string playerName)
    {
        Player player = new Player() {  UserId = 0, Name = playerName, Gold = 0, Gem = 0, Score = 0, Date = DateTime.Now, Skills = null };
        string jsonfile = JsonUtility.ToJson(player);
        print(jsonfile);
        StartCoroutine(Upload("Player", jsonfile));
    }

    IEnumerator Upload(string controllor, string jsonfile)
    {
        using (UnityWebRequest request = UnityWebRequest.Post($"https://localhost:44319/{controllor}", jsonfile))
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

    IEnumerator GetText(string controllor)
    {
        Debug.Log("테스트 시작");
        UnityWebRequest www = UnityWebRequest.Get($"https://localhost:44319/{controllor}");
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

    IEnumerator GetTextID(string controllor, int Id)
    {
        Debug.Log("테스트 시작");
        UnityWebRequest www = UnityWebRequest.Get($"https://localhost:44319/{controllor}");
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

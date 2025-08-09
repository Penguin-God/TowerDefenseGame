using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Net;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

public class WebServerTest : MonoBehaviour
{

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

    Player playerData;

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        AddPlayer("Gunal");
    //    }

    //    // 읽기
    //    if (Input.GetKeyDown(KeyCode.G))
    //    {
    //        StartCoroutine(GetText("Player"));
    //    }
    //}

    public void AddPlayer(string playerName)
    {
        Player player = new Player() {  UserId = 0, Name = playerName, Gold = 0, Gem = 0, Score = 0, Skills = null };
        string jsonfile = JsonUtility.ToJson(player);
        print("Add Player" + jsonfile);
        StartCoroutine(Upload("Player", jsonfile));
    }

    public Player GetPlayer(string playerName)
    {
        StartCoroutine(GetPlayerName("Player", playerName));

        return playerData;
    }

    public void StartPlayer(string playerName)
    {
        StartCoroutine(CoStartPlayer(playerName));
    }

    IEnumerator CoStartPlayer(string playerName)
    {
        yield return StartCoroutine(GetPlayerName("Player", playerName));
        
        if (playerData == null)
        {
            Debug.Log("아이디를 생성합니다.");
            AddPlayer(playerName);
        }
        else
        {
            Debug.Log("초기화 진행");
            // name = playerData.Name
        }
    }

    IEnumerator Upload(string controllor, string jsonfile)
    {
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm($"https://localhost:44319/{controllor}", jsonfile))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonfile);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();
            Debug.Log("Post 결과");
            
            if (request.downloadHandler.text == "")
                Debug.Log("이미 존재하는 닉네임 입니다.");
            else
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

    //IEnumerator GetPlayerID(string controllor, int Id)
    //{
    //    Debug.Log("테스트 시작");
    //    UnityWebRequest www = UnityWebRequest.Get($"https://localhost:44319/{controllor}/{Id}");
    //    yield return www.SendWebRequest();

    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        // Show results as text
    //        Debug.Log(www.downloadHandler.text);

    //        // Or retrieve results as binary data
    //        byte[] results = www.downloadHandler.data;
    //    }
    //}

    IEnumerator GetPlayerName(string controllor, string name)
    {
        playerData = null;
        UnityWebRequest www = UnityWebRequest.Get($"https://localhost:44319/{controllor}/{name}");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (www.downloadHandler.text == "")
                yield break;

            
            string json = www.downloadHandler.text;
            //Debug.Log("파싱전" + json);
            //playerData = JsonUtility.FromJson<Player>(json);
            playerData = JsonConvert.DeserializeObject<Player>(json);

            Debug.Log(playerData.Name);
            // Show results as text
            //Debug.Log(www.downloadHandler.text);
            
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
}

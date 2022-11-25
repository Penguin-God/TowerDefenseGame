﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Net;
using System.IO;
using System;
using System.Text;

public class WebServerTest : MonoBehaviour
{
    public GameResult testObj = new GameResult { UserName = "Unity", Score = 100 };

    void Start()
    {
        StartCoroutine(GetTextID(2));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            string jsonfile = JsonUtility.ToJson(testObj);
            print(jsonfile);
            StartCoroutine(Upload(jsonfile));
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(GetText());
        }
    }

    public class GameResult
    {
        public int Id;
        public int UserId;
        public string UserName;
        public int Score;
        public DateTime DateTime;
    }

    IEnumerator Upload(string jsonfile)
    {
        using (UnityWebRequest request = UnityWebRequest.Post("https://localhost:44394/api/ranking", jsonfile))
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

    //IEnumerator Upload()
    //{
    //    WWWForm form = new WWWForm();


    //    UnityWebRequest www = UnityWebRequest.Post("https://localhost:44394/api/ranking", form);
    //    yield return www.SendWebRequest();

    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        Debug.Log("Post 성공");
    //    }
    //}

    IEnumerator GetText()
    {
        Debug.Log("테스트 시작");
        UnityWebRequest www = UnityWebRequest.Get("https://localhost:44394/api/ranking");
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
        UnityWebRequest www = UnityWebRequest.Get($"https://localhost:44394/api/ranking/{Id}");
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

//string PostData = "a=1&b=2"
//StringBuilder dataParams = new StringBuilder();

//HttpWebRequest request = null;
//HttpWebResponse response = null;

//try
//{
//    byte[] bytes = UTF8Encoding.UTF8.GetBytes(dataParams.ToString());
//    request = (HttpWebRequest)WebRequest.Create(접속할 URL주소);
//    request.Method = "POST";
//    request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
//    request.Timeout = 1000;
//    using (var stream = request.GetRequestStream())
//    {
//        stream.Write(bytes, 0, bytes.Length);
//        stream.Flush();
//        stream.Close();
//    }

//    response = (HttpWebResponse)request.GetResponse();
//    StreamReader reader = new StreamReader(response.GetResponseStream());
//    string json = reader.ReadToEnd();
//}
//catch (WebException webExcp)
//{
//    WebExceptionStatus status = webExcp.Status;
//    if (status == WebExceptionStatus.ProtocolError)
//    {
//        HttpWebResponse httpResponse = (HttpWebResponse)webExcp.Response;
//    }
//}
//catch (Exception e)
//{
//    throw e;
//}

//response.Close();
//response.Dispose();
//request.Abort();
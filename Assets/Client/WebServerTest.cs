using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Net;
using System.IO;
using System;
using System.Text;

public class WebServerTest : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetText());
        StartCoroutine(GetTextID(1));
        StartCoroutine(Upload());
        StartCoroutine(GetText());
    }

    public class GameResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Score { get; set; }
        public DateTime DateTime { get; set; }
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("UserName", "unity");
        form.AddField("Score", "100");

        UnityWebRequest www = UnityWebRequest.Post("https://localhost:44394/api/ranking", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Post 성공");
        }
    }

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartButton : MonoBehaviour
{

    public GameObject EasyButton;
    public GameObject NormalButton;
    public GameObject HardButton;
    public GameObject ImpossiableButton;

    public void ClickStartButton()
    {
        Loding.LoadScene("합친 씬 - 장익준");
        //EasyButton.gameObject.SetActive(true);
        //NormalButton.gameObject.SetActive(true);
        //HardButton.gameObject.SetActive(true);
        //ImpossiableButton.gameObject.SetActive(true);
    }

    public void ClickTutorialsButton()
    {
        Loding.LoadScene("Tutorial - 박준");
    }

    //public void ClickEasyButton()
    //{
        //Loding.LoadScene("합친 씬 - 장익준");
        //GameManager.instance.starts = GameManager.Starts.Easy;
         
    //}

    //public void ClickNormalButton()
    //{
        //Loding.LoadScene("합친 씬 - 장익준");
        //GameManager.instance.starts = GameManager.Starts.Normal;
    //}

    //public void ClickHardButton()
    //{
        //Loding.LoadScene("합친 씬 - 장익준");
        //GameManager.instance.starts = GameManager.Starts.Hard;
        
        
    //}

    //public void ClickImpassiableButton()
    //{
        //Loding.LoadScene("합친 씬 - 장익준");
        //GameManager.instance.starts = GameManager.Starts.Impossiable;
         
    //}


}

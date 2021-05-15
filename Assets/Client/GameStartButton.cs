using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartButton : MonoBehaviour
{
    public void ClickStartButton()
    {
        SceneManager.LoadScene("합친 씬 - 장익준");
    }
    
}

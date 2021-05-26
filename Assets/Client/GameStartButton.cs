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
    public EnemySpawn enemySpawn;

    public void ClickStartButton()
    {
        //SceneManager.LoadScene("합친 씬 - 장익준");
        EasyButton.gameObject.SetActive(true);
        NormalButton.gameObject.SetActive(true);
        HardButton.gameObject.SetActive(true);
        ImpossiableButton.gameObject.SetActive(true);
    }

    public void ClickTutorialsButton()
    {
        SceneManager.LoadScene("Tutorials");
    }

    public void ClickEasyButton()
    {
        enemySpawn.enemyHpWeight = 15;
        SceneManager.LoadScene("합친 씬 - 장익준");
    }

    public void ClickNormalButton()
    {
        enemySpawn.enemyHpWeight = 25;
        SceneManager.LoadScene("합친 씬 - 장익준");
    }

    public void ClickHardButton()
    {
        enemySpawn.enemyHpWeight = 35;
        SceneManager.LoadScene("합친 씬 - 장익준");
    }

    public void ClickImpassiableButton()
    {
        enemySpawn.enemyHpWeight = 45;
        SceneManager.LoadScene("합친 씬 - 장익준");
    }


}

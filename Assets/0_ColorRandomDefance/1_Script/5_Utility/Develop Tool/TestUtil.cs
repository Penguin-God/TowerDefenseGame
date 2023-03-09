using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUtil : MonoBehaviour
{
    void Update()
    {
        ToggleTimeScale();
        GetMoney();
    }

    void ToggleTimeScale()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (Time.timeScale == 10f)
                Time.timeScale = 1f;
            else
                Time.timeScale = 10f;
        }
    }

    void GetMoney()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Multi_GameManager.Instance.AddGold(5000);
            Multi_GameManager.Instance.AddFood(1000);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CreateDefenser : MonoBehaviour
{
    // public GameObject Soldierprefab;
    public GameObject Soldier;

    void Start()
    {
        gameObject.SetActive(true);
    }

    public void DrawSoldier()
    {
        if (GameManager.instance.Gold >= 5)
        {
            CreateSoldier();
            ExpenditureGold();
        }
    }

    public void CreateSoldier()
    {
        int randomnumber = Random.Range(0, 6);
        // Soldier = transform.GetChild(randomnumber).gameObject;
        Soldier = Instantiate(transform.GetChild(randomnumber).gameObject, transform.position, transform.rotation);

        Soldier.transform.position = RandomPosition(10, 0, 10);
        Soldier.SetActive(true);
        
    }

    void ExpenditureGold()
    {
        GameManager.instance.Gold -= 5;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
    }

    Vector3 RandomPosition(float x, float y, float z)
    {
        float randomX = Random.Range(-x, x);
        float randomY = Random.Range(-y, y);
        float randomZ = Random.Range(-z, z);
        return new Vector3(randomX, randomY, randomZ);
    }


}
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

    public void DrawSoldier(int Colornumber, int Soldiernumber)
    {
        if (GameManager.instance.Gold >= 5)
        {
            CreateSoldier(Colornumber, Soldiernumber);
            ExpenditureGold();
        }
    }

    public GameObject CreateSoldier(int Colornumber, int Soldiernumber)
    {
        // Soldier = transform.GetChild(randomnumber).gameObject;
        Soldier = Instantiate(transform.GetChild(Colornumber).gameObject.transform.GetChild(Soldiernumber).gameObject, transform.position, transform.rotation);
        //GameManager.instance.Soldiers.Add(Soldier);
        //Soldier.SetActive(false);
        Soldier.transform.position = RandomPosition(10, 0, 10);
        //Soldier.SetActive(true);
        return Soldier;

    }

    public void CreateWhiteUnit(int Colornumber, int Soldiernumber, Transform creatPosition)
    {
        Transform unitTransform = transform.GetChild(Colornumber).gameObject.transform.GetChild(Soldiernumber);

        Soldier = Instantiate(unitTransform.gameObject, creatPosition.position, creatPosition.rotation);
        Soldier.SetActive(true);
    }

    public void StoryModeCreateSoldier(int Colornumber, int Soldiernumber)
    {

        // Soldier = transform.GetChild(randomnumber).gameObject;
        Soldier = Instantiate(transform.GetChild(Colornumber).gameObject.transform.GetChild(Soldiernumber).gameObject, transform.position, transform.rotation);
        //GameManager.instance.Soldiers.Add(Soldier);

        Soldier.transform.position = new Vector3(500, 0, 10);
        Soldier.SetActive(true);

    }

    public void ExpenditureGold()
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
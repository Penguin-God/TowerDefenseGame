using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class MultiSoldiers
{
    [SerializeField] GameObject[] soldiers;
}

public class Multi_SoldierSpawner : MonoBehaviour
{
    [SerializeField] MultiSoldiers[] multiSoldiers;

    public void DrawSoldier(int Colornumber, int Soldiernumber)
    {
        if (Multi_GameManager.instance.Gold >= 5)
        {
            SpawnSoldier(Colornumber, Soldiernumber);
            Multi_GameManager.instance.AddGold(-5);
        }
    }

    public Multi_TeamSoldier SpawnSoldier(int Colornumber, int Soldiernumber)
    {

        return null;
    }

    public void CreateWhiteUnit(int Colornumber, int Soldiernumber, Transform creatPosition)
    {
        Transform unitTransform = transform.GetChild(Colornumber).gameObject.transform.GetChild(Soldiernumber);

        GameObject Soldier = Instantiate(unitTransform.gameObject, creatPosition.position, creatPosition.rotation);
        Soldier.SetActive(true);
    }

    public void StoryModeCreateSoldier(int Colornumber, int Soldiernumber)
    {

        // Soldier = transform.GetChild(randomnumber).gameObject;
        GameObject Soldier = Instantiate(transform.GetChild(Colornumber).gameObject.transform.GetChild(Soldiernumber).gameObject, transform.position, transform.rotation);
        //GameManager.instance.Soldiers.Add(Soldier);

        Soldier.transform.position = new Vector3(500, 0, 10);
        Soldier.SetActive(true);

    }

    Vector3 RandomPosition(float x, float y, float z)
    {
        float randomX = Random.Range(-x, x);
        float randomY = Random.Range(-y, y);
        float randomZ = Random.Range(-z, z);
        return new Vector3(randomX, randomY, randomZ);
    }
}
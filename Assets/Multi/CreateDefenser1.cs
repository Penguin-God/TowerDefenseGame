using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class CreateDefenser1 : MonoBehaviourPun
{
    // public GameObject Soldierprefab;
    /// <summary>
    ///  바뀐점: Instantiate를 PhotonNetwork.Instantiate로 바꾸고 프리팹 게임 오브젝트 대신 .name를 붙여 이름으로 대신함 프리팹을 Resources로 옮김.
    /// </summary>
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

    public void CreateSoldier(int Colornumber, int Soldiernumber)
    {

        // Soldier = transform.GetChild(randomnumber).gameObject;
        Soldier =  PhotonNetwork.Instantiate(transform.GetChild(Colornumber).gameObject.transform.GetChild(Soldiernumber).gameObject.name, transform.position, transform.rotation);
        //GameManager.instance.Soldiers.Add(Soldier);

        Soldier.transform.position = RandomPosition(10, 0, 10);
        Soldier.SetActive(true);

    }

    public void CreateSoldier(int Colornumber, int Soldiernumber, Transform creatPosition) // 박준이 만든 하얀유닛 소환용 함수
    {
        Transform unitTransform = transform.GetChild(Colornumber).gameObject.transform.GetChild(Soldiernumber);

        Soldier = PhotonNetwork.Instantiate(unitTransform.gameObject.name, creatPosition.position, creatPosition.rotation);
        Soldier.SetActive(true);
    }

    public void StoryModeCreateSoldier(int Colornumber, int Soldiernumber)
    {

        // Soldier = transform.GetChild(randomnumber).gameObject;
        Soldier = PhotonNetwork.Instantiate(transform.GetChild(Colornumber).gameObject.transform.GetChild(Soldiernumber).gameObject.name, transform.position, transform.rotation);
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
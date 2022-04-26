using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[System.Serializable]
public class Units 
{
    public GameObject[] units = null;
}


public class Multi_CreateDefenser : MonoBehaviourPun
{
    [SerializeField] Units[] allUnit;

    private GameObject Soldier;

    void Start()
    {
        Multi_EnemySpawner.instance.OnBossDead += Give_BossReword;
    }

    public void DrawSoldier(int Colornumber, int Soldiernumber)
    {
        if (Multi_GameManager.instance.Gold >= 5)
        {
            CreateSoldier(Colornumber, Soldiernumber);
            Multi_GameManager.instance.AddGold(-5);
        }
    }

    public void CreateSoldier(int Colornumber, int Soldiernumber)
    {
        Soldier =  
            PhotonNetwork.Instantiate(allUnit[Colornumber].units[Soldiernumber].name, Multi_WorldPosUtility.Instance.GetUnitSpawnPositon(), Quaternion.identity);
        Soldier.SetActive(true);
    }


    [PunRPC]
    public void CreateSoldier(int Colornumber, int Soldiernumber, Vector3 creatPos)
    {
        photonView.RPC("CreateClientSoldier", RpcTarget.Others, Colornumber, Soldiernumber, creatPos);
    }


    [PunRPC]
    void CreateClientSoldier(int Colornumber, int Soldiernumber, Vector3 creatPos)
    {
        Soldier = PhotonNetwork.Instantiate(allUnit[Colornumber].units[Soldiernumber].name, creatPos, Quaternion.identity);
        Soldier.SetActive(true);
    }

    void Give_BossReword(int _bossLevel)
    {
        switch (_bossLevel)
        {
            case 1: case 2: CreateSoldier(7, 1); break;
            case 3: case 4: CreateSoldier(7, 2); break;
        }
    }
}
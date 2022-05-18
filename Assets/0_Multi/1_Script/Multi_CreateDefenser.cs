using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_CreateDefenser : MonoBehaviourPun
{
    // TODO : UI 구조 바꾸면 Button 자체 기능으로 옮기기
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
        Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber);
    }

    [PunRPC]
    public void CreateSoldier(int Colornumber, int Soldiernumber, Vector3 creatPos)
    {
        Multi_SpawnManagers.NormalUnit.Spawn(Colornumber, Soldiernumber, creatPos);
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
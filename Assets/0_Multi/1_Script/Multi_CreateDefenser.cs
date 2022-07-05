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
}
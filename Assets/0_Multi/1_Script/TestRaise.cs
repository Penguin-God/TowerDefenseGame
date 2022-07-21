using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestRaise : MonoBehaviourPun
{
    RPCAction<int> test = new RPCAction<int>();
    [SerializeField] int value;
    void Awake()
    {
        test += Multi_GameManager.instance.AddGold;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("click");
            test.RaiseEvent(Multi_Data.instance.Id, value);
        }
    }
}

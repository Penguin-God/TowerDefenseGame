using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class DataLoadTest : MonoBehaviour
{
    void Start()
    {
        TestMultiManager();        
    }

    void TestMultiManager()
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        var go = Managers.Multi.Instantiater.PhotonInstantiate("Prefabs/Unit/Swordman/Yellow_SwordMan 1", Vector3.one * 10, new Vector3(0, 90, 0));
        Debug.Assert(go.name == "Yellow_SwordMan 1");
        Managers.Multi.Instantiater.PhotonDestroy(go);
        Debug.Assert(go.transform.parent.name == "Yellow_SwordMan 1_Root");
    }
}

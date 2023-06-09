using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MultiLoading : MonoBehaviourPun
{
    [SerializeField] Text stateText = null;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            EnterBattle();
        }
    }

    [PunRPC] void EnterBattle() => Managers.Scene.LoadLevel(SceneTyep.Battle);
}

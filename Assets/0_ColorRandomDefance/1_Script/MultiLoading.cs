using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MultiLoading : MonoBehaviourPun
{
    [SerializeField] Text stateText = null;

    void Update()
    {
        stateText.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        if (Input.GetKeyDown(KeyCode.E)) EnterBattle();
    }

    void Start()
    {
        StartCoroutine(Co_Load());
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    IEnumerator Co_Load()
    {
        yield return new WaitUntil(() => PhotonNetwork.CurrentRoom.PlayerCount == 2);
        EnterBattle();
    }

    [PunRPC] void EnterBattle() => Managers.Scene.LoadLevel(SceneTyep.Battle);
}

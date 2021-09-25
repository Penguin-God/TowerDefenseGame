﻿using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class StoryMode : MonoBehaviourPun
{
    /// <summary>
    ///  Enter Story Mode에 if (!photonView.IsMine)
        //{
            //return;
        //} 를 추가함
/// </summary>
    [SerializeField]
    private GameObject unitStoryModeEnterButton;
    [SerializeField]
    private GameObject unitBackFiledButton;
    
    public AudioSource EnterStoryModeAudio;
    public string unitTagName = "";

    public void TranslateUnit()
    {
        GameObject[] moveUnits = GameObject.FindGameObjectsWithTag(unitTagName);
        if (moveUnits.Length == 0) return;

        for(int i = 0; i < moveUnits.Length; i++)
        {
            TeamSoldier teamSoldier = moveUnits[i].GetComponent<TeamSoldier>();
            if (teamSoldier.enterStoryWorld == GameManager.instance.playerEnterStoryMode) // unit과 player의 입장 필드가 같으면
            {
                teamSoldier.Unit_WorldChange();
                break;
            }
        }
    }

    public Text enterButtonText;
    public GameObject currentUnitWindow = null;
    public void EnterStoryMode()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        EnterStoryModeAudio.Play();
        if (!GameManager.instance.playerEnterStoryMode) SetMapValue("필드로", new Vector3(500, 100, -62));
        else SetMapValue("적군의 성으로", new Vector3(0, 100, -62));
    }

    void SetMapValue(string p_text, Vector3 camPosition)
    {
        enterButtonText.text = p_text;
        Camera.main.gameObject.transform.position = camPosition;
        GameManager.instance.playerEnterStoryMode = !GameManager.instance.playerEnterStoryMode;
        if(currentUnitWindow != null) currentUnitWindow.SetActive(false);
    }
}

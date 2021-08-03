using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientManager : MonoBehaviour
{
    public Text IronText;
    public Text WoodText;

    void Start()
    {
        PlayerPrefs.GetInt("Iron");
        PlayerPrefs.GetInt("Wood");
        UpdateWoodText(GameManager.instance.Wood);
        UpdateIronText(GameManager.instance.Iron);
    }

    void UpdateIronText(int Iron)
    {
        IronText.text = "" + Iron;
    }

    void UpdateWoodText(int Wood)
    {
        WoodText.text = "" + Wood;
    }



}

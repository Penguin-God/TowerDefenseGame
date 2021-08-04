using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientManager : MonoBehaviour
{
    public Text IronText;
    public Text WoodText;
    int ClientWood; 
    int ClientIron; 


    void Start()
    {
        ClientWood = PlayerPrefs.GetInt("Wood");
        ClientIron = PlayerPrefs.GetInt("Iron");
        UpdateWoodText(ClientWood);
        UpdateIronText(ClientIron);
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

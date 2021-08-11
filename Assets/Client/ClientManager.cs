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

    public void BuyStartGold()
    {
        if (ClientIron >= 1)
        {
            ClientIron -= 1;
            PlayerPrefs.SetInt("Iron", ClientIron);
            UpdateIronText(ClientIron);
        }
        
    }

    public void BuyStartFood()
    {
        if (ClientWood >= 1)
        {
            ClientWood -= 1;
            PlayerPrefs.SetInt("Wood", ClientWood);
            UpdateWoodText(ClientWood);
            
        }
    }


}

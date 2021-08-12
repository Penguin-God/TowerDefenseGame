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
    int GoldCount;
    int FoodCount;
    int StartGold;
    int StartFood;


    void Start()
    {
        GoldCount = PlayerPrefs.GetInt("GoldCount");
        FoodCount = PlayerPrefs.GetInt("FoodCount");
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
            StartGold = 10 * (GoldCount + 1);
            PlayerPrefs.SetInt("StartGold", StartGold);
            PlayerPrefs.SetInt("Iron", ClientIron);
            UpdateIronText(ClientIron);
            GoldCount += 1;
            PlayerPrefs.SetInt("GoldCount", GoldCount);
        }
        
    }

    public void BuyStartFood()
    {
        if (ClientWood >= 1)
        {
            ClientWood -= 1;
            StartFood = 10 * (FoodCount + 1);
            PlayerPrefs.SetInt("StartFood", StartFood);
            PlayerPrefs.SetInt("Wood", ClientWood);
            UpdateWoodText(ClientWood);
            FoodCount += 1;
            PlayerPrefs.SetInt("FoodCount", FoodCount);
            
        }
    }


}

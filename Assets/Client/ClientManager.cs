using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientManager : MonoBehaviour
{
    public Text IronText;
    public Text WoodText;
    public Text StartGoldPriceText;
    public Text StartFoodPriceText;
    int ClientWood; 
    int ClientIron;
    int GoldCount;
    int FoodCount;
    int StartGold;
    int StartFood;
    int StartGoldPrice;
    int StartFoodPrice;


    void Start()
    {
        StartGoldPrice = PlayerPrefs.GetInt("StartGoldPrice");
        StartFoodPrice = PlayerPrefs.GetInt("StartFoodPrice");
        GoldCount = PlayerPrefs.GetInt("GoldCount");
        FoodCount = PlayerPrefs.GetInt("FoodCount");
        ClientWood = PlayerPrefs.GetInt("Wood");
        ClientIron = PlayerPrefs.GetInt("Iron");
        StartGold = PlayerPrefs.GetInt("StartGold");
        StartFood = PlayerPrefs.GetInt("StartFood");
        PlayerPrefs.SetInt("StartGoldPrice", StartGold);
        PlayerPrefs.SetInt("StartFoodPrice", StartFood * 10);
        UpdateWoodText(ClientWood);
        UpdateIronText(ClientIron);
        UpdateStartGoldPrice();
        UpdateStartFoodPrice();
    }

    void UpdateIronText(int Iron)
    {
        IronText.text = "" + Iron;
    }

    void UpdateWoodText(int Wood)
    {
        WoodText.text = "" + Wood;
    }

    public void UpdateStartGoldPrice()
    {
        StartGoldPriceText.text = "철 " + StartGoldPrice + "개";
    }

    public void UpdateStartFoodPrice()
    {
        StartFoodPriceText.text = "나무 " + StartFoodPrice + "개";
    }

    public void BuyStartGold()
    {
        if (ClientIron >= StartGoldPrice)
        {
            ClientIron -= StartGoldPrice;
            StartGold = 10 * (GoldCount + 1);
            PlayerPrefs.SetInt("StartGoldPrice", StartGold);
            PlayerPrefs.SetInt("StartGold", StartGold);
            PlayerPrefs.SetInt("Iron", ClientIron);
            UpdateIronText(ClientIron);
            GoldCount += 1;
            PlayerPrefs.SetInt("GoldCount", GoldCount);
        }
        
    }

    public void BuyStartFood()
    {
        if (ClientWood >= StartFoodPrice)
        {
            ClientWood -= StartFoodPrice;
            StartFood = 1 * (FoodCount + 1);
            PlayerPrefs.SetInt("StartFoodPrice", StartFood * 10);
            PlayerPrefs.SetInt("StartFood", StartFood);
            PlayerPrefs.SetInt("Wood", ClientWood);
            UpdateWoodText(ClientWood);
            FoodCount += 1;
            PlayerPrefs.SetInt("FoodCount", FoodCount);
            
        }
    }


}

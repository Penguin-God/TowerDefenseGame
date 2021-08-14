﻿using System.Collections;
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
    public AudioSource ClientClickAudioSource;


    void Start()
    {
        StartGoldPrice = PlayerPrefs.GetInt("StartGoldPrice");
        StartFoodPrice = PlayerPrefs.GetInt("StartFoodPrice");
        StartGold = PlayerPrefs.GetInt("StartGold");
        StartFood = PlayerPrefs.GetInt("StartFood");
        if (StartGoldPrice == 0)
        {
            PlayerPrefs.SetInt("StartGoldPrice", 10);
            StartGoldPrice = PlayerPrefs.GetInt("StartGoldPrice");

            
        }
        else
        {
            PlayerPrefs.SetInt("StartGoldPrice", StartGold + 10);
            StartGoldPrice = PlayerPrefs.GetInt("StartGoldPrice");
            
        }
        if (StartFoodPrice == 0)
        {
            PlayerPrefs.SetInt("StartFoodPrice", 10);
            StartFoodPrice = PlayerPrefs.GetInt("StartFoodPrice");
        }
        else
        {
            PlayerPrefs.SetInt("StartFoodPrice", (StartFood + 1) * 10);
            StartFoodPrice = PlayerPrefs.GetInt("StartFoodPrice");
        }
        GoldCount = PlayerPrefs.GetInt("GoldCount");
        FoodCount = PlayerPrefs.GetInt("FoodCount");
        ClientWood = PlayerPrefs.GetInt("Wood");
        ClientIron = PlayerPrefs.GetInt("Iron");     
        UpdateWoodText(ClientWood);
        UpdateIronText(ClientIron);
        UpdateStartGoldPrice();
        UpdateStartFoodPrice();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
        }
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
    public void ClientClickSound()
    {
        ClientClickAudioSource.Play();
    }

    public void BuyStartGold()
    {
        ClientClickSound();
        if (ClientIron >= StartGoldPrice)
        {
            ClientIron -= StartGoldPrice;
            StartGold = 10 * (GoldCount + 1);
            PlayerPrefs.SetInt("StartGoldPrice", StartGold + 10);
            PlayerPrefs.SetInt("StartGold", StartGold);
            PlayerPrefs.SetInt("Iron", ClientIron);
            ClientIron = PlayerPrefs.GetInt("Iron");
            StartGoldPrice = PlayerPrefs.GetInt("StartGoldPrice");
            UpdateStartGoldPrice();
            UpdateIronText(ClientIron);
            GoldCount += 1;
            PlayerPrefs.SetInt("GoldCount", GoldCount);
        }
        
    }

    public void BuyStartFood()
    {
        ClientClickSound();
        if (ClientWood >= StartFoodPrice)
        {
            ClientWood -= StartFoodPrice;
            StartFood = 1 * (FoodCount + 1);
            PlayerPrefs.SetInt("StartFoodPrice", (StartFood + 1) * 10);
            PlayerPrefs.SetInt("StartFood", StartFood);
            PlayerPrefs.SetInt("Wood", ClientWood);
            ClientWood = PlayerPrefs.GetInt("Wood");
            StartFoodPrice = PlayerPrefs.GetInt("StartFoodPrice");
            UpdateStartFoodPrice();
            UpdateWoodText(ClientWood);
            FoodCount += 1;
            PlayerPrefs.SetInt("FoodCount", FoodCount);
            
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientManager : MonoBehaviour
{
    public Text IronText;
    public Text WoodText;
    public Text HammerText;
    public Text StartGoldPriceText;
    public Text StartFoodPriceText;
    public Text PlusTouchDamegePriceText;
    int ClientWood; 
    int ClientIron;
    int ClientHammer;
    int GoldCount;
    int FoodCount;
    int HammerCount;
    int StartGold;
    int StartFood;
    int PlusTouchDamege;
    int StartGoldPrice;
    int StartFoodPrice;
    int PlusTouchDamegePrice;
    public AudioSource ClientClickAudioSource;


    void Start()
    {
        StartGoldPrice = PlayerPrefs.GetInt("StartGoldPrice");
        StartFoodPrice = PlayerPrefs.GetInt("StartFoodPrice");
        PlusTouchDamegePrice = PlayerPrefs.GetInt("PlusTouchDamegePrice");
        StartGold = PlayerPrefs.GetInt("StartGold");
        StartFood = PlayerPrefs.GetInt("StartFood");
        PlusTouchDamege = PlayerPrefs.GetInt("PlusTouchDamege");
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
        if (PlusTouchDamegePrice == 0)
        {
            PlayerPrefs.SetInt("PlusTouchDamegePrice",1);
            PlusTouchDamegePrice = PlayerPrefs.GetInt("PlusTouchDamegePrice");
        }
        else
        {
            PlayerPrefs.SetInt("PlusTouchDamegePrice", (PlusTouchDamege + 1) * 1);
        }
        GoldCount = PlayerPrefs.GetInt("GoldCount");
        FoodCount = PlayerPrefs.GetInt("FoodCount");
        HammerCount = PlayerPrefs.GetInt("HammerCount");
        ClientWood = PlayerPrefs.GetInt("Wood");
        ClientIron = PlayerPrefs.GetInt("Iron");
        ClientHammer = PlayerPrefs.GetInt("Hammer");
        UpdateWoodText(ClientWood);
        UpdateIronText(ClientIron);
        UpdateHammerText(ClientHammer);
        UpdateStartGoldPrice();
        UpdateStartFoodPrice();
        UpdatePlusTouchDamegePrice();
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

    void UpdateHammerText(int Hammer)
    {
        HammerText.text = "" + Hammer;
    }

    public void UpdateStartGoldPrice()
    {
        StartGoldPriceText.text = "철 " + StartGoldPrice + "개";
    }

    public void UpdateStartFoodPrice()
    {
        StartFoodPriceText.text = "나무 " + StartFoodPrice + "개";
    }

    public void UpdatePlusTouchDamegePrice()
    {
        PlusTouchDamegePriceText.text = "망치 " + PlusTouchDamegePrice + "개";
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
    public void BuyPlusTouchDamege()
    {
        ClientClickSound();
        if (ClientHammer >= PlusTouchDamegePrice)
        {
            ClientHammer -= PlusTouchDamegePrice;
            PlusTouchDamege = 1 * (HammerCount + 1);
            PlayerPrefs.SetInt("PlusTouchDamegePrice", (PlusTouchDamege + 1));
            PlayerPrefs.SetInt("PlusTouchDamege", PlusTouchDamege);
            PlayerPrefs.SetInt("Hammer", ClientHammer);
            ClientHammer = PlayerPrefs.GetInt("Hammer");
            PlusTouchDamegePrice = PlayerPrefs.GetInt("PlusTouchDamegePrice");
            UpdatePlusTouchDamegePrice();
            UpdateHammerText(ClientHammer);
            HammerCount += 1;
            PlayerPrefs.SetInt("HammerCount", HammerCount);

        }
    }


}

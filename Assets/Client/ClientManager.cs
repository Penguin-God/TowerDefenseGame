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
    public Text PlusMaxUnitPriceText;
    int ClientWood; 
    int ClientIron;
    int ClientHammer;
    int GoldCount;
    int FoodCount;
    int HammerCount;
    int PlusMaxUnitCount;
    int StartGold;
    int StartFood;
    int PlusTouchDamege;
    int PlusMaxUnit;
    int StartGoldPrice;
    int StartFoodPrice;
    int PlusTouchDamegePrice;
    int PlusMaxUnitPrice;
    int StartGameCount;
    public AudioSource ClientClickAudioSource;


    void Start()
    {
        StartGameCount = PlayerPrefs.GetInt("StartGameCount");
        if (StartGameCount == 0)
        {
            PlayerPrefs.SetInt("Iron", 500);
            PlayerPrefs.SetInt("Wood", 500);
            PlayerPrefs.SetInt("Hemmer", 30);
            PlayerPrefs.SetInt("StartGameCount", 1000);
            StartGameCount = PlayerPrefs.GetInt("StartGameCount");
        }
        StartGoldPrice = PlayerPrefs.GetInt("StartGoldPrice");
        StartFoodPrice = PlayerPrefs.GetInt("StartFoodPrice");
        PlusTouchDamegePrice = PlayerPrefs.GetInt("PlusTouchDamegePrice");
        PlusMaxUnitPrice = PlayerPrefs.GetInt("PlusMaxUnitPrice");
        StartGold = PlayerPrefs.GetInt("StartGold");
        StartFood = PlayerPrefs.GetInt("StartFood");
        PlusTouchDamege = PlayerPrefs.GetInt("PlusTouchDamege");
        PlusMaxUnit = PlayerPrefs.GetInt("PlusMaxUnit");
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
        if (PlusMaxUnitPrice == 0)
        {
            PlayerPrefs.SetInt("PlusMaxUnitPrice", 10);
            PlusMaxUnitPrice = PlayerPrefs.GetInt("PlusMaxUnitPrice");
        }
        else
        {
            PlayerPrefs.SetInt("PlusMaxUnitPrice", (PlusMaxUnit + 1) * 10);
        }
        GoldCount = PlayerPrefs.GetInt("GoldCount");
        FoodCount = PlayerPrefs.GetInt("FoodCount");
        HammerCount = PlayerPrefs.GetInt("HammerCount");
        PlusMaxUnitCount = PlayerPrefs.GetInt("PlusMaxUnitCount");
        ClientWood = PlayerPrefs.GetInt("Wood");
        ClientIron = PlayerPrefs.GetInt("Iron");
        ClientHammer = PlayerPrefs.GetInt("Hammer");
        UpdateWoodText(ClientWood);
        UpdateIronText(ClientIron);
        UpdateHammerText(ClientHammer);
        UpdateStartGoldPrice();
        UpdateStartFoodPrice();
        UpdatePlusTouchDamegePrice();
        UpdatePlusMaxUnitPrice();
        UpdateAdIronCount(StartGoldPrice);
        UpdateAdWoodCount(StartFoodPrice);
        UpdateAdHammerCount(PlusTouchDamegePrice);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void UpdateIronText(int Iron)
    {
        IronText.text = "" + Iron;
    }

    public void UpdateWoodText(int Wood)
    {
        WoodText.text = "" + Wood;
    }

    public void UpdateHammerText(int Hammer)
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

    public void UpdatePlusMaxUnitPrice()
    {
        PlusMaxUnitPriceText.text = "망치 " + PlusMaxUnitPrice + "개";
    }
    public void ClientClickSound()
    {
        ClientClickAudioSource.Play();
    }

    public Text AdIronCount;
    public void UpdateAdIronCount(int StartGoldPrice)
    {
        AdIronCount.text = "10 ~ " + StartGoldPrice;
    }

    public Text AdWoodCount;
    public void UpdateAdWoodCount(int StartFoodPrice)
    {
        AdWoodCount.text = "10 ~ " + StartFoodPrice;
    }

    public Text AdHammerCount;
    public void UpdateAdHammerCount(int PlusTouchDamegePrice)
    {
        AdHammerCount.text = "1 ~ " + PlusTouchDamegePrice;
    }

    public void BuyStartGold()
    {
        ClientClickSound();
        ClientIron = PlayerPrefs.GetInt("Iron");
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
            UpdateAdIronCount(StartGoldPrice);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("실패");
        }

    }

    public void BuyStartFood()
    {
        ClientWood = PlayerPrefs.GetInt("Wood");
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
            UpdateAdWoodCount(StartFoodPrice);
            PlayerPrefs.Save();

        }
        else
        {
            Debug.Log("실패");
        }
    }
    public void BuyPlusTouchDamege()
    {
        ClientHammer = PlayerPrefs.GetInt("Hammer");
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
            UpdateAdHammerCount(PlusTouchDamegePrice);
            PlayerPrefs.Save();

        }
        else
        {
            Debug.Log("실패");
        }
    }

    public void BuyPlusMaxUnit()
    {
        ClientHammer = PlayerPrefs.GetInt("Hammer");
        ClientClickSound();
        if (ClientHammer >= PlusMaxUnitPrice && PlusMaxUnitCount <= 4)
        {
            ClientHammer -= PlusMaxUnitPrice;
            PlusMaxUnit = 1 * (PlusMaxUnitCount + 1);
            PlayerPrefs.SetInt("PlusMaxUnitPrice", (PlusMaxUnit + 1) * 10);
            PlayerPrefs.SetInt("PlusMaxUnit", PlusMaxUnit);
            PlayerPrefs.SetInt("Hammer", ClientHammer);
            ClientHammer = PlayerPrefs.GetInt("Hammer");
            PlusMaxUnitPrice = PlayerPrefs.GetInt("PlusMaxUnitPrice");
            UpdatePlusMaxUnitPrice();
            UpdateHammerText(ClientHammer);
            PlusMaxUnitCount += 1;
            PlayerPrefs.SetInt("PlusMaxUnitCOunt", PlusMaxUnitCount);
            PlayerPrefs.SetInt("PlusMaxUnit", PlusMaxUnit);
            PlayerPrefs.Save();

        }
        else
        {
            Debug.Log("실패");
            if (PlusMaxUnitCount >= 5)
            {
                // 텍스트 바꾸기
            }
        }
    }

    //public Image Category1;
    //public Image Category2;
    //public Image Category3;
    //public void TouchCategory1()
    //{
       
    //}

    //public void TouchCategory2()
    //{

    //}

    //public void TouchCategory3()
    //{

    //}


}

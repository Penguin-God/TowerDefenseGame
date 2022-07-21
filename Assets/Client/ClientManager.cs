using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class ClientManager : MonoBehaviour
{
    public Text IronText;
    public Text WoodText;
    public Text HammerText;
    public Text StartGoldPriceText;
    public Text StartFoodPriceText;
    public Text PlusTouchDamegePriceText;
    public Text PlusMaxUnitPriceText;
    public Text GoldSkillPriceText;

    int ClientWood; 
    int ClientIron;
    int ClientHammer;

    const int STARTGOLDPRICE = 3000;
    const int STARTFOODPRICE = 3000;
    const int PLUSTOUCHDAMEGEPRICE = 3000;
    const int PLUSMAXUNITPRICE = 3000;
    
    int Skill1;
    public AudioSource ClientClickAudioSource;

    void Start()
    {
        // 임시 코드
        EventIdManager.Reset();

        ClientIron = Multi_Managers.ClientData.MoneyByType[MoneyType.Iron].Amount;
        ClientWood = Multi_Managers.ClientData.MoneyByType[MoneyType.Wood].Amount;
        ClientHammer = Multi_Managers.ClientData.MoneyByType[MoneyType.Hammer].Amount;
        //print("결과 ==============================================");
        //print(ClientIron);
        //print(ClientWood);
        //print(ClientHammer);
        UpdateWoodText(ClientWood);
        UpdateIronText(ClientIron);
        UpdateHammerText(ClientHammer);
        //UpdateAdIronCount(STARTGOLDPRICE);
        //UpdateAdWoodCount(STARTFOODPRICE);
        //UpdateAdHammerCount(PLUSTOUCHDAMEGEPRICE);
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.P)) // p 누르면 데이터 삭제
        {
            PlayerPrefs.DeleteAll();
        }
        if (Input.GetKeyDown(KeyCode.A)) // a 누르면 돈복사
        {
            ClientIron += 10000;
            ClientWood += 10000;
            ClientHammer += 10000;
            InitMoney();
            UpdateMoney();
        }
    }

    public void UpdateMoney()
    {
        UpdateIronText(ClientIron);
        UpdateWoodText(ClientWood);
        UpdateHammerText(ClientHammer);
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

    public void BuySkills(ref int use_money, int use_price, SkillType skillType, MoneyType moneyType)
    {
        ClientClickSound();
        use_money = Multi_Managers.ClientData.MoneyByType[moneyType].Amount;
        if (use_money >= use_price && !(Multi_Managers.ClientData.SkillByType[skillType].HasSkill))
        {
            Debug.Log($"{skillType} 구매");
            use_money -= use_price;
            Multi_Managers.ClientData.SkillByType[skillType].SetHasSkill(true);

            Multi_Managers.ClientData.MoneyByType[moneyType].SetAmount(use_money);
            use_money = Multi_Managers.ClientData.MoneyByType[moneyType].Amount;

            UpdateMoney();
        }
        else
        {
            Debug.Log("실패");
        }
    }

    public void BuyStartGold()
    {
        BuySkills(ref ClientIron, STARTGOLDPRICE, SkillType.시작골드증가, MoneyType.Iron);
        //ClientClickSound();
        //ClientIron = Multi_Managers.ClientData.MoneyByType[MoneyType.Iron].Amount;
        //if (ClientIron >= STARTGOLDPRICE && !(Multi_Managers.ClientData.SkillByType[SkillType.시작골드증가].HasSkill))
        //{
        //    Debug.Log("시작 골드 구매");
        //    ClientIron -= STARTGOLDPRICE;
        //    Multi_Managers.ClientData.SkillByType[SkillType.시작골드증가].SetHasSkill(true);
        //    InitMoney();
        //    UpdateIronText(ClientIron);
        //}
        //else
        //{
        //    Debug.Log("실패");
        //}

    }

    public void BuyStartFood()
    {
        BuySkills(ref ClientWood, STARTFOODPRICE, SkillType.시작식량증가, MoneyType.Wood);
        //ClientClickSound();
        //ClientWood = Multi_Managers.ClientData.MoneyByType[MoneyType.Wood].Amount;
        //if (ClientWood >= STARTFOODPRICE && !(Multi_Managers.ClientData.SkillByType[SkillType.시작식량증가].HasSkill))
        //{
        //    Debug.Log("시작 식량 구매");
        //    ClientWood -= STARTFOODPRICE;
        //    Multi_Managers.ClientData.SkillByType[SkillType.시작식량증가].SetHasSkill(true);
        //    InitMoney();
        //    UpdateWoodText(ClientWood);

        //}
        //else
        //{
        //    Debug.Log("실패");
        //}
    }

    public void BuyPlusMaxUnit()
    {
        BuySkills(ref ClientHammer, PLUSMAXUNITPRICE, SkillType.최대유닛증가, MoneyType.Hammer);
        //ClientClickSound();
        //ClientHammer = Multi_Managers.ClientData.MoneyByType[MoneyType.Hammer].Amount;
        //if (ClientHammer >= PLUSMAXUNITPRICE && !(Multi_Managers.ClientData.SkillByType[SkillType.최대유닛증가].HasSkill))
        //{
        //    Debug.Log("최대 유닛 구매");
        //    ClientHammer -= PLUSMAXUNITPRICE;
        //    Multi_Managers.ClientData.SkillByType[SkillType.최대유닛증가].SetHasSkill(true);
        //    InitMoney();
        //    UpdateHammerText(ClientHammer);
            

        //}
        //else
        //{
        //    Debug.Log("실패");
        //}
    }

    //public void BuyPlusTouchDamege()
    //{
    //    ClientHammer = PlayerPrefs.GetInt("Hammer");
    //    ClientClickSound();
    //    if (ClientHammer >= PlusTouchDamegePrice)
    //    {
    //        Debug.Log("터치뎀");
    //        ClientHammer -= PlusTouchDamegePrice;
    //        PlusTouchDamege = 1 * (HammerCount + 1);
    //        PlayerPrefs.SetInt("PlusTouchDamegePrice", (PlusTouchDamege + 1));
    //        PlayerPrefs.SetInt("PlusTouchDamege", PlusTouchDamege);
    //        InitMoney();
    //        PlusTouchDamegePrice = PlayerPrefs.GetInt("PlusTouchDamegePrice");
    //        UpdatePlusTouchDamegePrice();
    //        UpdateHammerText(ClientHammer);
    //        HammerCount += 1;
    //        PlayerPrefs.SetInt("HammerCount", HammerCount);
    //        UpdateAdHammerCount(PlusTouchDamegePrice);
    //        PlayerPrefs.Save();

    //    }
    //    else
    //    {

    //        Debug.Log(PlusTouchDamegePrice);
    //        Debug.Log(ClientHammer);
    //        Debug.Log("실패");
    //    }
    //}

    public void BuyGoldSkill()
    {
        
        ClientWood = PlayerPrefs.GetInt("Wood");
        ClientIron = PlayerPrefs.GetInt("Iron");
        ClientHammer = PlayerPrefs.GetInt("Hammer");
        Skill1 = PlayerPrefs.GetInt("Skill1");

        ClientClickSound();

        if (ClientHammer >= 30 && ClientIron >= 1000 && ClientWood >= 1000 && Skill1 == 0)
        {
            ClientHammer -= 30;
            ClientIron -= 1000;
            ClientWood -= 1000;


            InitMoney();

            
            UpdateHammerText(ClientHammer);

            PlayerPrefs.SetInt("Skill1", 1);
            Skill1 = PlayerPrefs.GetInt("Skill1");
            PlayerPrefs.Save();

        }

        else if (Skill1 == 1)
        {
            Debug.Log("품절");
        }
        else
        {
            Debug.Log("실패");
        }
    }

    private void InitMoney()
    {
        Multi_Managers.ClientData.MoneyByType[MoneyType.Iron].SetAmount(ClientIron);
        Multi_Managers.ClientData.MoneyByType[MoneyType.Wood].SetAmount(ClientWood);
        Multi_Managers.ClientData.MoneyByType[MoneyType.Hammer].SetAmount(ClientHammer);

        ClientIron = Multi_Managers.ClientData.MoneyByType[MoneyType.Iron].Amount;
        ClientWood = Multi_Managers.ClientData.MoneyByType[MoneyType.Wood].Amount;
        ClientHammer = Multi_Managers.ClientData.MoneyByType[MoneyType.Hammer].Amount;
    }


}

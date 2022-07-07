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

    // 에러떠서 주석처리함
    // Multi_ClientData ClientData = Multi_Managers.ClientData;

    void Start()
    {
        //ClientData.Skills = Multi_Managers.ClientData.Load(Multi_Managers.ClientData.Skills, Multi_ClientData.Skill.path);
        //ClientData.Moneys = Multi_Managers.ClientData.Load(Multi_Managers.ClientData.Moneys, Multi_ClientData.Money.path);

        //print(ClientData.SkillByType[SkillType.시작골드증가].HasSkill);
        //print(ClientData.MoneyByType[MoneyType.Hammer].Amount);

        //ClientData.SkillByType[SkillType.시작골드증가].SetHasSkill(true);
        //print("======================추가 전 코드=======================");
        //Debug.Log(Multi_Managers.ClientData.Skills.Count);
        //Debug.Log(Multi_Managers.ClientData.Moneys.Count);
        //Debug.Log(Multi_Managers.ClientData.Moneys[0].Name);
        //Debug.Log(Multi_Managers.ClientData.Moneys[0].Id);
        //Debug.Log($"{Multi_Managers.ClientData.Moneys[0].Amount} 출력");

        
        ClientIron = Multi_Managers.ClientData.MoneyByType[MoneyType.Iron].Amount;
        ClientWood = Multi_Managers.ClientData.MoneyByType[MoneyType.Wood].Amount;
        ClientHammer = Multi_Managers.ClientData.MoneyByType[MoneyType.Hammer].Amount;
        //print("결과 ==============================================");
        //print(ClientIron);
        //print(ClientWood);
        //print(ClientHammer);
        //UpdateWoodText(ClientWood);
        //UpdateIronText(ClientIron);
        //UpdateHammerText(ClientHammer);
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
            Multi_Managers.ClientData.MoneyByType[MoneyType.Iron].SetAmount(ClientIron);
            Multi_Managers.ClientData.MoneyByType[MoneyType.Wood].SetAmount(ClientWood);
            Multi_Managers.ClientData.MoneyByType[MoneyType.Hammer].SetAmount(ClientHammer);
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
        ClientIron = Multi_Managers.ClientData.MoneyByType[MoneyType.Iron].Amount;
        if (ClientIron >= STARTGOLDPRICE && !(Multi_Managers.ClientData.SkillByType[SkillType.시작골드증가].HasSkill))
        {
            ClientIron -= STARTGOLDPRICE;
            Multi_Managers.ClientData.SkillByType[SkillType.시작골드증가].SetHasSkill(true);
            InitMoney();
            UpdateIronText(ClientIron);
        }
        else
        {
            Debug.Log("실패");
        }

    }

    public void BuyStartFood()
    {
        ClientClickSound();
        ClientWood = Multi_Managers.ClientData.MoneyByType[MoneyType.Wood].Amount;
        if (ClientWood >= STARTFOODPRICE && !(Multi_Managers.ClientData.SkillByType[SkillType.시작식량증가].HasSkill))
        {
            ClientWood -= STARTFOODPRICE;
            Multi_Managers.ClientData.SkillByType[SkillType.시작식량증가].SetHasSkill(true);
            InitMoney();
            UpdateWoodText(ClientWood);

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
        if (ClientHammer >= PLUSMAXUNITPRICE && !(Multi_Managers.ClientData.SkillByType[SkillType.최대유닛증가].HasSkill))
        {
            Debug.Log("최대 유닛");
            ClientHammer -= PLUSMAXUNITPRICE;
            Multi_Managers.ClientData.SkillByType[SkillType.최대유닛증가].SetHasSkill(true);
            InitMoney();
            UpdateHammerText(ClientHammer);
            

        }
        else
        {
            Debug.Log("실패");
        }
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

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

    public Button StartGoldEquipButton;
    public Button PlusMaxUnitEquipButton;
    public Button TaegeukSkillEquipButton;
    public Button BlackUnitUpgradeEquipButton;
    public Button YellowUnitUpgradeEquipButton;
    public Button ColorChangeEquipButton;
    public Button CommonSkillEquipButton;
    public Button FoodHaterEquipButton;
    public Button SellUpgradeEquipButton;
    public Button BossDamageUpgradeEquipButton;

    public Image Skill1Image;
    public Image Skill2Image;

    public Skill_Image skill_Image;

    int ClientWood; 
    int ClientIron;
    int ClientHammer;

    const int STARTGOLDPRICE = 3000;
    const int TAEGEUKSKILLPRICE = 3000;
    const int PLUSMAXUNITPRICE = 3000;
    
    public AudioSource ClientClickAudioSource;

    void Start()
    {
        // 임시 코드
        //EventIdManager.Reset();

        Skill_Image skill_Image = GetComponent<Skill_Image>();

        ClientIron = Multi_Managers.ClientData.MoneyByType[MoneyType.Iron].Amount;
        ClientWood = Multi_Managers.ClientData.MoneyByType[MoneyType.Wood].Amount;
        ClientHammer = Multi_Managers.ClientData.MoneyByType[MoneyType.Hammer].Amount;

        UpdateWoodText(ClientWood);
        UpdateIronText(ClientIron);
        UpdateHammerText(ClientHammer);
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
    }

    public void BuyPlusMaxUnit()
    {
        BuySkills(ref ClientHammer, PLUSMAXUNITPRICE, SkillType.최대유닛증가, MoneyType.Hammer);
    }

    public void BuyTaegeukSkill()
    {
        BuySkills(ref ClientIron, TAEGEUKSKILLPRICE, SkillType.태극스킬, MoneyType.Iron);
    }

    public void BuyBlackUnitUpgrade()
    {
        BuySkills(ref ClientIron, TAEGEUKSKILLPRICE, SkillType.검은유닛강화, MoneyType.Iron);
    }

    public void BuyYellowUnitUpgrade()
    {
        BuySkills(ref ClientIron, TAEGEUKSKILLPRICE, SkillType.노란기사강화, MoneyType.Iron);
    }

    public void BuyColorChange()
    {
        BuySkills(ref ClientIron, TAEGEUKSKILLPRICE, SkillType.상대색깔변경, MoneyType.Iron);
    }

    public void BuyCommonSkill()
    {
        BuySkills(ref ClientIron, TAEGEUKSKILLPRICE, SkillType.흔한스킬, MoneyType.Iron);
    }

    public void BuyFoodHater()
    {
        BuySkills(ref ClientIron, TAEGEUKSKILLPRICE, SkillType.고기혐오자, MoneyType.Iron);
    }

    public void BuySellUpgrade()
    {
        BuySkills(ref ClientIron, TAEGEUKSKILLPRICE, SkillType.판매보상증가, MoneyType.Iron);
    }

    public void BuyBossDamageUpgrade()
    {
        BuySkills(ref ClientIron, TAEGEUKSKILLPRICE, SkillType.보스데미지증가, MoneyType.Iron);
    }


    public void EquipStartGold()
    {
        if (EquipSkill(SkillType.시작골드증가) == false)
            return;

        if (CheckSkill() == 1)
            Skill1Image.sprite = skill_Image.StratGoldImage;
        else if (CheckSkill() == 2)
            Skill2Image.sprite = skill_Image.StratGoldImage;
    }

    public void EquipPlusMaxUnit()
    {
        if (EquipSkill(SkillType.최대유닛증가) == false)
            return;


        if (CheckSkill() == 1)
            Skill1Image.sprite = skill_Image.PlusMaxUnitImage;
        else if (CheckSkill() == 2)
            Skill2Image.sprite = skill_Image.PlusMaxUnitImage;
    }

    public void EquipTaegeukSkill()
    {
        if (EquipSkill(SkillType.태극스킬) == false)
            return;


        if (CheckSkill() == 1)
            Skill1Image.sprite = skill_Image.TaegeukSkillImage;
        else if (CheckSkill() == 2)
            Skill2Image.sprite = skill_Image.TaegeukSkillImage;
    }

    public void EquipBlackUnitUpgrade()
    {
        if (EquipSkill(SkillType.검은유닛강화) == false)
            return;


        if (CheckSkill() == 1)
            Skill1Image.sprite = skill_Image.BlackUnitUpgradeImage;
        else if (CheckSkill() == 2)
            Skill2Image.sprite = skill_Image.BlackUnitUpgradeImage;
    }

    public void EquipYellowUnitUpgrade()
    {
        if (EquipSkill(SkillType.노란기사강화) == false)
            return;


        if (CheckSkill() == 1)
            Skill1Image.sprite = skill_Image.YellowUnitUpgradeImage;
        else if (CheckSkill() == 2)
            Skill2Image.sprite = skill_Image.YellowUnitUpgradeImage;
    }

    public void EquipColorChange()
    {
        if (EquipSkill(SkillType.상대색깔변경) == false)
            return;


        if (CheckSkill() == 1)
            Skill1Image.sprite = skill_Image.ColorChangeImage;
        else if (CheckSkill() == 2)
            Skill2Image.sprite = skill_Image.ColorChangeImage;
    }

    public void EquipCommonSkill()
    {
        if (EquipSkill(SkillType.흔한스킬) == false)
            return;


        if (CheckSkill() == 1)
            Skill1Image.sprite = skill_Image.CommonSkillImage;
        else if (CheckSkill() == 2)
            Skill2Image.sprite = skill_Image.CommonSkillImage;
    }

    public void EquipFoodHater()
    {
        if (EquipSkill(SkillType.고기혐오자) == false)
            return;


        if (CheckSkill() == 1)
            Skill1Image.sprite = skill_Image.FoodHaterImage;
        else if (CheckSkill() == 2)
            Skill2Image.sprite = skill_Image.FoodHaterImage;
    }

    public void EquipSellUpgrade()
    {
        if (EquipSkill(SkillType.판매보상증가) == false)
            return;


        if (CheckSkill() == 1)
            Skill1Image.sprite = skill_Image.SellUpgradeImage;
        else if (CheckSkill() == 2)
            Skill2Image.sprite = skill_Image.SellUpgradeImage;
    }

    public void EquipBossDamageUpgrade()
    {
        if (EquipSkill(SkillType.보스데미지증가) == false)
            return;


        if (CheckSkill() == 1)
            Skill1Image.sprite = skill_Image.BossDamageUpgradeImage;
        else if (CheckSkill() == 2)
            Skill2Image.sprite = skill_Image.BossDamageUpgradeImage;
    }


    bool EquipSkill(SkillType skillType)
    {
        if (CheckSkill() >= 2)
        {
            Debug.Log("스킬을 2개 장착 중");
            return false;
        }
        
        Multi_Managers.ClientData.SkillByType[skillType].SetEquipSkill(true);
        InitEquips();
        return true;
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

    public void InitEquips()
    {
        InitEquip(SkillType.시작골드증가, StartGoldEquipButton);
        InitEquip(SkillType.최대유닛증가, PlusMaxUnitEquipButton);
        InitEquip(SkillType.태극스킬, TaegeukSkillEquipButton);
        InitEquip(SkillType.검은유닛강화, BlackUnitUpgradeEquipButton);
        InitEquip(SkillType.노란기사강화, YellowUnitUpgradeEquipButton);
        InitEquip(SkillType.상대색깔변경, ColorChangeEquipButton);
        InitEquip(SkillType.흔한스킬, CommonSkillEquipButton);
        InitEquip(SkillType.고기혐오자, FoodHaterEquipButton);
        InitEquip(SkillType.판매보상증가, SellUpgradeEquipButton);
        InitEquip(SkillType.보스데미지증가, BossDamageUpgradeEquipButton);
    }

    void InitEquip(SkillType skillType, Button equipButton)
    {
        if (equipButton == null)
            return;

        // 스킬이 없거나 스킬을 장착한 상태라면
        if (Multi_Managers.ClientData.SkillByType[skillType].HasSkill == false || Multi_Managers.ClientData.SkillByType[skillType].EquipSkill == true)
        {
            equipButton.interactable = false;
        }
        else
        {
            equipButton.interactable = true;
        }
    }

    // 뭔가 좋은 방법 필요
    public int CheckSkill()
    {
        int count = 0;

        if (Multi_Managers.ClientData.SkillByType[SkillType.시작골드증가].EquipSkill == true)
            count++;
        if (Multi_Managers.ClientData.SkillByType[SkillType.최대유닛증가].EquipSkill == true)
            count++;
        if (Multi_Managers.ClientData.SkillByType[SkillType.태극스킬].EquipSkill == true)
            count++;
        if (Multi_Managers.ClientData.SkillByType[SkillType.검은유닛강화].EquipSkill == true)
            count++;
        if (Multi_Managers.ClientData.SkillByType[SkillType.노란기사강화].EquipSkill == true)
            count++;
        if (Multi_Managers.ClientData.SkillByType[SkillType.상대색깔변경].EquipSkill == true)
            count++;
        if (Multi_Managers.ClientData.SkillByType[SkillType.흔한스킬].EquipSkill == true)
            count++;
        if (Multi_Managers.ClientData.SkillByType[SkillType.고기혐오자].EquipSkill == true)
            count++;
        if (Multi_Managers.ClientData.SkillByType[SkillType.판매보상증가].EquipSkill == true)
            count++;
        if (Multi_Managers.ClientData.SkillByType[SkillType.보스데미지증가].EquipSkill == true)
            count++;

        return count;
    }

    public void UnEquip()
    {
        Skill1Image.sprite = skill_Image.UImask;
        Skill2Image.sprite = skill_Image.UImask;

        Multi_Managers.ClientData.SkillByType[SkillType.시작골드증가].SetEquipSkill(false);
        Multi_Managers.ClientData.SkillByType[SkillType.최대유닛증가].SetEquipSkill(false);
        Multi_Managers.ClientData.SkillByType[SkillType.태극스킬].SetEquipSkill(false);
        Multi_Managers.ClientData.SkillByType[SkillType.검은유닛강화].SetEquipSkill(false);
        Multi_Managers.ClientData.SkillByType[SkillType.노란기사강화].SetEquipSkill(false);
        Multi_Managers.ClientData.SkillByType[SkillType.상대색깔변경].SetEquipSkill(false);
        Multi_Managers.ClientData.SkillByType[SkillType.흔한스킬].SetEquipSkill(false);
        Multi_Managers.ClientData.SkillByType[SkillType.고기혐오자].SetEquipSkill(false);
        Multi_Managers.ClientData.SkillByType[SkillType.판매보상증가].SetEquipSkill(false);
        Multi_Managers.ClientData.SkillByType[SkillType.보스데미지증가].SetEquipSkill(false);

        InitEquips();
    }

}

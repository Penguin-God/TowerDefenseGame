using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class ClientManager : MonoBehaviour
{
    #region 변수들
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
    #endregion

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

        if (Input.GetKeyDown(KeyCode.S)) // 돈복사 후 모든 스킬 구매
        {
            ClientIron += 10000;
            ClientWood += 10000;
            ClientHammer += 10000;
            InitMoney();
            UpdateMoney();

            foreach (SkillType type in Enum.GetValues(typeof(SkillType)))
                new UserSkillShopUseCase().GetSkillExp(type, 1);
        }
    }

    // 버튼에서 사용 중
    public void ShowSkillEquipUI() => Multi_Managers.UI.ShowPopupUI<SkillEquip_UI>().RefreshUI();

    #region update Money
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
    #endregion

    #region Buy Skills

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

    #endregion

    #region Equip Skills
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
        
        //Multi_Managers.ClientData.SkillByType[skillType].SetEquipSkill(true);
        Multi_Managers.ClientData.AddEquipSkill(skillType);

        InitEquips();
        return true;
    }
    #endregion

    #region Init Info
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
    #endregion


    public int CheckSkill() => Multi_Managers.ClientData.EquipSkillCount;

    public void UnEquip()
    {
        Skill1Image.sprite = skill_Image.UImask;
        Skill2Image.sprite = skill_Image.UImask;

        Multi_Managers.ClientData.Clear();
        InitEquips();
    }

    #region Upgrade Skills

    public GameObject SkillUpgradeBackground;
    public Image SkillImage;
    public Text SkillExplane;
    public Text SkillName;

    public void OpenSkillUpgrade(Sprite skillSprite, string skillexplane, string skillName)
    {
        // 스킬마다 내용 초기화
        SkillImage.sprite = skillSprite;
        SkillExplane.text = skillexplane;
        SkillName.text = skillName;
        SkillUpgradeBackground.SetActive(true);
    }

    public void UpgradeTaegeuk()
    {
        string taegeukText = "태극 스킬 설명";
        OpenSkillUpgrade(skill_Image.TaegeukSkillImage, taegeukText, "태극 스킬");
    }

    public void UpgradeBlackUnitUpgrade()
    {
        string blackUnitUpgrage = "검은 유닛 강화 설명";
        OpenSkillUpgrade(skill_Image.BlackUnitUpgradeImage, blackUnitUpgrage, "검은 유닛 강화");
    }

    public void UpgrageYellowUnitUpgrade()
    {
        string yellowUnitUpgrade = "노란 기사 강화 설명";
        OpenSkillUpgrade(skill_Image.YellowUnitUpgradeImage, yellowUnitUpgrade, "노랑 기사 강화");
    }

    public void UpgrageColorChange()
    {
        string colorChange = "상대 색깔 변경 설명";
        OpenSkillUpgrade(skill_Image.ColorChangeImage, colorChange, "상대 색깔 변경");
    }

    public void UpgrageFoodHater()
    {
        string foodHater = "고기 혐오자 설명";
        OpenSkillUpgrade(skill_Image.FoodHaterImage, foodHater, "고기 혐오자");
    }

    public void UpgrageStartFood()
    {
        string startFood = "현재: 시작 식량이 1 증가한다.\n강화 후: 시작 식량이 2 증가한다.";
        OpenSkillUpgrade(skill_Image.CommonSkillImage, startFood, "시작 고기 증가");
    }

    public void UpgragePlusMaxUnit()
    {
        string plusMaxUnit = "최대 유닛 증가 설명";
        OpenSkillUpgrade(skill_Image.PlusMaxUnitImage, plusMaxUnit, "최대 유닛 증가");
    }

    public void UpgrageBossDamage()
    {
        string bossDamage = "보스데미지 증가 설명";
        OpenSkillUpgrade(skill_Image.BossDamageUpgradeImage, bossDamage, "보스 데미지 증가");
    }
    #endregion
}

public enum UserSkillClass
{
    Main,
    Sub,
}

class UserSkillShopUseCase
{
    public void GetSkillExp(SkillType skillType, int getQuantity)
    {
        if (skillType == SkillType.None) return;
        Multi_Managers.ClientData.GetExp(skillType, getQuantity);
    }
}
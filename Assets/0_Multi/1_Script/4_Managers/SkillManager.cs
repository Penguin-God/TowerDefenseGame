using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Skill
{
    public string Name;
    public int Id;
    public const string path = "SkillData";

    public bool HasSkill;
    public bool EquipSkill;

    public void SetHasSkill(bool hasSkill)
    {
        HasSkill = hasSkill;
    }

    public void SetEquipSkill(bool equipSkill)
    {
        EquipSkill = equipSkill;
    }

    //public virtual void InitSkill(Skill skill) { }
    public virtual void InitSkill() { }
}

public class SkillRepository
{
    Dictionary<SkillType, Skill> _typeBySkill = new Dictionary<SkillType, Skill>();

    public SkillRepository()
    {
        _typeBySkill.Add(SkillType.시작골드증가, new StartGold());
        _typeBySkill.Add(SkillType.최대유닛증가, new MaxUnit());
        _typeBySkill.Add(SkillType.태극스킬, new Taegeuk());
        _typeBySkill.Add(SkillType.검은유닛강화, new BlackUnitUpgrade());
        _typeBySkill.Add(SkillType.노란기사강화, new YellowSowrdmanUpgrade());
        _typeBySkill.Add(SkillType.상대색깔변경, new ColorChange());
        _typeBySkill.Add(SkillType.판매보상증가, new SellUpgrade());
        _typeBySkill.Add(SkillType.보스데미지증가, new BossDamageUpgrade());
        _typeBySkill.Add(SkillType.고기혐오자, new FoodHater());
    }

    public Skill GetSkill(SkillType type) => _typeBySkill[type];
}

public class SkillManager
{
    public void Init()
    {
        foreach (var skill in Multi_Managers.ClientData.EquipSkills)
            skill.InitSkill();

        //List<Skill> skills = new List<Skill>();

        //if (Multi_Managers.ClientData.SkillByType[SkillType.시작골드증가].EquipSkill == true)
        //{
        //    StartGold startGold = new StartGold();
        //    startGold.EquipSkill = true;
        //    skills.Add(startGold);
        //    Debug.Log("시작 골드 증가 사용");
        //}
        //else
        //{
        //    Debug.Log("시작 골드 증가 없음.....");
        //}

        //if (Multi_Managers.ClientData.SkillByType[SkillType.최대유닛증가].EquipSkill == true)
        //{
        //    MaxUnit maxUnit = new MaxUnit();
        //    maxUnit.EquipSkill = true;
        //    skills.Add(maxUnit);
        //    Debug.Log("시작 최대 유닛 증가 사용");
        //}
        //else
        //{
        //    Debug.Log("시작 최대 유닛 증가 없음.....");
        //}


        //if (Multi_Managers.ClientData.SkillByType[SkillType.태극스킬].EquipSkill == true)
        //{
        //    Taegeuk taegeuk = new Taegeuk();
        //    taegeuk.EquipSkill = true;
        //    skills.Add(taegeuk);
        //    Debug.Log("태극 스킬 추가");
        //}
        //else
        //{
        //    Debug.Log("태극스킬 없음.....");
        //}

        //if (Multi_Managers.ClientData.SkillByType[SkillType.검은유닛강화].EquipSkill == true)
        //{
        //    BlackUnitUpgrade blackUnitUpgrade = new BlackUnitUpgrade();
        //    blackUnitUpgrade.EquipSkill = true;
        //    skills.Add(blackUnitUpgrade);
        //    Debug.Log("검은유닛강화 추가");
        //}
        //else
        //{
        //    Debug.Log("검은유닛강화 없음.....");
        //}

        //if (Multi_Managers.ClientData.SkillByType[SkillType.노란기사강화].EquipSkill == true)
        //{
        //    YellowSowrdmanUpgrade yellowUnitUpgrade = new YellowSowrdmanUpgrade();
        //    yellowUnitUpgrade.EquipSkill = true;
        //    skills.Add(yellowUnitUpgrade);
        //    Debug.Log("노란기사강화 추가");
        //}
        //else
        //{
        //    Debug.Log("노란기사강화 없음.....");
        //}

        //if (Multi_Managers.ClientData.SkillByType[SkillType.상대색깔변경].EquipSkill == true)
        //{
        //    ColorChange colorChange = new ColorChange();
        //    colorChange.EquipSkill = true;
        //    skills.Add(colorChange);
        //    Debug.Log("상대색깔변경 추가");
        //}
        //else
        //{
        //    Debug.Log("상대색깔변경 없음.....");
        //}

        //if (Multi_Managers.ClientData.SkillByType[SkillType.판매보상증가].EquipSkill == true)
        //{
        //    SellUpgrade sellUpgrade = new SellUpgrade();
        //    sellUpgrade.EquipSkill = true;
        //    skills.Add(sellUpgrade);
        //    Debug.Log("판매보상증가 추가");
        //}
        //else
        //{
        //    Debug.Log("판매보상증가 없음.....");
        //}

        //if (Multi_Managers.ClientData.SkillByType[SkillType.보스데미지증가].EquipSkill == true)
        //{
        //    BossDamageUpgrade bossDamageUpgrade = new BossDamageUpgrade();
        //    bossDamageUpgrade.EquipSkill = true;
        //    skills.Add(bossDamageUpgrade);
        //    Debug.Log("보스데미지증가 추가");
        //}
        //else
        //{
        //    Debug.Log("보스데미지증가 없음.....");
        //}

        //if (Multi_Managers.ClientData.SkillByType[SkillType.고기혐오자].EquipSkill == true)
        //{
        //    FoodHater foodHater = new FoodHater();
        //    foodHater.EquipSkill = true;
        //    skills.Add(foodHater);
        //    Debug.Log("고기혐오자 추가");
        //}
        //else
        //{
        //    Debug.Log("보스데미지증가 없음.....");
        //}

        //Debug.Log("==========================================================");
        //Debug.Log(skills.Count);
        //for (int i = 0; i < skills.Count; i++)
        //{
        //    if (skills[i].EquipSkill == true)
        //        skills[i].InitSkill(skills[i]);
        //}
    }
}

public class PassiveSkill : Skill
{
    //public override void InitSkill()
    //{

    //}
}

public class ActiveSkill : Skill
{
    //public override void InitSkill()
    //{

    //}
}

// ================= 스킬 세부 구현 =====================

public class StartGold : PassiveSkill
{
    public override void InitSkill()
    {

    }
}

public class MaxUnit : PassiveSkill
{
    public override void InitSkill()
    {

    }
}

public class Taegeuk : PassiveSkill
{

    // 빨강, 파랑을 제외한 유닛 수
    public List<int> Ather
    {
        get
        {
            List<int> countList = new List<int>();
            int SwordmanCount = 0;
            int ArhcerCount = 0;
            int SpearmanCount = 0;
            int MageCount = 0;

            for (int i = 2; i < 6; i++)
            {
                SwordmanCount += Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags(i, 0)];
                ArhcerCount += Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags(i, 1)];
                SpearmanCount += Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags(i, 2)];
                MageCount += Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags(i, 3)];
            }

            countList.Add(SwordmanCount);
            countList.Add(ArhcerCount);
            countList.Add(SpearmanCount);
            countList.Add(MageCount);

            return countList;
        }
    }

    public List<int> Red
    {
        get
        {
            List<int> countList = new List<int>();
            int SwordmanCount = 0;
            int ArhcerCount = 0;
            int SpearmanCount = 0;
            int MageCount = 0;

            SwordmanCount += Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags(0, 0)];
            ArhcerCount += Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags(0, 1)];
            SpearmanCount += Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags(0, 2)];
            MageCount += Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags(0, 3)];

            countList.Add(SwordmanCount);
            countList.Add(ArhcerCount);
            countList.Add(SpearmanCount);
            countList.Add(MageCount);

            return countList;
        }
    }

    public List<int> Blue
    {
        get
        {
            List<int> countList = new List<int>();
            int SwordmanCount = 0;
            int ArhcerCount = 0;
            int SpearmanCount = 0;
            int MageCount = 0;

            SwordmanCount += Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags(1, 0)];
            ArhcerCount += Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags(1, 1)];
            SpearmanCount += Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags(1, 2)];
            MageCount += Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags(1, 3)];

            countList.Add(SwordmanCount);
            countList.Add(ArhcerCount);
            countList.Add(SpearmanCount);
            countList.Add(MageCount);

            return countList;
        }
    }

    public override void InitSkill()
    {
        Debug.Log("태극 시너지 스킬 착용");
        Multi_UnitManager.Instance.OnUnitCountChanged += (count) => UseSkill();
    }

    void UseSkill()
    {
        if (Red[0] >= 1 && Blue[0] >= 1 && Ather[0] == 0)
        {
            Debug.Log("기사 강화!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 0), 300);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 0), 300);
        }
        else
        {
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 0), 25);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 0), 25);
        }

        if (Red[1] >= 1 && Blue[1] >= 1 && Ather[1] == 0)
        {
            Debug.Log("궁수 강화!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 1), 1600);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 1), 1600);
        }
        else
        {
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 1), 250);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 1), 250);
        }

        if (Red[2] >= 1 && Blue[2] >= 1 && Ather[2] == 0)
        {
            Debug.Log("창병 강화!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 2), 15000);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 2), 15000);
        }
        else
        {
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 2), 4000);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 2), 4000);
        }

        if (Red[3] >= 1 && Blue[3] >= 1 && Ather[3] == 0)
        {
            Debug.Log("마법사 강화!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 3), 100000);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 3), 100000);
        }
        else
        {
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 3), 25000);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 3), 25000);
        }
    }
}

public class BlackUnitUpgrade : PassiveSkill
{
    public override void InitSkill()
    {
        Multi_UnitManager.Instance.OnUnitCountChanged += (count) => UseSkill();
    }

    void UseSkill()
    {
        Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(7, 0), 30000);
        Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(7, 1), 100000);
        Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(7, 2), 1000000);
        Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(7, 3), 10000000);
    }
}

public class YellowSowrdmanUpgrade : PassiveSkill
{
    public override void InitSkill()
    {
        // 노란 기사 패시브 5원으로 변경
        Multi_Managers.Data.Skill.InitCombineAdditionalGold(3);
    }
}

public class ColorChange : ActiveSkill
{
    public override void InitSkill()
    {
        // 하얀 유닛을 뽑을 때 뽑은 직업과 같은 상대 유닛의 색깔을 다른 색깔로 변경
        Multi_UnitManager.Instance.OnUnitFlagChanged += UseSkill;
    }

    void UseSkill(UnitFlags flag, bool increaseed)
    {
        // 상대 직업의 색깔 변경
        if(flag.UnitColor == UnitColor.white && increaseed)
        {
            // 색깔 변경
        }
    }
}

public class CommonSkill : PassiveSkill
{
    public override void InitSkill()
    {
        // 대충 안좋은 효과
    }
}

public class FoodHater : PassiveSkill
{
    public override void InitSkill()
    {
        // 고기창 닫기

        // 하얀 유닛 돈으로 구매로 변경 받는 고기 전부 1당 10원으로 변경
        Multi_GameManager.instance.OnFoodChanged += (food) => FoodToGold();
    }

    void FoodToGold()
    {
        Multi_GameManager.instance.FoodToGold(10);
    }
}

public class SellUpgrade : PassiveSkill
{
    public override void InitSkill()
    {
        // 유닛 판매 보상 증가 (유닛별로 증가폭 별도)
    }
}

public class BossDamageUpgrade : PassiveSkill
{
    public override void InitSkill()
    {
        // 모든 유닛 보스 데미지 증가
    }
}


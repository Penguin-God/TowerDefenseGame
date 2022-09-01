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

    public virtual void InitSkill(Skill skill) { }

    // public abstract void InitSkill(SkillType skillType);
}

// TODO : 곧 죽음
public class SkillManager
{
    Dictionary<SkillType, System.Action> keyValuePairs = new Dictionary<SkillType, System.Action>();
    

    public void Init()
    {
        List<Skill> skills = new List<Skill>();

        if (Multi_Managers.ClientData.SkillByType[SkillType.시작골드증가].EquipSkill == true)
        {
            StartGold startGold = new StartGold();
            startGold.EquipSkill = true;
            skills.Add(startGold);
            Debug.Log("시작 골드 증가 사용");
        }
        else
        {
            Debug.Log("시작 골드 증가 없음.....");
        }

        if (Multi_Managers.ClientData.SkillByType[SkillType.최대유닛증가].EquipSkill == true)
        {
            MaxUnit maxUnit = new MaxUnit();
            maxUnit.EquipSkill = true;
            skills.Add(maxUnit);
            Debug.Log("시작 최대 유닛 증가 사용");
        }
        else
        {
            Debug.Log("시작 최대 유닛 증가 없음.....");
        }

        //Multi_Managers.ClientData.SkillByType[SkillType.태극스킬].EquipSkill == 
        if (true)
        {
            Taegeuk taegeuk = new Taegeuk();
            taegeuk.EquipSkill = true;
            skills.Add(taegeuk);
            Debug.Log("태극 스킬 추가");
        }
        else
        {
            Debug.Log("태극스킬 없음.....");
        }

        if (Multi_Managers.ClientData.SkillByType[SkillType.검은유닛강화].EquipSkill == true)
        {
            BlackUnitUpgrade blackUnitUpgrade = new BlackUnitUpgrade();
            blackUnitUpgrade.EquipSkill = true;
            skills.Add(blackUnitUpgrade);
            Debug.Log("검은유닛강화 추가");
        }
        else
        {
            Debug.Log("검은유닛강화 없음.....");
        }

        if (Multi_Managers.ClientData.SkillByType[SkillType.노란기사강화].EquipSkill == true)
        {
            YellowUnitUpgrade yellowUnitUpgrade = new YellowUnitUpgrade();
            yellowUnitUpgrade.EquipSkill = true;
            skills.Add(yellowUnitUpgrade);
            Debug.Log("노란기사강화 추가");
        }
        else
        {
            Debug.Log("노란기사강화 없음.....");
        }

        if (Multi_Managers.ClientData.SkillByType[SkillType.상대색깔변경].EquipSkill == true)
        {
            ColorChange colorChange = new ColorChange();
            colorChange.EquipSkill = true;
            skills.Add(colorChange);
            Debug.Log("상대색깔변경 추가");
        }
        else
        {
            Debug.Log("상대색깔변경 없음.....");
        }

        if (Multi_Managers.ClientData.SkillByType[SkillType.판매보상증가].EquipSkill == true)
        {
            SellUpgrade sellUpgrade = new SellUpgrade();
            sellUpgrade.EquipSkill = true;
            skills.Add(sellUpgrade);
            Debug.Log("판매보상증가 추가");
        }
        else
        {
            Debug.Log("판매보상증가 없음.....");
        }

        if (Multi_Managers.ClientData.SkillByType[SkillType.보스데미지증가].EquipSkill == true)
        {
            BossDamageUpgrade bossDamageUpgrade = new BossDamageUpgrade();
            bossDamageUpgrade.EquipSkill = true;
            skills.Add(bossDamageUpgrade);
            Debug.Log("보스데미지증가 추가");
        }
        else
        {
            Debug.Log("보스데미지증가 없음.....");
        }

        Debug.Log("==========================================================");
        Debug.Log(skills.Count);
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].EquipSkill == true)
                skills[i].InitSkill(skills[i]);
        }
    }

    

    public void Clear()
    {
        keyValuePairs.Clear();
        // skills.Clear();
    }
}

public class PassiveSkill : Skill
{
    public override void InitSkill(Skill skill)
    {

    }
}

public class ActiveSkill : Skill
{
    public override void InitSkill(Skill skill)
    {

    }
}

// --------------------------------------------

public class StartGold : PassiveSkill
{
    public override void InitSkill(Skill skill)
    {

    }
}

public class MaxUnit : PassiveSkill
{
    public override void InitSkill(Skill skill)
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

    public override void InitSkill(Skill skill)
    {
        Debug.Log("태극 시너지 스킬 착용");
        Multi_UnitManager.Instance.OnUnitCountChanged += (count) => UseSkill();
    }

    void UseSkill()
    {
        if (Red[0] >= 1 && Blue[0] >= 1 && Ather[0] == 0)
        {
            Debug.Log("기사 강화!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.Damage, new UnitFlags(0, 0), 200);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.Damage, new UnitFlags(1, 0), 200);
        }

        if (Red[1] >= 1 && Blue[1] >= 1 && Ather[1] == 0)
        {
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.Damage, new UnitFlags(0, 1), 2000);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.Damage, new UnitFlags(1, 1), 2000);
        }

        if (Red[2] >= 1 && Blue[2] >= 1 && Ather[2] == 0)
        {
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.Damage, new UnitFlags(0, 2), 33000);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.Damage, new UnitFlags(1, 2), 33000);
        }

        if (Red[3] >= 1 && Blue[3] >= 1 && Ather[3] == 0)
        {
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.Damage, new UnitFlags(0, 3), 200000);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.Damage, new UnitFlags(1, 3), 200000);
        }
    }
}

public class BlackUnitUpgrade : PassiveSkill
{
    public override void InitSkill(Skill skill)
    {
        Multi_UnitManager.Instance.OnUnitCountChanged += (count) => UseSkill();
    }

    void UseSkill()
    {
        Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.Damage, new UnitFlags(7, 0), 10000);
        Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.Damage, new UnitFlags(7, 1), 100000);
        Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.Damage, new UnitFlags(7, 2), 1000000);
        Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.Damage, new UnitFlags(7, 3), 10000000);
    }
}

public class YellowUnitUpgrade : PassiveSkill
{
    public override void InitSkill(Skill skill)
    {
        // 노란 기사 패시브 5원으로 변경
    }
}

public class ColorChange : ActiveSkill
{
    public override void InitSkill(Skill skill)
    {
        // 하얀 유닛을 뽑을 때 뽑은 직업과 같은 상대 유닛의 색깔을 다른 색깔로 변경
    }
}

public class CommonSkill : PassiveSkill
{
    public override void InitSkill(Skill skill)
    {
        // 대충 안좋은 효과
    }
}

public class FoodHater : PassiveSkill
{
    public override void InitSkill(Skill skill)
    {
        // 하얀 유닛 돈으로 구매로 변경 받는 고기 전부 1당 10원으로 변경
    }
}

public class SellUpgrade : PassiveSkill
{
    public override void InitSkill(Skill skill)
    {
        // 유닛 판매 보상 증가 (1원 추가)
    }
}

public class BossDamageUpgrade : PassiveSkill
{
    public override void InitSkill(Skill skill)
    {
        // 모든 유닛 보스 데미지 증가
    }
}


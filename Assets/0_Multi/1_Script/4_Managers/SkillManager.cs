using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public interface IUserSkill
{
    void InitSkill();
}

[Serializable]
public class Skill
{
    public string Name;
    public int Id;
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
}

public class SkillRepository
{
    Dictionary<SkillType, IUserSkill> _typeBySkill = new Dictionary<SkillType, IUserSkill>();

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

    public IUserSkill GetSkill(SkillType type) => _typeBySkill[type];
}

public class SkillManager
{
    public void Init()
    {
        foreach (var skill in Multi_Managers.ClientData.EquipSkills)
            skill.InitSkill();
    }
}


// ================= 스킬 세부 구현 =====================

public class StartGold : IUserSkill
{
    public void InitSkill()
    {

    }
}

public class MaxUnit : IUserSkill
{
    public void InitSkill()
    {

    }
}

// 유닛 카운트 현황
public class Taegeuk : IUserSkill
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

    public void InitSkill()
    {
        Debug.Log("태극 시너지 스킬 착용");
        Multi_UnitManager.Instance.OnUnitFlagCountChanged += (count, flag) => UseSkill();
    }

    void UseSkill()
    {
        float[] datas = Multi_Managers.Data.GetUserSKillData(SkillType.태극스킬, 1);

        if (Red[0] >= 1 && Blue[0] >= 1 && Ather[0] == 0)
        {
            Debug.Log("기사 강화!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 0), (int)datas[0]);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 0), (int)datas[0]);
        }
        else
        {
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 0), 25);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 0), 25);
        }

        if (Red[1] >= 1 && Blue[1] >= 1 && Ather[1] == 0)
        {
            Debug.Log("궁수 강화!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 1), (int)datas[1]);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 1), (int)datas[1]);
        }
        else
        {
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 1), 250);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 1), 250);
        }

        if (Red[2] >= 1 && Blue[2] >= 1 && Ather[2] == 0)
        {
            Debug.Log("창병 강화!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 2), (int)datas[2]);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 2), (int)datas[2]);
        }
        else
        {
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 2), 4000);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 2), 4000);
        }

        if (Red[3] >= 1 && Blue[3] >= 1 && Ather[3] == 0)
        {
            Debug.Log("마법사 강화!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 3), (int)datas[3]);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 3), (int)datas[3]);
        }
        else
        {
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 3), 25000);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 3), 25000);
        }
    }
}

// 유닛 카운트 현황
public class BlackUnitUpgrade : IUserSkill
{
    public void InitSkill()
    {
        Multi_UnitManager.Instance.OnUnitFlagCountChanged += (flag, count) => UseSkill(flag);
    }

    void UseSkill(UnitFlags unitFlags)
    {
        if (unitFlags.UnitColor != UnitColor.black) return;

        float[] datas = Multi_Managers.Data.GetUserSKillData(SkillType.검은유닛강화, 1);
        switch (unitFlags.UnitClass)
        {
            case UnitClass.sowrdman:
                Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(7, 0), (int)datas[0]);
                break;
            case UnitClass.archer:
                Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(7, 1), (int)datas[1]);
                break;
            case UnitClass.spearman:
                Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(7, 2), (int)datas[2]);
                break;
            case UnitClass.mage:
                Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(7, 3), (int)datas[3]);
                break;
        }
    }
}

public class YellowSowrdmanUpgrade : IUserSkill
{
    public void InitSkill()
    {
        // 노란 기사 패시브 골드 변경
        Multi_GameManager.instance.BattleData.YellowKnightRewardGold = (int)Multi_Managers.Data.GetUserSKillData(SkillType.노란기사강화, 1)[0];
    }
}

public class ColorChange : IUserSkill
{
    // 하얀 유닛을 뽑을 때 뽑은 직업과 같은 상대 유닛의 색깔을 다른 색깔로 변경

    int[] _prevUnitCounts = new int[4];

    public void InitSkill()
    {
        Multi_UnitManager.Instance.OnUnitFlagCountChanged += UseSkill;
    }

    void UseSkill(UnitFlags flag, int count)
    {
        if (flag.UnitColor != UnitColor.white) return;

        if (count > _prevUnitCounts[flag.ClassNumber])
        {
            var list = Util.GetRangeList(0, 6);
            list.Remove(flag.ColorNumber);
            Multi_UnitManager.Instance.UnitColorChanged_RPC(Multi_Data.instance.EnemyPlayerId, flag, list.GetRandom());
        }
        _prevUnitCounts[flag.ClassNumber] = count;
    }
}

public class FoodHater : IUserSkill
{
    public void InitSkill()
    {
        var battleData = Multi_GameManager.instance.BattleData;
        battleData.GetAllPriceDatas()
                .Where(x => x.CurrencyType == GameCurrencyType.Food)
                .ToList()
                .ForEach(x => x.ChangedCurrencyType(GameCurrencyType.Gold));

        battleData.WhiteUnitPriceRecord.PriceDatas.ToList().ForEach(x => x.ChangePrice(x.Price * 10));
        battleData.MaxUnitIncreaseRecord.ChangePrice(battleData.MaxUnitIncreaseRecord.Price * 10);

        // 하얀 유닛 돈으로 구매로 변경 받는 고기 전부 1당 10원으로 변경
        Multi_GameManager.instance.OnFoodChanged += FoodToGold;
    }

    void FoodToGold(int food)
    {
        if (food <= 0) return;

        int rate = (int)Multi_Managers.Data.GetUserSKillData(SkillType.고기혐오자, 1)[0];
        if (Multi_GameManager.instance.TryUseFood(food))
            Multi_GameManager.instance.AddGold(food * rate);
    }
}

public class SellUpgrade : IUserSkill
{
    public void InitSkill()
    {
        // 유닛 판매 보상 증가 (유닛별로 증가폭 별도)
        int[] sellData = Multi_Managers.Data.GetUserSKillData(SkillType.판매보상증가, 1).Select(x => (int)x).ToArray();
        var battleData = Multi_GameManager.instance.BattleData;
        battleData.SwordmanSellGold = sellData[0];
        battleData.ArcherSellGold = sellData[1];
        battleData.SpearmanSellGold = sellData[2];
        battleData.MageSellGold = sellData[3];
    }
}

public class BossDamageUpgrade : IUserSkill
{
    public void InitSkill()
    {
        // 모든 유닛 보스 데미지 증가
    }
}


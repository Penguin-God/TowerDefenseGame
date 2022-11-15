using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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

public abstract class UserSkill
{
    public void SetInfo(SkillType skillType) => _skillType = skillType;
    SkillType _skillType;

    public abstract void InitSkill();
    protected float[] GetData() => Multi_Managers.ClientData.GetSkillLevelData(_skillType).BattleDatas;
}

public class UserSkillFactory
{
    Dictionary<SkillType, UserSkill> _typeBySkill = new Dictionary<SkillType, UserSkill>();

    public UserSkillFactory()
    {
        _typeBySkill.Add(SkillType.시작골드증가, new StartGold());
        _typeBySkill.Add(SkillType.시작고기증가, new StartFood());
        _typeBySkill.Add(SkillType.최대유닛증가, new MaxUnit());
        _typeBySkill.Add(SkillType.태극스킬, new Taegeuk());
        _typeBySkill.Add(SkillType.검은유닛강화, new BlackUnitUpgrade());
        _typeBySkill.Add(SkillType.노란기사강화, new YellowSowrdmanUpgrade());
        _typeBySkill.Add(SkillType.상대색깔변경, new ColorChange());
        _typeBySkill.Add(SkillType.판매보상증가, new SellUpgrade());
        _typeBySkill.Add(SkillType.보스데미지증가, new BossDamageUpgrade());
        _typeBySkill.Add(SkillType.고기혐오자, new FoodHater());
    }

    public UserSkill GetSkill(SkillType type, int level)
    {
        _typeBySkill[type].SetInfo(type);
        return _typeBySkill[type];
    }
}

public class SkillManager
{
    public void Init()
    {
        foreach (var skillType in Multi_Managers.ClientData.EquipSkillManager.EquipSkills)
        {
            if (skillType == SkillType.None)
                continue;
            new UserSkillFactory().GetSkill(skillType, 1).InitSkill();
        }
    }
}


// ================= 스킬 세부 구현 =====================

public class StartGold : UserSkill
{
    public override void InitSkill()
        => Multi_GameManager.instance.AddGold((int)GetData()[0]);
}

public class StartFood : UserSkill
{
    public override void InitSkill()
        => Multi_GameManager.instance.AddFood((int)GetData()[0]);
}

public class MaxUnit : UserSkill
{
    public override void InitSkill()
        => Multi_GameManager.instance.BattleData.MaxUnit += (int)GetData()[0];
}

// 유닛 카운트 현황
public class Taegeuk : UserSkill
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
        Multi_UnitManager.Instance.OnUnitFlagCountChanged += (count, flag) => UseSkill();
    }

    void UseSkill()
    {
        int[] datas = GetData().Select(x => (int)x).ToArray();

        var strongDamages = new UnitDamages(datas[0], datas[1], datas[2], datas[3]);
        var originDamages = new UnitDamages(25, 250, 4000, 25000);

        if (Red[0] >= 1 && Blue[0] >= 1 && Ather[0] == 0)
        {
            Debug.Log("기사 강화!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 0), strongDamages.SwordmanDamage);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 0), strongDamages.SwordmanDamage);
        }
        else
        {
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 0), originDamages.SwordmanDamage);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 0), originDamages.SwordmanDamage);
        }

        if (Red[1] >= 1 && Blue[1] >= 1 && Ather[1] == 0)
        {
            Debug.Log("궁수 강화!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 1), strongDamages.ArcherDamage);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 1), strongDamages.ArcherDamage);
        }
        else
        {
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 1), originDamages.ArcherDamage);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 1), originDamages.ArcherDamage);
        }

        if (Red[2] >= 1 && Blue[2] >= 1 && Ather[2] == 0)
        {
            Debug.Log("창병 강화!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 2), strongDamages.SpearmanDamage);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 2), strongDamages.SpearmanDamage);
        }
        else
        {
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 2), originDamages.SpearmanDamage);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 2), originDamages.SpearmanDamage);
        }

        if (Red[3] >= 1 && Blue[3] >= 1 && Ather[3] == 0)
        {
            Debug.Log("마법사 강화!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 3), strongDamages.MageDamage);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 3), strongDamages.MageDamage);
        }
        else
        {
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(0, 3), originDamages.MageDamage);
            Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(1, 3), originDamages.MageDamage);
        }
    }
}

// 유닛 카운트 현황
public class BlackUnitUpgrade : UserSkill
{
    public override void InitSkill()
    {
        Multi_UnitManager.Instance.OnUnitFlagCountChanged += (flag, count) => UseSkill(flag);
    }

    void UseSkill(UnitFlags unitFlags)
    {
        if (unitFlags.UnitColor != UnitColor.black) return;

        int[] datas = GetData().Select(x => (int)x).ToArray();

        var strongDamages = new UnitDamages(datas[0], datas[1], datas[2], datas[3]);
        switch (unitFlags.UnitClass)
        {
            case UnitClass.sowrdman:
                Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(7, 0), strongDamages.SwordmanDamage);
                break;
            case UnitClass.archer:
                Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(7, 1), strongDamages.ArcherDamage);
                break;
            case UnitClass.spearman:
                Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(7, 2), strongDamages.SpearmanDamage);
                break;
            case UnitClass.mage:
                Multi_UnitManager.Instance.UnitStatChange_RPC(UnitStatType.All, new UnitFlags(7, 3), strongDamages.MageDamage);
                break;
        }
    }
}

public class YellowSowrdmanUpgrade : UserSkill
{
    public override void InitSkill()
    {
        // 노란 기사 패시브 골드 변경
        Multi_GameManager.instance.BattleData.YellowKnightRewardGold = (int)GetData()[0];
    }
}

public class ColorChange : UserSkill
{
    // 하얀 유닛을 뽑을 때 뽑은 직업과 같은 상대 유닛의 색깔을 다른 색깔로 변경

    int[] _prevUnitCounts = new int[4];

    public override void InitSkill()
    {
        Multi_GameManager.instance.BattleData.UnitSummonData.maxColorNumber = 6;
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

public class FoodHater : UserSkill
{
    public override void InitSkill()
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

        int rate = (int)GetData()[0];
        if (Multi_GameManager.instance.TryUseFood(food))
            Multi_GameManager.instance.AddGold(food * rate);
    }
}

public class SellUpgrade : UserSkill
{
    public override void InitSkill()
    {
        // 유닛 판매 보상 증가 (유닛별로 증가폭 별도)
        int[] sellData = GetData().Select(x => (int)x).ToArray();
        var sellRewardDatas = Multi_GameManager.instance.BattleData.UnitSellPriceRecord.PriceDatas;
        for (int i = 0; i < sellRewardDatas.Length; i++)
            sellRewardDatas[i].ChangePrice(sellData[i]);
    }
}

public class BossDamageUpgrade : UserSkill
{
    public override void InitSkill()
    {
        float rate = GetData()[0];
        Multi_SpawnManagers.NormalUnit.OnSpawn += (unit) => unit.BossDamage = Mathf.RoundToInt(unit.BossDamage * rate);
    }
}

public struct UnitDamages
{
    public UnitDamages(int sword, int archer, int spear, int mage)
    {
        _swordmanDamage = sword;
        _archerDamage = archer;
        _spearmanDamage = spear;
        _mageDamage = mage;
    }

    int _swordmanDamage;
    int _archerDamage;
    int _spearmanDamage;
    int _mageDamage;

    public int SwordmanDamage => _swordmanDamage;
    public int ArcherDamage => _archerDamage;
    public int SpearmanDamage => _spearmanDamage;
    public int MageDamage => _mageDamage;
}
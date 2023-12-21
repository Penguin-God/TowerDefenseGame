using System;
using System.Collections;
using System.Collections.Generic;

public enum PlayerMoneyType
{
    Gold,
    Gem,
}

public class PlayerDataManager
{
    public readonly Money Gold;
    public readonly Money Gem;

    public bool UseMoney(MoneyData moneyData) => UseMoney(moneyData.MoneyType, moneyData.Amount);
    public bool UseMoney(PlayerMoneyType type, int amount)
    {
        switch (type)
        {
            case PlayerMoneyType.Gold: return UseMoney(Gold, amount);
            case PlayerMoneyType.Gem: return UseMoney(Gem, amount);
            default: return false;
        }
    }
    bool UseMoney(Money money, int amount)
    {
        if (money.Has(amount) == false) return false;
        money.Subtract(amount);
        return true;
    }

    public int Score { get; private set; }
    public Action<int> OnChangeScore;
    public void ChangeScore(int changeAmount)
    {
        Score = UnityEngine.Mathf.Max(0, Score + changeAmount);
        OnChangeScore?.Invoke(Score);
    }

    public SkillInventroy SkillInventroy { get; private set; }
    public EquipSkillManager EquipSkillManager { get; private set; }

    public PlayerDataManager(SkillInventroy skillInventroy, int gold, int gem, int score, SkillType equipMain, SkillType subMain)
    {
        SkillInventroy = skillInventroy;
        Gold = new Money(gold);
        Gem = new Money(gem);
        Score = score;
        EquipSkillManager = new EquipSkillManager(equipMain, subMain);
    }
}

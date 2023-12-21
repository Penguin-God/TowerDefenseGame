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

    public event Action<int> OnGoldAmountChanged;
    public event Action<int> OnGemAmountChanged;

    void NotifyGoldChange(int amount) => OnGoldAmountChanged?.Invoke(amount);
    void NotifyGemChange(int amount) => OnGemAmountChanged?.Invoke(amount);

    public void AddGold(int amount) => AddMoney(Gold, amount);
    public void AddGem(int amount) => AddMoney(Gem, amount);

    public bool HasGold(int amount) => Gold.Has(amount);
    public bool UseMoney(MoneyData moneyData) => UseMoney(moneyData.MoneyType, moneyData.Amount);
    public bool UseMoney(PlayerMoneyType type, int amount)
    {
        switch (type)
        {
            case PlayerMoneyType.Gold: return TryUseGold(amount);
            case PlayerMoneyType.Gem: return TryUseGem(amount);
            default: return false;
        }
    }
    public bool TryUseGold(int amount) => UseMoney(Gold, amount);
    public bool TryUseGem(int amount) => UseMoney(Gem, amount);

    void AddMoney(Money money, int amount) => money.Add(amount);

    bool UseMoney(Money money, int amount)
    {
        if (amount > money.Amount) return false;

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
    public PlayerDataManager(SkillInventroy skillInventroy, int gold, int gem, int score)
    {
        SkillInventroy = skillInventroy;
        Gold = new Money(gold);
        Gem = new Money(gem);
        ChangeScore(Score);

        Gold.OnAmountChange += NotifyGoldChange;
        Gem.OnAmountChange += NotifyGemChange;
    }
}

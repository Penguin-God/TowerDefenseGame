using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerDataManager
{
    readonly Money _gold;
    readonly Money _gem;

    public event Action<int> OnGoldAmountChanged;
    public event Action<int> OnGemAmountChanged;

    void NotifyGoldChange(int amount) => OnGoldAmountChanged?.Invoke(amount);
    void NotifyGemChange(int amount) => OnGemAmountChanged?.Invoke(amount);


    public void AddGold(int amount) => AddMoney(_gold, amount);
    public void AddGem(int amount) => AddMoney(_gem, amount);

    public bool TryUseGold(int amount) => UseMoney(_gold, amount);
    public bool TryUseGem(int amount) => UseMoney(_gem, amount);

    void AddMoney(Money money, int amount) => money.Add(amount);

    bool UseMoney(Money money, int amount)
    {
        if (amount > money.Amount) return false;

        money.Subtract(amount);
        return true;
    }

    public SkillInventroy SkillInventroy { get; private set; }
    public PlayerDataManager(SkillInventroy skillInventroy, int gold, int gem)
    {
        SkillInventroy = skillInventroy;
        _gold = new Money(gold);
        _gem = new Money(gem);

        _gold.OnAmountChange += NotifyGoldChange;
        _gem.OnAmountChange += NotifyGemChange;
    }
}

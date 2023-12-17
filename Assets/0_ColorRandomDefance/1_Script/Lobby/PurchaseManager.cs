using System.Collections;
using System.Collections.Generic;

public interface IPurchaseOperator
{
    void SuccessPurchase(PlayerDataManager playerDataManager);
}

public readonly struct MoneyData
{
    public readonly PlayerMoneyType MoneyType;
    public readonly int Amount;

    public MoneyData(PlayerMoneyType moneyType, int amount)
    {
        MoneyType = moneyType;
        Amount = amount;
    }
}

public class PurchaseManager
{
    readonly IPurchaseOperator _purchase;
    readonly PlayerDataManager _playerDataManager;
    public PurchaseManager(IPurchaseOperator purchase, PlayerDataManager playerDataManager)
    {
        _purchase = purchase;
        _playerDataManager = playerDataManager;
    }

    public void Purchase(MoneyData price)
    {
        if (_playerDataManager.UseMoney(price) == false) return;
        
        _purchase.SuccessPurchase(_playerDataManager);
    }
}

public class BoxPurchaseOperator : IPurchaseOperator
{
    readonly SkillDrawer _skillDrawer;
    public BoxPurchaseOperator(SkillDrawer skillDrawer) => _skillDrawer = skillDrawer;

    public void SuccessPurchase(PlayerDataManager playerDataManager)
    {
        var drawInfos = new List<SkillDrawInfo>
        {
            new SkillDrawInfo(UserSkillClass.Main, 1, 10),
            new SkillDrawInfo(UserSkillClass.Main, 20, 40),
            new SkillDrawInfo(UserSkillClass.Sub, 30, 80),
        };
        var result = _skillDrawer.DrawSkills(drawInfos);
        playerDataManager.AddSkills(result);
    }
}

public class GoldPurchaseOperator : IPurchaseOperator
{
    readonly int GainGold;
    public GoldPurchaseOperator(int gainGold)
    {
        GainGold = gainGold;
    }
        
    public void SuccessPurchase(PlayerDataManager playerDataManager)
    {
        playerDataManager.AddGold(GainGold);
    }
}


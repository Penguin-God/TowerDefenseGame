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
    readonly IEnumerable<SkillDrawInfo> _drawInfos;
    public BoxPurchaseOperator(SkillDrawer skillDrawer, IEnumerable<SkillDrawInfo> drawInfos)
    {
        _skillDrawer = skillDrawer;
        _drawInfos = drawInfos;
    }

    public void SuccessPurchase(PlayerDataManager playerDataManager)
    {
        var result = _skillDrawer.DrawSkills(_drawInfos);
        playerDataManager.AddSkills(result);
        Managers.UI.ShowPopupUI<UI_NotifySkillDrawWindow>().ShowSkillsInfo(result);
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
        Managers.UI.ShowPopupUI<UI_NotifyWindow>().SetMessage("구매가 완료되었습니다");   
    }
}


using System.Collections;
using System.Collections.Generic;

public interface IPurchaseOperator
{
    void SuccessPurchase(PlayerDataManager playerDataManager);
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

    public void Purchase(PlayerMoneyType playerMoneyType, int amount)
    {
        if (_playerDataManager.UseMoney(playerMoneyType, amount) == false) return;
        
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

using System.Collections;
using System.Collections.Generic;

public interface IProductGiver
{
    void GiveProduct(PlayerDataManager playerDataManager);
}

public interface IDataPersistence
{
    void Save(PlayerDataManager playerData);
}

public class BuyUseCase
{
    readonly PlayerDataManager _playerDataManager;
    readonly IDataPersistence _dataPersistence;
    IProductGiver _giver;
    public BuyUseCase(PlayerDataManager playerDataManager, IDataPersistence dataPersistence, IProductGiver giver)
    {
        _playerDataManager = playerDataManager;
        _dataPersistence = dataPersistence;
        _giver = giver;
    }

    public bool Buy(PlayerMoneyType type, int amount)
    {
        if(_playerDataManager.UseMoney(type, amount) == false) return false;

        _giver.GiveProduct(_playerDataManager);
        _dataPersistence.Save(_playerDataManager);
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyUseCase
{
    public int Amount { get; private set; }

    public CurrencyUseCase(int amount) => Amount = amount;

    public void AddCurrency(int amount) => Amount += amount;
    public bool HasCurrency(int amount) => Amount >= amount;
    public bool TryUseCurrency(int amount)
    {
        if (HasCurrency(amount))
        {
            Amount -= amount;
            return true;
        }
        else return false;
    }
}

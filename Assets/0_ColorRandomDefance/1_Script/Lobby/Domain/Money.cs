using System;
using System.Collections;
using System.Collections.Generic;

public class Money
{
    public int Amount { get; private set; }
    public event Action<int> OnAmountChange;
    void ChangeAmount(int amount)
    {
        Amount = amount;
        OnAmountChange?.Invoke(Amount);
    }
    public Money(int amount) => Amount = amount;

    public void Add(int amount) => ChangeAmount(Amount + amount);

    public void Subtract(int amount)
    {
        if (Amount < amount) return;
        ChangeAmount(Amount - amount);
    }
}


using System.Collections;
using System.Collections.Generic;

public class Money
{
    public int Amount { get; private set; }

    public Money(int amount) => Amount = amount;

    public void Add(int amount) => Amount += amount;

    public void Subtract(int amount)
    {
        if (Amount < amount) return;
        Amount -= amount;
    }
}


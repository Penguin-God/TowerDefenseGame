using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    public readonly string Name;
    public readonly CurrencyUseCase GemUseCase;
    public readonly CurrencyUseCase GoldUseCase;
    public PlayerManager(string name, int gem, int gold)
    {
        Name = name;
        GemUseCase = new CurrencyUseCase(gem);
        GoldUseCase = new CurrencyUseCase(gold);
    }
}
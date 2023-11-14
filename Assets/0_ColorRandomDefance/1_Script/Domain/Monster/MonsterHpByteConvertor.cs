using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHpByteConvertor
{
    public byte CalculateHealthByte(int currentHealth, int maxHealth)
    {
        if(currentHealth <= 0) return 0;

        float baseValue = maxHealth / (float)byte.MaxValue;
        return (byte)Mathf.FloorToInt(currentHealth / baseValue);
    }
}

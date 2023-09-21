using System.Collections;
using System.Collections.Generic;

public class ManaUseCase
{
    readonly int MaxMana;
    int _currentMana;

    public bool IsManaFull => _currentMana >= MaxMana;

    public ManaUseCase(int maxMana)
    {
        MaxMana = maxMana;
    }

    public int AddMana(int manaAmount)
    {
        _currentMana += manaAmount;
        return _currentMana;
    }

    public void ClearMana() => _currentMana = 0;
}

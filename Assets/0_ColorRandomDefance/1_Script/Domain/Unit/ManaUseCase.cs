using System.Collections;
using System.Collections.Generic;

public class ManaUseCase
{
    readonly int MaxMana;
    int _currentMana;
    bool _isManaLock;

    public bool IsManaFull => _currentMana >= MaxMana;

    public ManaUseCase(int maxMana)
    {
        MaxMana = maxMana;
    }

    public int AddMana(int manaAmount)
    {
        if(_isManaLock == false)
            _currentMana += manaAmount;
        return _currentMana;
    }

    public void ClearMana() => _currentMana = 0;

    public void LockMana() => _isManaLock = true;
    public void ReleaseMana() => _isManaLock = false;
}

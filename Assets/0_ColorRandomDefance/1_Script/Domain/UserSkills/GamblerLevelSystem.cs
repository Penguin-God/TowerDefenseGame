using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GamblerLevelSystem
{
    readonly LevelSystem _levelSystem;

    public int Level => _levelSystem.Level;
    public event Action OnOverExp;
    public int Experince;

    bool IsOverExp => Experince >= _levelSystem.NeedExperienceForLevelUp;
    int MaxLevel => _levelSystem.MaxLevel - 1;
    bool IsMaxLevel => Level >= MaxLevel;

    public GamblerLevelSystem(LevelSystem levelSystem) => _levelSystem = levelSystem;

    public void AddExperience(int amount)
    {
        Experince += amount;
        _levelSystem.AddExperience(amount);
        
        if (_levelSystem.IsExpOver)
            OnOverExp?.Invoke();
    }

    public void LevelUp()
    {
        if (IsMaxLevel)
        {
            if (IsOverExp)
                Experince -= _levelSystem.NeedExperienceForLevelUp;
            return;
        }

        if (_levelSystem.LevelUp())
            Experince = _levelSystem.Experience;
    }
}

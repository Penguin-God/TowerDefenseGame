using System.Collections;
using System.Collections.Generic;
using System;

public class GamblerLevelSystem
{
    readonly LevelSystem _levelSystem;
    public int Level => _levelSystem.Level;

    int _experience;
    public int Experience
    {
        get => _experience;
        private set
        {
            _experience = value;
            OnChangeExp?.Invoke();
        }
    }
    public event Action OnChangeExp;

    bool IsOverExp => Experience >= _levelSystem.NeedExperienceForLevelUp;
    public event Action OnOverExp;

    int MaxLevel => _levelSystem.MaxLevel - 1;
    bool IsMaxLevel => Level >= MaxLevel;

    public GamblerLevelSystem(LevelSystem levelSystem) => _levelSystem = levelSystem;

    public void AddExperience(int amount)
    {
        Experience += amount;
        _levelSystem.AddExperience(amount);
        
        if (_levelSystem.IsExpOver)
            OnOverExp?.Invoke();
    }

    public void LevelUp()
    {
        if (IsMaxLevel)
        {
            if (IsOverExp)
                Experience -= _levelSystem.NeedExperienceForLevelUp;
        }
        else if (_levelSystem.LevelUp())
            Experience = _levelSystem.Experience;
    }
}

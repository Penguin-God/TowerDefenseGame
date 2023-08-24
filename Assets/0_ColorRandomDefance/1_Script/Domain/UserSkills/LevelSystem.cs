using System;
using System.Collections;
using System.Collections.Generic;

public class LevelSystem
{
    public int Level { get; private set; }
    public event Action<int> OnLevelUp;
    public bool LevelUpCondition => IsMaxLevel == false && Experience >= NeedExperienceForLevelUp;

    int _experience;
    public int Experience
    {
        get => _experience;
        private set
        {
            _experience = value;
            OnChangeExp?.Invoke(_experience);
        }
    }
    public event Action<int> OnChangeExp;

    public int NeedExperienceForLevelUp => ExperienceToNextLevel[Level - 1];
    public int[] ExperienceToNextLevel { get; private set; }
    public int MaxLevel => ExperienceToNextLevel.Length + 1;
    public bool IsMaxLevel => Level >= MaxLevel;
    public LevelSystem(int[] experienceToNextLevel)
    {
        Level = 1;
        Experience = 0;
        ExperienceToNextLevel = experienceToNextLevel;
    }

    public void AddExperience(int amount)
    {
        if (IsMaxLevel) return;
        Experience += amount;
    }

    public bool LevelUp()
    {
        if (LevelUpCondition == false) return false;

        Experience -= NeedExperienceForLevelUp; // 레벨업에 필요한 경험치를 차감합니다.
        Level++;
        OnLevelUp?.Invoke(Level);
        if (Level >= MaxLevel)
            Experience = 0;
        return true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;

public class LevelSystem
{
    public int Level { get; private set; }
    public event Action<int> OnLevelUp;

    int _experience;
    public int Experience
    {
        get => _experience;
        private set
        {
            _experience = value;
            OnChangeExp?.Invoke(_experience); // 경험치 변경 시 이벤트 호출
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

    public void AddExperience(int amount) // 경험치를 추가하고 레벨업을 확인합니다.
    {
        if (IsMaxLevel) return;

        Experience += amount;
        // 경험치가 2레벨 이상 업할 수준으로 많이 얻을 수 있기에 while문 사용
        while (Experience >= NeedExperienceForLevelUp)
        {
            LevelUp();
            if (IsMaxLevel) break;
        }
    }

    void LevelUp()
    {
        Experience -= NeedExperienceForLevelUp; // 레벨업에 필요한 경험치를 차감합니다.
        Level++;
        OnLevelUp?.Invoke(Level);
        if (Level >= MaxLevel)
            Experience = 0;
    }
}

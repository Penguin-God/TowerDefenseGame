using System.Collections;
using System.Collections.Generic;

public class LevelSystem
{
    public int Level { get; private set; }
    public int Experience { get; private set; }
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

    public void AddExperience(int amount) // ����ġ�� �߰��ϰ� �������� Ȯ���մϴ�.
    {
        if (IsMaxLevel) return;

        Experience += amount;
        // ����ġ�� 2���� �̻� ���� �������� ���� ���� �� �ֱ⿡ while�� ���
        while (Experience >= NeedExperienceForLevelUp)
        {
            LevelUp();
            if (IsMaxLevel) break;
        }
    }

    void LevelUp()
    {
        Experience -= NeedExperienceForLevelUp; // �������� �ʿ��� ����ġ�� �����մϴ�.
        Level++;
        if (Level >= MaxLevel)
            Experience = 0;
    }
}

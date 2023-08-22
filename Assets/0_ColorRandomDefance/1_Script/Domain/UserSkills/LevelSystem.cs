using System.Collections;
using System.Collections.Generic;

public class LevelSystem
{
    public int Level { get; private set; }
    public int Experience { get; private set; }
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
        while (Experience >= ExperienceToNextLevel[Level - 1])
        {
            LevelUp();
            if (IsMaxLevel) break;
        }
    }

    void LevelUp()
    {
        Experience -= ExperienceToNextLevel[Level - 1]; // �������� �ʿ��� ����ġ�� �����մϴ�.
        Level++;
        if (Level >= MaxLevel)
            Experience = 0;
    }
}

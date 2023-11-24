using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Drawing : MonoBehaviour
{
    public List<int> NumberChoice()
    {
        List<int> nums = new List<int>();
        int one = UnityEngine.Random.Range(50, 81);
        int two = UnityEngine.Random.Range(15, 41);
        int three = UnityEngine.Random.Range(10, 16);
        int four = 100 - one - two - three;

        while (four <= 0)
        {
            one = UnityEngine.Random.Range(50, 81);
            two = UnityEngine.Random.Range(15, 41);
            three = UnityEngine.Random.Range(10, 16);
            four = 100 - one - two - three;
        }

        nums.Add(one);
        nums.Add(two);
        nums.Add(three);
        nums.Add(four);

        nums.Sort();
        nums.Reverse();

        return nums;
    }

    public List<int> DrawingSkills()
    {
        //List<int> randomInt = NumberChoice();
        List<int> numbers = new List<int>();
        
        for (int i = 1; i <= Enum.GetValues(typeof(SkillType)).Length - 1; i++)
        {
            numbers.Add(i);
        }

        List<int> selectedNumbers = new List<int>();
        int count = 0;
        
        while (count < 4)
        {
            int index = UnityEngine.Random.Range(0, numbers.Count);

            int number = numbers[index];

            if (!selectedNumbers.Contains(number))
            {
                selectedNumbers.Add(number);
                //Managers.ClientData.GetExp((SkillType)number, randomInt[count]);
                count++;
            }

            numbers.RemoveAt(index);
        }

        Debug.Log(string.Join(", ", selectedNumbers));
        return selectedNumbers;

    }

    public void TestButton()
    {
        OpenBox(SkillBoxType.전설상자);
    }

    public void OpenBox(SkillBoxType boxType)
    {
        List<int> itemCountList = NumberChoice();
        List<int> selectedSkills = DrawingSkills();

        for (int i = 0; i <= selectedSkills.Count - 1; i++)
        {
            print($"{selectedSkills[i]}, {itemCountList[i] * (int)boxType}");
            Managers.ClientData.GetExp((SkillType)selectedSkills[i], itemCountList[i] * (int)boxType);
        }
    }

}

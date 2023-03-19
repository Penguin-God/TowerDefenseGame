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
        nums.Add(UnityEngine.Random.Range(50, 81)); 
        nums.Add(UnityEngine.Random.Range(15, 41));
        nums.Add(UnityEngine.Random.Range(10, 16)); 
        nums.Add(100 - nums[0] - nums[1] - nums[2]); 
        
        nums.Sort();
        nums.Reverse();

        return nums;
    }

    public List<int> DrawingSkills()
    {
        //List<int> randomInt = NumberChoice();
        List<int> numbers = new List<int>();
        
        for (int i = 1; i <= Enum.GetValues(typeof(SkillType)).Length; i++)
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
        print(DrawingSkills()[0]);
    }

    public void Test()
    {
        List<int> itemCountList = NumberChoice();
        List<int> selectedNumbers = DrawingSkills();

        for (int i = 0; i <= selectedNumbers.Count; i++)
        {
            Managers.ClientData.GetExp((SkillType)selectedNumbers[i], itemCountList[i]);
            print(selectedNumbers[i] + itemCountList[i]); // 임시
        }
    }

}

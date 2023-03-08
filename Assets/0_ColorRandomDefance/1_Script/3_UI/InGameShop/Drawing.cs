﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Drawing : MonoBehaviour
{
    
    public List<int> DrawingSkills()
    {
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
                count++;
            }

            numbers.RemoveAt(index);

            Managers.ClientData.GetExp((SkillType)number, 10); 
        }

        Debug.Log(string.Join(", ", selectedNumbers));
        return null;


    }

}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateDefenser : MonoBehaviour
{
    // public GameObject Soldierprefab;
    private GameObject Soldier;
    
    void Start()
    {
      gameObject.SetActive(true);
    }

    public void create()
    {
        int randomnumber = Random.Range(0, 4);
        // Soldier = transform.GetChild(randomnumber).gameObject;
        Soldier = Instantiate(transform.GetChild(randomnumber).gameObject, transform.position, transform.rotation);

        Soldier.transform.position = RandomPosition(10, 0 ,10);
        Soldier.SetActive(true);
    }

    Vector3 RandomPosition(float x, float y, float z)
    {
        float randomX = Random.Range(-x, x);
        float randomY = Random.Range(-y, y);
        float randomZ = Random.Range(-z, z);
        return new Vector3(randomX, randomY, randomZ);
    }
}

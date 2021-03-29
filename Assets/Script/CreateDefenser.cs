using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateDefenser : MonoBehaviour
{
    private GameObject Soldier;
    
    
    void Start()
    {
      gameObject.SetActive(true);
    }

    
    void Update()
    {
        
    }

    public void create()
    {
        int randomnumber = Random.Range(0, 4);
        Soldier = transform.GetChild(randomnumber).gameObject;

        Soldier.transform.position = new Vector3(-Random.Range(20, 40), 0, Random.Range(90, 110));
        Soldier.SetActive(true);
    }
}

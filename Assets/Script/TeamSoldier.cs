using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamSoldier : MonoBehaviour
{
    public Button Soldierclick;
    private GameObject CombineSoldier;
    void Start()
    {
        Soldierclick.GetComponent<Button>();
    }

    
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Soldierclick.gameObject.SetActive(true);
    }

    public void Combine()
    {
        
        Soldierclick.gameObject.SetActive(false);

    }
}

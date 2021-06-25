using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipScrips : MonoBehaviour
{
    public GameObject Tips;
    void Start()
    {
        int RandomNumber = Random.Range(0, Tips.transform.childCount);
        Tips.transform.GetChild(RandomNumber).gameObject.SetActive(true);
    }


}

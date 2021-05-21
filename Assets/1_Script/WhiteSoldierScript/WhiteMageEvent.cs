using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteMageEvent : MonoBehaviour
{
    public float timer;
    public CreateDefenser createDefenser;
    public SoldiersTags soldiersTags;
    private int Colornumber;


    private void Start()
    {
        Colornumber = Random.Range(0, 5);


    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 10f)
        {
            soldiersTags.WhiteMageTag();
            Destroy(soldiersTags.WhiteMage[0]);
            createDefenser.CreateSoldier(Colornumber, 3);
            timer = 0f;
            return;

        }

    }
}

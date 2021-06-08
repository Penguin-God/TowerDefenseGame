using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteSwordmanEvent : MonoBehaviour
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
        if (timer >= 30f)
        {
            soldiersTags.WhiteSwordmanTag();
            Destroy(soldiersTags.WhiteSwordman[0]);
            createDefenser.CreateSoldier(Colornumber, 0);
            timer = 0f;
            return;

        }

    }
}

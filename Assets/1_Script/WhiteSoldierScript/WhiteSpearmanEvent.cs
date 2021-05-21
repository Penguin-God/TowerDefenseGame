using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteSpearmanEvent : MonoBehaviour
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
            soldiersTags.WhiteSpearmanTag();
            Destroy(soldiersTags.WhiteSpearman[0]);
            createDefenser.CreateSoldier(Colornumber, 2);
            timer = 0f;
            return;

        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCombine : MonoBehaviour
{
    public List<Queue<GameObject>> colorsQueue;

    private int Colornumber;
    private int Soldiernumber;
    public TestCreateUnit testCreateUnit;

    private void Start()
    {
        colorsQueue = new List<Queue<GameObject>>();
        colorsQueue.Add(testCreateUnit.blueSowrdman);
        colorsQueue.Add(testCreateUnit.yellowSowrdman);
        colorsQueue.Add(testCreateUnit.greenSowrdman);
        colorsQueue.Add(testCreateUnit.orangeSowrdman);
        colorsQueue.Add(testCreateUnit.violetSowrdman);
    }

    int SetCombineColor(string color)
    {
        int colorNumber = -1;
        switch (color)
        {
            case "Bule":
                colorNumber = 0;
                break;
            case "Yellow":
                colorNumber = 1;
                break;
            case "Green":
                colorNumber = 2;
                break;
            case "Orange":
                colorNumber = 3;
                break;
            case "Violet":
                colorNumber = 4;
                break;
        }
        return colorNumber;
    }

    public void CombineUnit()
    {
        int colorNumber = -1;
        TeamSoldier teamSoldier = GameManager.instance.HitEnemy.GetComponent<TeamSoldier>();
        //if (teamSoldier != null) colorNumber = SetCombineColor(teamSoldier.unitColor);
        if (colorNumber != -1 && colorsQueue[colorNumber].Count >= 2) // 나중에 들어가는 유닛수를 변수화 시켜서 2마리보다 많은 유닛 조합가능
        {
            for(int i = 0; i < 2; i++) 
            {
                GameObject removeUnit = colorsQueue[colorNumber].Dequeue(); // 맨처음을 뺌
                Destroy(removeUnit);
            }

            testCreateUnit.CombineCreateSoldier();
            //UIManager.instance.SetActiveButton(false);
        }
    }
}

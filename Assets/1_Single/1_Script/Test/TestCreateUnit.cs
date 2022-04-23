using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TestCreateUnit : MonoBehaviour
{
    GameObject Soldier;
    Transform[] unitColors;

    // 급식줄
    public Queue<GameObject> blueSowrdman;
    public Queue<GameObject> yellowSowrdman;
    public Queue<GameObject> greenSowrdman;
    public Queue<GameObject> orangeSowrdman;
    public Queue<GameObject> violetSowrdman;

    private void Awake()
    {
        // 추가
        blueSowrdman = new Queue<GameObject>();
        yellowSowrdman = new Queue<GameObject>();
        greenSowrdman = new Queue<GameObject>();
        orangeSowrdman = new Queue<GameObject>();
        violetSowrdman = new Queue<GameObject>();
    }

    void Start()
    {
        unitColors = new Transform[transform.childCount];
        for (int i = 0; i < unitColors.Length; i++)
        {
            unitColors[i] = transform.GetChild(i);
        }
        gameObject.SetActive(true);
    }

    public void TestDrawSoldier()
    {
        if (GameManager.instance.Gold >= 5)
        {
            RandomCreateSoldier(0);
            ExpenditureGold();
        }
    }

    int SetColor()
    {
        int randomColor = Random.Range(0, 5);
        return randomColor;
    }

    public void RandomCreateSoldier(int Soldiernumber)
    {
        int unitColorParent = SetColor();
        Soldier = Instantiate(unitColors[unitColorParent].GetChild(Soldiernumber).gameObject, transform.position, transform.rotation);

        Soldier.transform.position = RandomPosition(10, 0, 10);
        Soldier.SetActive(true);
        AddQueue(unitColorParent, Soldier);
    }

    public void CombineCreateSoldier()
    {
        GameObject solider = Instantiate(unitColors[1].GetChild(0).gameObject, transform.position, transform.rotation);
        solider.transform.position = RandomPosition(10, 0, 10);
        solider.SetActive(true);
    }

    void AddQueue(int colorNumber, GameObject addUnit)
    {
        switch (colorNumber)
        {
            case 0:
                blueSowrdman.Enqueue(addUnit); // addUnit을 넣음
                break;
            case 1:
                yellowSowrdman.Enqueue(addUnit);
                break;
            case 2:
                greenSowrdman.Enqueue(addUnit);
                break;
            case 3:
                orangeSowrdman.Enqueue(addUnit);
                break;
            case 4:
                violetSowrdman.Enqueue(addUnit);
                break;
        }
    }

    public void ExpenditureGold()
    {
        GameManager.instance.Gold -= 5;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
    }

    Vector3 RandomPosition(float x, float y, float z)
    {
        float randomX = Random.Range(-x, x);
        float randomY = Random.Range(-y, y);
        float randomZ = Random.Range(-z, z);
        return new Vector3(randomX, randomY, randomZ);
    }


}